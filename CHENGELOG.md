# Changelog — LUDUS Monitor SDK

Todas as mudanças relevantes do projeto são registradas aqui.  
Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/).

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

- `[0.2.0]` — `LudusGameEvents.cs`: API pública estática com eventos semânticos do _Para Que Serve?_
- `[0.3.0]` — `LudusInputTracker.cs` + `LudusClickable.cs`: captura global de input e nomeação semântica
- `[0.4.0]` — `LudusExporter.cs`: serialização JSON e envio HTTP com fallback offline
- `[0.5.0]` — Cena de identificação do jogador integrada ao SDK
- `[1.0.0]` — SDK completo integrado ao _Para Que Serve?_ e testado em ambiente real
