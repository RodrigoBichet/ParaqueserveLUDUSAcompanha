# ParaqueserveLUDUSAcompanha

# LUDUS Monitor SDK

> Parte do projeto **LUDUS Acompanha** — Mestrado em Ciência da Computação, UFPel (2026)  
> Autor: Rodrigo Leitzke Bichet  
> Orientador: Prof. Dr. Leomar Soares da Rosa Júnior

---

## O que é

O **LUDUS Monitor SDK** é um conjunto de scripts Unity (C#) que adiciona monitoramento automático de gameplay a qualquer jogo da plataforma **Mais LUDUS**. Ele coleta dados de interação das crianças (cliques, acertos, erros, inatividade, trajetória do mouse) e os envia para um backend Node.js, onde ficam disponíveis para análise em um dashboard para professores e tutores.

O SDK funciona como uma "máscara" plugável — o jogo não precisa ter sua lógica alterada, apenas chama os métodos públicos do SDK nos pontos relevantes.

---

## Jogos monitorados

| Jogo                   | Plataforma      | Status       |
| ---------------------- | --------------- | ------------ |
| Para Que Serve?        | WebGL + Android | ✅ Integrado |
| Historietas Divertidas | WebGL + Android | 🔜 Futuro    |

---

## Arquitetura geral do projeto

```
Unity (C# SDK) → JSON → Node.js + Express → MongoDB → API REST → Python ML → Dashboard React
```

---

## Estrutura do SDK

```
Assets/
├── Scripts/
│   ├── LUDUS_SDK/
│   │   ├── LudusConfig.cs        ← Configuração (ScriptableObject)          ✅
│   │   ├── LudusSession.cs       ← Modelo de dados da sessão                ✅
│   │   ├── LudusMonitor.cs       ← Singleton orquestrador                   ✅
│   │   ├── LudusGameEvents.cs    ← API pública do SDK                       ✅
│   │   ├── LudusInputTracker.cs  ← Captura global de mouse/touch            ✅
│   │   ├── LudusClickable.cs     ← Nomeação semântica de objetos            ✅
│   │   └── LudusExporter.cs      ← Serialização e envio HTTP                ✅
│   └── ParaQueServe/
│       ├── IdentificacaoController.cs  ← Identificação do jogador           ✅
│       ├── Menu.cs                     ← Seleção de categoria               ✅
│       ├── SceneControl.cs             ← Controle de fases                  ✅
│       └── ItemColado.cs               ← Detecção de acerto/erro            ✅
└── Resources/
    └── LUDUS_SDK/
        └── LudusConfig.asset     ← Asset de configuração (editável no Inspector)
```

---

## Cenas do jogo

| Índice | Cena          | Descrição                        |
| ------ | ------------- | -------------------------------- |
| 0      | Identificacao | Tela de identificação do jogador |
| 1      | Menu          | Menu principal                   |
| 2      | SelectLevel   | Seleção de categoria             |
| 3      | Tutorial      | Tutorial do jogo                 |
| 4      | Fase01        | Categoria: Ações                 |
| 5      | Fase02        | Categoria: Alimentos             |
| 6      | Fase03        | Categoria: Cotidiano             |
| 7      | Fase04        | Categoria: Diversão              |
| 8      | Fase05        | Categoria: Higiene               |

---

## Como instalar em um novo jogo

1. Copie a pasta `Assets/Scripts/LUDUS_SDK/` para dentro do projeto Unity de destino
2. Copie a pasta `Assets/Resources/LUDUS_SDK/` para dentro do projeto
3. Crie um GameObject vazio na primeira cena do jogo
4. Adicione os componentes `LudusMonitor`, `LudusInputTracker` e `LudusExporter` nesse GameObject
5. Configure o `LudusConfig.asset` em `Resources/LUDUS_SDK/` com os dados do jogo
6. Nos scripts do jogo, adicione as chamadas de `LudusGameEvents` nos pontos relevantes

---

## Como configurar

Selecione o arquivo `LudusConfig.asset` em `Resources/LUDUS_SDK/` no Unity Inspector:

| Campo                        | Descrição                         | Padrão                  |
| ---------------------------- | --------------------------------- | ----------------------- |
| `gameId`                     | Identificador único do jogo       | `para-que-serve`        |
| `gameVersion`                | Versão atual do jogo              | `1.0.0`                 |
| `backendUrl`                 | URL do servidor Node.js           | `http://localhost:3000` |
| `sendOnSessionEnd`           | Envia dados ao encerrar sessão    | `true`                  |
| `enableLocalFallback`        | Salva localmente se offline       | `true`                  |
| `fallbackFolderName`         | Nome da pasta de fallback local   | `ludus_offline`         |
| `inactivityThresholdSeconds` | Segundos até detectar inatividade | `10`                    |
| `debugMode`                  | Logs no Console do Unity          | `true`                  |

---

## Como usar

### 1. Iniciar e encerrar sessão

```csharp
// Iniciar sessão (chamar na cena de identificação do jogador)
LudusMonitor.Instance.StartSession("nome_do_jogador");

// Encerrar sessão (chamar ao sair do jogo ou finalizar partida)
LudusGameEvents.SessionEnded();
```

### 2. Disparar eventos semânticos

```csharp
// Criança escolheu uma categoria
LudusGameEvents.CategorySelected("Fase01");

// Nova fase iniciada
LudusGameEvents.PhaseStarted("Canvas02", new string[] { });

// Criança arrastou um item
LudusGameEvents.DragAttempt("cadeira", "praqueserve", true);

// Registrar acerto ou erro
LudusGameEvents.CorrectMatch("cadeira", 27.41f);
LudusGameEvents.WrongMatch("bola", "praqueserve");

// Fase concluída
LudusGameEvents.PhaseCompleted(acertos: 4, erros: 0, timeSeconds: 45.2f, stars: 3);
```

### 3. Nomear objetos interativos

Adicione o componente `LudusClickable` em qualquer botão ou objeto interativo e defina o `elementName`. O `LudusInputTracker` detecta o clique automaticamente.

---

## Fluxo completo de uma sessão

```
Jogo abre (cena Identificacao)
    ↓
LudusExporter verifica e reenvia sessões pendentes
    ↓
Professor digita nome da criança → BotaoJogar()
    ↓
LudusMonitor.StartSession("nome")
    ↓
Criança navega pelo Menu → SelectLevel
    ↓
Criança escolhe categoria → CategorySelected("Fase01")
    ↓
Canvas aleatório ativado → PhaseStarted("Canvas02")
    ↓
Criança arrasta item → DragAttempt + CorrectMatch ou WrongMatch
    ↓
4 acertos → PhaseCompleted(acertos, erros, tempo, estrelas)
    ↓
Criança retorna ao SelectLevel ou encerra
    ↓
SessionEnded() → LudusExporter envia JSON ao backend
    ↓ (se falhar)
Salva localmente em persistentDataPath/ludus_offline/
```

---

## Eventos semânticos (Para Que Serve?)

| Evento                                        | Quando dispara                      |
| --------------------------------------------- | ----------------------------------- |
| `CategorySelected(category)`                  | Criança escolhe uma categoria       |
| `PhaseStarted(target, options[])`             | Novo Canvas de fase ativado         |
| `DragAttempt(item, target, correct)`          | Criança arrasta qualquer item       |
| `CorrectMatch(item, timeSeconds)`             | Pareamento correto                  |
| `WrongMatch(item, expected)`                  | Pareamento incorreto                |
| `PhaseCompleted(acertos, erros, time, stars)` | 4 Canvas completados                |
| `InactivityDetected`                          | Automático — threshold configurável |
| `SessionEnded()`                              | Encerra sessão e envia dados        |

---

## Status do desenvolvimento

### Etapa 1 — SDK Unity ✅

| Componente           | Status |
| -------------------- | ------ |
| LudusConfig.cs       | ✅     |
| LudusSession.cs      | ✅     |
| LudusMonitor.cs      | ✅     |
| LudusGameEvents.cs   | ✅     |
| LudusInputTracker.cs | ✅     |
| LudusClickable.cs    | ✅     |
| LudusExporter.cs     | ✅     |

### Etapa 1.5 — Integração no Para Que Serve? ✅

| Componente                                            | Status |
| ----------------------------------------------------- | ------ |
| Cena de identificação do jogador                      | ✅     |
| CategorySelected nos botões de categoria              | ✅     |
| PhaseStarted / PhaseCompleted no SceneControl         | ✅     |
| DragAttempt / CorrectMatch / WrongMatch no ItemColado | ✅     |

### Próximas etapas

| Etapa   | Descrição                            | Status |
| ------- | ------------------------------------ | ------ |
| Etapa 2 | Backend Node.js + Express + MongoDB  | 🔜     |
| Etapa 3 | Dashboard React                      | 🔜     |
| Etapa 4 | Análise ML com Python + scikit-learn | 🔜     |

---

## Contexto acadêmico

Este SDK é parte da dissertação de mestrado **"LUDUS Acompanha — Uma Ferramenta para Monitoramento e Análise de Dados de Interação em Jogos Educacionais para Auxílio a Professores e Tutores"**, desenvolvida no Programa de Pós-Graduação em Ciência da Computação da UFPel.
