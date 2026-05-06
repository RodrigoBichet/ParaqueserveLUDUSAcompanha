# Para Que Serve? — LUDUS Monitor SDK

> Parte do projeto **LUDUS Acompanha** — Mestrado em Ciência da Computação, UFPel (2026)  
> Autor: Rodrigo Leitzke Bichet  
> Orientador: Prof. Dr. Leomar Soares da Rosa Júnior

---

## O que é

O **Para Que Serve?** é um jogo educacional desenvolvido em Unity para a plataforma **Mais LUDUS**, voltado para crianças com necessidades educacionais específicas (TEA). Este repositório contém o jogo integrado ao **LUDUS Monitor SDK** — um sistema de monitoramento de gameplay que coleta dados de interação e os envia para análise em um dashboard para professores e tutores.

> ⚠️ **Princípio fundamental:** O LUDUS Acompanha é uma ferramenta de apoio pedagógico. Fornece dados e indicadores para auxiliar professores e tutores nas suas observações. **Nunca substitui avaliação profissional e nunca emite diagnósticos.**

---

## Arquitetura geral do projeto

```
Unity (C# SDK) → JSON → Node.js + Express → MongoDB → API REST → Dashboard React
```

---

## Estrutura do SDK

```
Assets/
├── Scripts/
│   ├── LUDUS_SDK/
│   │   ├── LudusConfig.cs        ← Configuração (ScriptableObject)
│   │   ├── LudusSession.cs       ← Modelo de dados da sessão
│   │   ├── LudusMonitor.cs       ← Singleton orquestrador
│   │   ├── LudusGameEvents.cs    ← API pública do SDK
│   │   ├── LudusInputTracker.cs  ← Captura global de mouse/touch
│   │   ├── LudusClickable.cs     ← Nomeação semântica de objetos
│   │   └── LudusExporter.cs      ← Serialização e envio HTTP
│   └── (scripts do jogo)
│       ├── IdentificacaoController.cs  ← Seleção de aluno em cascata
│       ├── Menu.cs                     ← Navegação, NovaSessaoCategoria e VoltarIdentificacao
│       ├── SceneControl.cs             ← PhaseStarted + PhaseCompleted
│       ├── ItemColado.cs               ← CorrectMatch + WrongMatch
│       └── SoundControl.cs             ← Controle de volume persistente
└── Resources/
    └── LUDUS_SDK/
        └── LudusConfig.asset     ← Asset de configuração
```

---

## Cenas do jogo

| Índice | Cena          | Descrição                        |
| ------ | ------------- | -------------------------------- |
| 0      | Identificacao | Seleção de escola, turma e aluno |
| 1      | Menu          | Menu principal                   |
| 2      | SelectLevel   | Seleção de categoria             |
| 3      | Tutorial      | Tutorial do jogo                 |
| 4      | Fase01        | Categoria: Ações                 |
| 5      | Fase02        | Categoria: Alimentos             |
| 6      | Fase03        | Categoria: Cotidiano             |
| 7      | Fase04        | Categoria: Diversão              |
| 8      | Fase05        | Categoria: Higiene               |

---

## Como configurar o SDK

1. Seleciona o `LudusConfig.asset` em `Resources/LUDUS_SDK/` no Inspector
2. Configura o `backendUrl` com a URL do servidor Node.js
3. O GameObject do SDK deve estar na cena `Identificacao`

| Campo                        | Descrição             | Padrão                  |
| ---------------------------- | --------------------- | ----------------------- |
| `gameId`                     | Identificador do jogo | `para-que-serve`        |
| `gameVersion`                | Versão do jogo        | `1.0.0`                 |
| `backendUrl`                 | URL do servidor       | `http://localhost:3000` |
| `sendOnSessionEnd`           | Envia ao encerrar     | `true`                  |
| `enableLocalFallback`        | Salva offline         | `true`                  |
| `inactivityThresholdSeconds` | Threshold inatividade | `10`                    |
| `debugMode`                  | Logs no Console       | `true`                  |

---

## Elementos com LudusClickable

| Elemento                    | elementName                                            |
| --------------------------- | ------------------------------------------------------ |
| Botão jogar (identificação) | `btn_jogar_identificacao`                              |
| Botão retry (identificação) | `btn_retry_identificacao`                              |
| Botão jogar (menu)          | `btn_menu_jogar`                                       |
| Botões de categoria         | `btn_categoria_acoes`, `btn_categoria_alimentos`, etc. |
| Botão avançar canvas        | `btn_avancar_canvas`                                   |
| Botão avançar feedback      | `btn_avancar`                                          |
| Sliders de volume           | `slider_volume`                                        |

