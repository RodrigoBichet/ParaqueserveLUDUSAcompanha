# Changelog — LUDUS Monitor SDK

Todas as mudanças relevantes do projeto são registradas aqui.  
Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/).

---

## [0.2.0] — 2026-04-13

### Adicionado

- `LudusGameEvents.cs` — API pública estática do SDK
    - `CategorySelected(category)` — criança escolhe uma categoria
    - `PhaseStarted(targetItem, options[])` — nova fase iniciada com item-alvo e opções
    - `DragAttempt(draggedItem, targetItem, correct)` — criança arrasta qualquer item
    - `CorrectMatch(item, timeSeconds)` — pareamento correto com tempo da fase
    - `WrongMatch(draggedItem, expectedItem)` — pareamento incorreto
    - `PhaseCompleted(acertos, erros, timeSeconds, stars)` — resumo de desempenho da fase
    - `SessionEnded()` — encerra sessão e aciona o Monitor
    - `ValidarMonitor()` — verificação interna antes de cada chamada com mensagens de erro claras
    - Payloads em JSON com `InvariantCulture` nos floats (ponto decimal garantido)
    - Atualização automática de `totalCorrect` e `totalWrong` nas métricas da sessão

### Testado

- Todos os eventos disparando corretamente em sequência
- Payloads JSON válidos para cada evento
- Sessão encerrada corretamente ao final do fluxo completo

---

## [0.1.0] — 2026-04-12

### Adicionado

- `LudusConfig.cs` — ScriptableObject de configuração do SDK
    - Campos: `gameId`, `gameVersion`, `backendUrl`, `sendOnSessionEnd`, `enableLocalFallback`, `fallbackFolderName`, `inactivityThresholdSeconds`, `debugMode`
    - Criação via menu `Assets → Create → LUDUS → Configuração SDK`
- `LudusSession.cs` — modelo de dados da sessão em memória
    - Classes: `LudusSession`, `LudusMetrics`, `LudusClickEvent`, `LudusPathPoint`, `LudusGameEvent`
    - Geração automática de UUID por sessão via `Guid.NewGuid()`
    - Timestamps em formato ISO 8601
    - Detecção automática de plataforma (WebGL / Android)
- `LudusMonitor.cs` — Singleton DontDestroyOnLoad orquestrador do SDK
    - Carregamento automático do `LudusConfig.asset` via `Resources.Load`
    - `StartSession(playerId)` — inicia sessão com ID do jogador
    - `EndSession()` — encerra sessão e calcula duração
    - `RegistrarAcao()` — registra interação e atualiza métricas
    - `RegistrarEvento(eventType, payload)` — adiciona evento semântico à sessão
    - `RegistrarClique(element, x, y)` — registra clique/toque com posição
    - `RegistrarPontoPath(x, y)` — registra ponto do caminho para heatmap
    - `VerificarInatividade()` — detecção automática por threshold configurável
    - Gancho para `LudusExporter` comentado (a ser conectado futuramente)
- Estrutura de pastas do SDK definida:
    - `Assets/Scripts/LUDUS_SDK/` — scripts
    - `Assets/Resources/LUDUS_SDK/` — assets de configuração

### Testado

- Configuração carregada corretamente via `Resources.Load`
- Sessão iniciada com UUID único gerado automaticamente
- Logs de debug funcionando no Console do Unity

---

## Próximas versões planejadas

- `[0.3.0]` — `LudusInputTracker.cs` + `LudusClickable.cs`: captura global de input e nomeação semântica
- `[0.4.0]` — `LudusExporter.cs`: serialização JSON e envio HTTP com fallback offline
- `[0.5.0]` — Cena de identificação do jogador integrada ao SDK
- `[1.0.0]` — SDK completo integrado ao _Para Que Serve?_ e testado em ambiente real
