# Changelog — LUDUS Monitor SDK

Todas as mudanças relevantes do projeto são registradas aqui.  
Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/).

---

## [0.5.0] — 2026-04-15 — Etapa 1.5 completa 🎉

### Adicionado

- `IdentificacaoController.cs` — controle da cena de identificação do jogador
    - Lê o nome digitado no `TMP_InputField` e chama `LudusMonitor.StartSession()`
    - Nome padrão `"Jogador"` quando campo vazio
    - Cena `Inicial` criada e adicionada como índice 0 no Build Settings
- `Menu.cs` atualizado — `CategorySelected` filtrado
    - Evento disparado apenas para cenas de categoria real (`Fase01` a `Fase05`)
    - Navegação entre menus não gera eventos desnecessários
- `SceneControl.cs` atualizado — controle de fases integrado ao SDK
    - `PhaseStarted` ao ativar cada Canvas aleatório com nome do Canvas como identificador
    - `PhaseCompleted` com tempo real da rodada, acertos, erros e estrelas calculados automaticamente
    - Helpers `ObterErrosDaCategoria()`, `ObterAcertosDaCategoria()` e `CalcularEstrelas()` reutilizando os GameManagers existentes
    - Cronômetro `_tempoInicioFase` reiniciado a cada nova fase
- `ItemColado.cs` atualizado — detecção de acerto/erro integrada ao SDK
    - `DragAttempt` registrado em toda tentativa de arraste
    - `CorrectMatch` com tempo real da fase via `_tempoInicioFase` no `OnEnable()`
    - `WrongMatch` com nome do item arrastado e item esperado

### Testado

- Fluxo completo em jogo real: identificação → menu → categoria → fases → acertos/erros
- `CategorySelected` disparando apenas para categorias reais
- `PhaseStarted` identificando Canvas aleatório corretamente
- `DragAttempt` + `CorrectMatch` com tempo real registrado
- `PhaseCompleted` com estrelas calculadas automaticamente

### Marco

- **Etapas 1 e 1.5 concluídas** — SDK completo e integrado ao Para Que Serve?

---

## [0.4.0] — 2026-04-14 — SDK Completo 🎉

### Adicionado

- `LudusExporter.cs` — serialização e envio de sessões ao backend
    - `Exportar(session)` — serializa em JSON via `JsonUtility.ToJson()` e envia via HTTP POST
    - `EnviarParaBackend()` — Coroutine com `UnityWebRequest` para envio assíncrono
    - `SalvarLocalmente()` — fallback offline em `persistentDataPath/ludus_offline/`
    - `TentarReenviarPendentes()` — reenvio automático ao iniciar o jogo
    - `RemoverArquivoFallback()` — limpeza após reenvio bem-sucedido
- `LudusMonitor.cs` atualizado — `EndSession()` conectado ao `LudusExporter`

### Testado

- Fallback funcionando: sessão salva localmente quando backend offline
- Reenvio automático de pendentes ao iniciar o jogo

---

## [0.3.0] — 2026-04-14

### Adicionado

- `LudusClickable.cs` — nomeação semântica de objetos interativos
- `LudusInputTracker.cs` — captura global de input
    - Mouse (WebGL/Editor) e touch (Android)
    - Raycast em UI e mundo 2D
    - Registro de caminho a cada `0.1s` para heatmap

### Testado

- Clique em botão UI identificado com nome semântico e posição

---

## [0.2.0] — 2026-04-13

### Adicionado

- `LudusGameEvents.cs` — API pública estática
    - `CategorySelected`, `PhaseStarted`, `DragAttempt`, `CorrectMatch`, `WrongMatch`, `PhaseCompleted`, `SessionEnded`
    - Validação automática do Monitor antes de cada chamada

### Testado

- Todos os eventos disparando com payloads JSON válidos

---

## [0.1.0] — 2026-04-12

### Adicionado

- `LudusConfig.cs` — ScriptableObject de configuração
- `LudusSession.cs` — modelo de dados com UUID, timestamps ISO 8601 e detecção de plataforma
- `LudusMonitor.cs` — Singleton DontDestroyOnLoad com detecção de inatividade

### Testado

- Configuração carregada via `Resources.Load`, sessão iniciada com UUID único

---

## Próximas versões planejadas

- `[1.0.0]` — Backend Node.js + Express + MongoDB conectado ao SDK
- `[1.1.0]` — Dashboard React com visualização dos dados coletados
- `[1.2.0]` — Análise ML com Python + scikit-learn
- `[2.0.0]` — Sistema completo testado nas escolas parceiras
