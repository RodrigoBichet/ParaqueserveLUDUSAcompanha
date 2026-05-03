# Changelog — Para Que Serve? + LUDUS Monitor SDK

Todas as mudanças relevantes do projeto são registradas aqui.  
Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/).

---

## [0.8.0] — 2026-05-02 — Fix bug de sessão múltipla por categoria

### Corrigido

- `LudusMonitor.cs` — adiciona campo `_currentPlayerId` para persistir jogador entre sessões
- `LudusMonitor.cs` — adiciona método `DefinirJogador()` — registra jogador sem iniciar sessão, separando identificação de início de sessão
- `LudusMonitor.cs` — simplifica `ReiniciarSessao()` — remove `EndSession()` automático que gerava sessão vazia extra a cada nova categoria
- `LudusGameEvents.cs` — adiciona `DefinirJogador()` — API pública chamada pela tela de identificação
- `LudusGameEvents.cs` — adiciona `NovaSessaoCategoria()` — reinicia sessão e registra categoria em uma única chamada
- `Menu.cs` — substitui chamada de `CategorySelected()` por `NovaSessaoCategoria()` ao selecionar categoria
- `IdentificacaoController.cs` — substitui `StartSession()` por `DefinirJogador()`, sessão não é mais aberta na identificação

---

## [0.7.0] — 2026-04-20 — Identificação robusta + LudusClickable completo

### Atualizado

- `IdentificacaoController.cs` — melhorias de UX e robustez
    - Feedback visual de loading durante cada busca ao backend
    - Mensagem de erro clara quando backend está offline
    - Mensagem de aviso quando não há dados cadastrados
    - Botão retry para tentar reconectar sem reiniciar o jogo
    - Timeout de 10 segundos nas requisições HTTP

### Adicionado

- `LudusClickable` em todos os botões e elementos interativos
    - `btn_jogar_identificacao`, `btn_retry_identificacao`
    - `btn_menu_jogar`
    - `btn_categoria_acoes`, `btn_categoria_alimentos`, `btn_categoria_cotidiano`, `btn_categoria_diversao`, `btn_categoria_higiene`
    - `btn_avancar_canvas`, `btn_avancar`
    - `slider_volume`
- Botões de voltar removidos durante gameplay — garante sessões completas

### Testado

- Backend offline — mensagem de erro e retry funcionando
- Cliques registrando com nomes semânticos corretos

---

## [0.6.0] — 2026-04-20

### Atualizado

- `IdentificacaoController.cs` — seleção em cascata Escola → Turma → Aluno
- Dropdowns com `interactable` em vez de `SetActive`

---

## [0.5.0] — 2026-04-19

### Corrigido

- `Menu.cs` — `SessionEnded` ao retornar para SelectLevel
- Sessões com `durationMs` correto no MongoDB

---

## [0.4.0] — 2026-04-18

### Adicionado — Integração no Para Que Serve?

- `Menu.cs`, `SceneControl.cs`, `ItemColado.cs` com eventos SDK
- `IdentificacaoController.cs` versão inicial

---

## [0.3.0] — 2026-04-14

### Adicionado

- `LudusExporter.cs` — envio HTTP + fallback offline

---

## [0.2.0] — 2026-04-13

### Adicionado

- `LudusClickable.cs` e `LudusInputTracker.cs`

---

## [0.1.0] — 2026-04-12

### Adicionado — SDK inicial

- `LudusConfig.cs`, `LudusSession.cs`, `LudusMonitor.cs`, `LudusGameEvents.cs`

---

## Próximas versões planejadas

- `[0.9.0]` — Publicar backend em servidor real e atualizar `backendUrl`
- `[1.0.0]` — Jogo testado nas escolas parceiras com dados reais
