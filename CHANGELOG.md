# Changelog — Para Que Serve? + LUDUS Monitor SDK

Todas as mudanças relevantes do projeto são registradas aqui.  
Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/).

---

## [1.0.0] — 2026-05-06 — Captura de imagens por fase para heatmap

### Adicionado

- `IdentificacaoController.cs` — leitura do campo `capturaSolicitada` recebido do backend junto com os alunos
- `LudusGameEvents.cs` — `DefinirJogador(playerId, capturaSolicitada)` repassa a solicitação ao SDK
- `LudusSession.cs` — lista `screenshots` e classe `LudusFaseScreenshot` no JSON da sessão
- `LudusMonitor.cs` — captura screenshot em base64 no início de cada fase quando solicitado pelo dashboard
- `LudusMonitor.cs` — atraso apenas na primeira fase da categoria para aguardar animação inicial antes do print
- `LudusMonitor.cs` — bloqueio temporário de interação durante a captura inicial
- `LudusInputTracker.cs` — pausa o rastreamento de cliques/caminho enquanto a captura inicial está bloqueando interação

### Comportamento

- A captura é sob demanda e vale para a próxima sessão enviada pelo jogo.
- O índice de screenshots reinicia a cada sessão/categoria, gerando fases `0`, `1`, `2` e `3` no JSON.

---

## [0.9.0] — 2026-05-04 — Troca de aluno entre sessões

### Adicionado

- `Menu.cs` — método `VoltarIdentificacao()` para troca de aluno sem fechar o jogo
    - Encerra sessão ativa (se houver) antes de sair, garantindo que nenhum dado seja perdido
    - Limpa PlayerPrefs do aluno anterior
    - Preserva configurações do dispositivo: volume (`Master`, `Music`) e tema (`theme`)
    - Reseta feedback estático em memória das 5 categorias (`resultadoAvaliacao`)
    - Carrega cena de Identificacao pronta para novo aluno
- Botão "Trocar Aluno" adicionado nas cenas Menu e SelectLevel apontando para `VoltarIdentificacao()`

### Corrigido

- `SoundControl.cs` — detecção de primeira execução corrigida
    - Substituído `GetFloat("Master") == 0f` por `HasKey("Master")` — evita confundir volume zerado com ausência de configuração
    - Volume real aplicado ao carregar valores salvos (`AudioListener.volume` e `AudioSource.volume`)

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

- `[1.1.0]` — Publicar backend em servidor real e atualizar `backendUrl`
- `[1.2.0]` — Jogo testado nas escolas parceiras com dados reais
