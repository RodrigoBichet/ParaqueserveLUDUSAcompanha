# Changelog — LUDUS Monitor SDK

Todas as mudanças relevantes do projeto são registradas aqui.  
Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/).

---

## [0.4.0] — 2026-04-14 — SDK Completo 🎉

### Adicionado

- `LudusExporter.cs` — serialização e envio de sessões ao backend
    - `Exportar(session)` — serializa a sessão em JSON via `JsonUtility.ToJson()` e envia via HTTP POST
    - `EnviarParaBackend()` — Coroutine com `UnityWebRequest` para envio assíncrono sem travar o jogo
    - `SalvarLocalmente()` — fallback offline: salva JSON em `persistentDataPath/ludus_offline/` com UUID como nome do arquivo
    - `TentarReenviarPendentes()` — roda ao iniciar o jogo, reenvia arquivos pendentes com pausa de 0.5s entre cada um
    - `RemoverArquivoFallback()` — limpa o arquivo local após reenvio bem-sucedido
- `LudusMonitor.cs` atualizado — `EndSession()` agora aciona `LudusExporter.Instance.Exportar()` diretamente

### Testado

- Fallback funcionando: sessão salva localmente em `persistentDataPath/ludus_offline/` quando backend está offline
- Arquivo nomeado corretamente com UUID da sessão
- Reenvio automático de pendentes ao iniciar o jogo

### Marco

- **SDK completo** — todos os 7 componentes implementados e testados

---

## [0.3.0] — 2026-04-14

### Adicionado

- `LudusClickable.cs` — componente de nomeação semântica de objetos
    - Campo `elementName` para identificar botões e objetos interativos nos dados coletados
    - Compatível com qualquer GameObject (UI ou mundo 2D)
- `LudusInputTracker.cs` — captura global de input
    - Detecção de cliques via mouse (WebGL/Editor) e toque via touch (Android)
    - Tratamento separado para cliques em UI (`EventSystem`) e objetos 2D do mundo (`Physics2D.Raycast`)
    - Busca `LudusClickable` no objeto clicado e em seus pais na hierarquia (`GetComponentInParent`)
    - Registro do caminho do mouse/dedo a cada `0.1s` para geração de heatmap
    - Só processa input quando há sessão ativa

### Testado

- Clique em botão UI identificado corretamente com nome semântico e posição

---

## [0.2.0] — 2026-04-13

### Adicionado

- `LudusGameEvents.cs` — API pública estática do SDK
    - `CategorySelected`, `PhaseStarted`, `DragAttempt`, `CorrectMatch`, `WrongMatch`, `PhaseCompleted`, `SessionEnded`
    - `ValidarMonitor()` — verificação interna antes de cada chamada
    - Payloads em JSON com `InvariantCulture` nos floats
    - Atualização automática de `totalCorrect` e `totalWrong` nas métricas

### Testado

- Todos os eventos disparando corretamente em sequência com payloads JSON válidos

---

## [0.1.0] — 2026-04-12

### Adicionado

- `LudusConfig.cs` — ScriptableObject de configuração do SDK
- `LudusSession.cs` — modelo de dados da sessão em memória
    - Classes: `LudusSession`, `LudusMetrics`, `LudusClickEvent`, `LudusPathPoint`, `LudusGameEvent`
    - UUID automático, timestamps ISO 8601, detecção de plataforma
- `LudusMonitor.cs` — Singleton DontDestroyOnLoad orquestrador do SDK
    - `StartSession`, `EndSession`, `RegistrarAcao`, `RegistrarEvento`, `RegistrarClique`, `RegistrarPontoPath`, `VerificarInatividade`
- Estrutura de pastas: `Assets/Scripts/LUDUS_SDK/` e `Assets/Resources/LUDUS_SDK/`

### Testado

- Configuração carregada via `Resources.Load`, sessão iniciada com UUID único

---

## Próximas versões planejadas

- `[0.5.0]` — Cena de identificação do jogador integrada ao Para Que Serve?
- `[0.6.0]` — Integração real dos eventos SDK nos scripts do Para Que Serve?
- `[1.0.0]` — SDK integrado, backend conectado e testado em ambiente real
