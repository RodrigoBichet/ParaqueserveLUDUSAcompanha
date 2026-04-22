# Changelog — Para Que Serve? + LUDUS Monitor SDK

Todas as mudanças relevantes do projeto são registradas aqui.  
Formato baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/).

---

## [0.6.0] — 2026-04-20 — Seleção de aluno em cascata 🎉

### Atualizado

- `IdentificacaoController.cs` — refatoração completa da tela de identificação
    - Substituição do `TMP_InputField` por 3 `TMP_Dropdown` em cascata
    - Busca escolas ao abrir a cena via `GET /api/unity/schools`
    - Ao selecionar escola busca turmas via `GET /api/unity/groups/:schoolId`
    - Ao selecionar turma busca alunos via `GET /api/unity/students/:groupId`
    - Dropdowns desabilitados (`interactable = false`) até seleção anterior
    - Botão Jogar só ativa quando aluno está selecionado
    - `StartSession` chamado com nome real do aluno cadastrado no banco

### Testado

- Cascata completa: Escola → Turma → Aluno funcionando em tempo real
- `Player: João Silva` aparecendo no log após seleção e confirmação

---

## [0.5.0] — 2026-04-19

### Corrigido

- `Menu.cs` — `SessionEnded` disparado ao retornar para SelectLevel
- Sessões reais chegando ao MongoDB com `durationMs` correto

---

## [0.4.0] — 2026-04-18

### Adicionado — Integração no Para Que Serve?

- `Menu.cs` — `CategorySelected` filtrado para categorias reais
- `SceneControl.cs` — `PhaseStarted` e `PhaseCompleted` com tempo real
- `ItemColado.cs` — `DragAttempt`, `CorrectMatch`, `WrongMatch`
- `IdentificacaoController.cs` — versão inicial com campo de texto

---

## [0.3.0] — 2026-04-14

### Adicionado — SDK completo

- `LudusExporter.cs` — serialização JSON e envio HTTP com fallback offline
- Conexão ao `LudusExporter` no `EndSession()`

---

## [0.2.0] — 2026-04-13

### Adicionado

- `LudusClickable.cs` — nomeação semântica de objetos
- `LudusInputTracker.cs` — captura global de input com raycast

---

## [0.1.0] — 2026-04-12

### Adicionado — SDK inicial

- `LudusConfig.cs` — ScriptableObject de configuração
- `LudusSession.cs` — modelo de dados com UUID e timestamps ISO 8601
- `LudusMonitor.cs` — Singleton DontDestroyOnLoad com detecção de inatividade
- `LudusGameEvents.cs` — API pública com todos os eventos semânticos

---

## Próximas versões planejadas

- `[0.7.0]` — Publicar backend em servidor real e atualizar `backendUrl`
- `[1.0.0]` — Jogo testado nas escolas parceiras com dados reais