---

## Fluxo completo de uma sessão

```
Jogo abre (cena Identificacao)
    ↓
LudusExporter verifica e reenvia sessões pendentes
    ↓
Professor seleciona Escola → Turma → Aluno
    ↓
LudusGameEvents.DefinirJogador("nome do aluno", capturaSolicitada)   ← registra aluno e solicitação de imagens
    ↓
Criança seleciona categoria
    ↓
LudusGameEvents.NovaSessaoCategoria("Ações")      ← inicia sessão + registra categoria
    ↓
Se houver captura solicitada → SDK salva imagens por fase para o heatmap
    ↓
Criança joga — LudusGameEvents registra cada ação
    ↓
LudusInputTracker registra cliques e caminho automaticamente
    ↓
Criança completa os 4 Canvas → PhaseCompleted
    ↓
Professor clica em avançar no feedback → SessionEnded()
    ↓
LudusExporter envia JSON ao backend
    ↓ (se falhar)
Salva localmente em persistentDataPath/ludus_offline/
    ↓
Criança seleciona nova categoria → NovaSessaoCategoria() → nova sessão independente
    ↓ (troca de aluno)
Professor clica "Trocar Aluno" → VoltarIdentificacao() → sessão encerrada + dados limpos
```

---

## Captura de imagens para heatmap

Quando o professor ativa **Imagens no mapa de calor** no dashboard, a tela de identificação recebe o campo `capturaSolicitada` junto com os dados do aluno. O SDK guarda essa informação e, na próxima sessão/categoria, captura uma imagem de cada uma das quatro fases.

Na primeira fase da categoria, o SDK aguarda a animação inicial terminar antes do print. Durante esse pequeno intervalo, a interação e o rastreamento bruto ficam bloqueados para evitar imagens com item sendo arrastado ou dados de heatmap contaminados. Nas fases seguintes, a captura acontece imediatamente no início da fase.

As imagens são enviadas em base64 dentro do JSON da sessão. O backend salva os arquivos em `backend/uploads/screenshots/` e desativa automaticamente a solicitação após receber a sessão.
## Eventos semânticos implementados

| Evento                                        | Quando dispara                      |
| --------------------------------------------- | ----------------------------------- |
| `DefinirJogador(playerId, capturaSolicitada)` | Aluno selecionado na identificação  |
| `NovaSessaoCategoria(category)`               | Criança escolhe categoria           |
| `PhaseStarted(target, options[])`             | Novo Canvas de fase ativado         |
| `DragAttempt(item, target, correct)`          | Criança arrasta item                |
| `CorrectMatch(item, timeSeconds)`             | Pareamento correto                  |
| `WrongMatch(item, expected)`                  | Pareamento incorreto                |
| `PhaseCompleted(acertos, erros, time, stars)` | 4 Canvas completados                |
| `InactivityDetected`                          | Automático — threshold configurável |
| `SessionEnded()`                              | Encerra sessão e envia dados        |

---

## Status do desenvolvimento

### SDK Unity

| Componente           | Status |
| -------------------- | ------ |
| LudusConfig.cs       | ✅     |
| LudusSession.cs      | ✅     |
| LudusMonitor.cs      | ✅     |
| LudusGameEvents.cs   | ✅     |
| LudusInputTracker.cs | ✅     |
| LudusClickable.cs    | ✅     |
| LudusExporter.cs     | ✅     |

### Integração no Para Que Serve?

| Componente                                            | Status |
| ----------------------------------------------------- | ------ |
| Tela de identificação com seleção em cascata          | ✅     |
| Feedback de erro e botão retry                        | ✅     |
| LudusClickable em todos os botões e sliders           | ✅     |
| DefinirJogador na tela de identificação               | ✅     |
| NovaSessaoCategoria nos botões de categoria           | ✅     |
| PhaseStarted / PhaseCompleted no SceneControl         | ✅     |
| DragAttempt / CorrectMatch / WrongMatch no ItemColado | ✅     |
| SessionEnded ao retornar para SelectLevel             | ✅     |
| Sessão independente por categoria jogada              | ✅     |
| Troca de aluno com reset de dados e sessão            | ✅     |
| Volume persistente corrigido entre alunos             | ✅     |
| Captura sob demanda de imagens para mapa de calor      | ✅     |

---

## Contexto acadêmico

Este SDK é parte da dissertação de mestrado **"LUDUS Acompanha — Uma Ferramenta para Monitoramento e Análise de Dados de Interação em Jogos Educacionais para Auxílio a Professores e Tutores"**, desenvolvida no Programa de Pós-Graduação em Ciência da Computação da UFPel.
