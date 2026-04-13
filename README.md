# ParaqueserveLUDUSAcompanha

# LUDUS Monitor SDK

> Parte do projeto **LUDUS Acompanha** — Mestrado em Ciência da Computação, UFPel (2026)  
> Autor: Rodrigo Leitzke Bichet

---

## O que é

O **LUDUS Monitor SDK** é um conjunto de scripts Unity (C#) que adiciona monitoramento automático de gameplay a qualquer jogo da plataforma **Mais LUDUS**. Ele coleta dados de interação das crianças (cliques, acertos, erros, inatividade, trajetória do mouse) e os envia para um backend Node.js, onde ficam disponíveis para análise em um dashboard para professores e tutores.

O SDK funciona como uma "máscara" plugável — o jogo não precisa ter sua lógica alterada, apenas chama os métodos públicos do SDK nos pontos relevantes.

---

## Jogos monitorados

| Jogo                   | Plataforma      | Status           |
| ---------------------- | --------------- | ---------------- |
| Para Que Serve?        | WebGL + Android | ✅ Em integração |
| Historietas Divertidas | WebGL + Android | 🔜 Futuro        |

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
│   └── LUDUS_SDK/
│       ├── LudusConfig.cs        ← Configuração (ScriptableObject)
│       ├── LudusSession.cs       ← Modelo de dados da sessão
│       ├── LudusMonitor.cs       ← Singleton orquestrador
│       ├── LudusGameEvents.cs    ← API pública do SDK ✅
│       ├── LudusInputTracker.cs  ← Captura de mouse/touch (em desenvolvimento)
│       ├── LudusClickable.cs     ← Nomeação semântica de objetos (em desenvolvimento)
│       └── LudusExporter.cs      ← Serialização e envio HTTP (em desenvolvimento)
└── Resources/
    └── LUDUS_SDK/
        └── LudusConfig.asset     ← Asset de configuração (editável no Inspector)
```

---

## Como instalar em um novo jogo

1. Copie a pasta `Assets/Scripts/LUDUS_SDK/` para dentro do projeto Unity de destino
2. Copie a pasta `Assets/Resources/LUDUS_SDK/` para dentro do projeto
3. Crie um GameObject vazio na primeira cena do jogo e adicione o componente `LudusMonitor`
4. Configure o `LudusConfig.asset` em `Resources/LUDUS_SDK/` com os dados do jogo

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
LudusGameEvents.CategorySelected("Alimentos");

// Nova fase iniciada
LudusGameEvents.PhaseStarted("maçã", new string[] { "maçã", "bola", "carro", "peixe" });

// Criança arrastou um item (correto ou errado)
LudusGameEvents.DragAttempt("bola", "maçã", false);  // errou
LudusGameEvents.DragAttempt("maçã", "maçã", true);   // acertou

// Registrar acerto ou erro separadamente
LudusGameEvents.CorrectMatch("maçã", 3.5f);
LudusGameEvents.WrongMatch("bola", "maçã");

// Fase concluída com resumo de desempenho
LudusGameEvents.PhaseCompleted(acertos: 3, erros: 1, timeSeconds: 45.2f, stars: 2);
```

---

## Eventos semânticos (Para Que Serve?)

| Evento                                        | Quando disparar                 |
| --------------------------------------------- | ------------------------------- |
| `CategorySelected(category)`                  | Criança escolhe uma categoria   |
| `PhaseStarted(target, options[])`             | Nova fase iniciada              |
| `DragAttempt(item, target, correct)`          | Criança arrasta qualquer item   |
| `CorrectMatch(item, timeSeconds)`             | Pareamento correto              |
| `WrongMatch(item, expected)`                  | Pareamento incorreto            |
| `PhaseCompleted(acertos, erros, time, stars)` | Fase concluída                  |
| `InactivityDetected`                          | Automático — disparado pelo SDK |
| `SessionEnded()`                              | Encerra sessão e envia dados    |

---

## Status do desenvolvimento

| Componente           | Status                |
| -------------------- | --------------------- |
| LudusConfig.cs       | ✅ Concluído          |
| LudusSession.cs      | ✅ Concluído          |
| LudusMonitor.cs      | ✅ Concluído          |
| LudusGameEvents.cs   | ✅ Concluído          |
| LudusInputTracker.cs | 🔧 Em desenvolvimento |
| LudusClickable.cs    | 🔧 Em desenvolvimento |
| LudusExporter.cs     | 🔜 Pendente           |

---

## Contexto acadêmico

Este SDK é parte da dissertação de mestrado **"LUDUS Acompanha — Uma Ferramenta para Monitoramento e Análise de Dados de Interação em Jogos Educacionais para Auxílio a Professores e Tutores"**, desenvolvida no Programa de Pós-Graduação em Ciência da Computação da UFPel.
