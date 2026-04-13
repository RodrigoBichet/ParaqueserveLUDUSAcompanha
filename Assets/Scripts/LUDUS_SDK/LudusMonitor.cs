// =============================================================================
// LudusMonitor.cs
// Parte do LUDUS Monitor SDK — LUDUS Acompanha (UFPel, 2026)
// Autor: Rodrigo Leitzke Bichet
// Orientador: Prof. Dr. Leomar Soares da Rosa Júnior
//
// Singleton principal do SDK. Orquestra toda a coleta de dados.
// Deve existir apenas uma instância durante toda a execução do jogo.
// Adicione este componente a um GameObject vazio na primeira cena do jogo.
// =============================================================================

using System;
using UnityEngine;

namespace LudusSDK
{
    public class LudusMonitor : MonoBehaviour
    {
        // -------------------------------------------------------------------------
        // Singleton
        // -------------------------------------------------------------------------

        public static LudusMonitor Instance { get; private set; }

        // -------------------------------------------------------------------------
        // Referências
        // -------------------------------------------------------------------------

        // Configuração do SDK (carregada automaticamente da pasta Resources)
        private LudusConfig _config;

        // Sessão atual em memória
        private LudusSession _currentSession;

        // -------------------------------------------------------------------------
        // Controle interno de tempo
        // -------------------------------------------------------------------------

        private float _sessionStartTime;       // Time.time no momento do StartSession
        private float _lastActionTime;         // Time.time da última ação registrada
        private bool _inactivityDispatched;    // Evita disparar inatividade múltiplas vezes
        private bool _sessionActive;           // Sessão está em andamento?

        // -------------------------------------------------------------------------
        // Propriedade pública para leitura da sessão (somente leitura)
        // -------------------------------------------------------------------------

        public LudusSession CurrentSession => _currentSession;
        public LudusConfig Config => _config;

        // =========================================================================
        // Unity — Awake
        // Inicializa o Singleton e carrega a configuração
        // =========================================================================

        private void Awake()
        {
            // Garante que só existe uma instância do Monitor
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas

            CarregarConfig();
        }

        // =========================================================================
        // Unity — Update
        // Roda a cada frame — verifica inatividade
        // =========================================================================

        private void Update()
        {
            if (!_sessionActive) return;

            VerificarInatividade();
        }

        // =========================================================================
        // CarregarConfig
        // Lê o LudusConfig.asset da pasta Resources/LUDUS_SDK/
        // =========================================================================

        private void CarregarConfig()
        {
            _config = Resources.Load<LudusConfig>("LUDUS_SDK/LudusConfig");

            if (_config == null)
            {
                Debug.LogError("[LUDUS] LudusConfig.asset não encontrado em Resources/LUDUS_SDK/. " +
                               "Crie o asset via Assets → Create → LUDUS → Configuração SDK.");
                return;
            }

            if (_config.debugMode)
                Debug.Log("[LUDUS] Configuração carregada. Jogo: " + _config.gameId +
                          " v" + _config.gameVersion);
        }

        // =========================================================================
        // StartSession
        // Inicia uma nova sessão de jogo para um jogador
        // Chamado pela cena de identificação (ou temporariamente com "jogador_teste")
        // =========================================================================

        public void StartSession(string playerId)
        {
            if (_config == null)
            {
                Debug.LogError("[LUDUS] Impossível iniciar sessão: configuração não carregada.");
                return;
            }

            if (_sessionActive)
            {
                Debug.LogWarning("[LUDUS] Já existe uma sessão ativa. Encerre-a antes de iniciar outra.");
                return;
            }

            // Cria a sessão com os dados do config + playerId recebido
            _currentSession = new LudusSession(playerId, _config.gameId, _config.gameVersion);

            // Inicializa controles de tempo
            _sessionStartTime = Time.time;
            _lastActionTime = Time.time;
            _inactivityDispatched = false;
            _sessionActive = true;

            if (_config.debugMode)
                Debug.Log("[LUDUS] Sessão iniciada. Player: " + playerId +
                        " | ID: " + _currentSession.sessionId);
        }

        // =========================================================================
        // EndSession
        // Encerra a sessão, calcula duração e aciona o envio dos dados
        // =========================================================================

        public void EndSession()
        {
            if (!_sessionActive)
            {
                Debug.LogWarning("[LUDUS] Nenhuma sessão ativa para encerrar.");
                return;
            }

            // Preenche dados de encerramento
            _currentSession.endedAt = DateTime.UtcNow.ToString("o");
            _currentSession.durationMs = (long)((Time.time - _sessionStartTime) * 1000f);
            _sessionActive = false;

            if (_config.debugMode)
                Debug.Log("[LUDUS] Sessão encerrada. Duração: " +
                        _currentSession.durationMs + "ms");

            // Envia os dados se configurado para isso
            if (_config.sendOnSessionEnd)
            {
                if (LudusExporter.Instance != null)
                    LudusExporter.Instance.Exportar(_currentSession);
                else
                    Debug.LogWarning("[LUDUS] LudusExporter não encontrado. " +
                                "Adicione o componente no mesmo GameObject do LudusMonitor.");
            }
        }

        // =========================================================================
        // RegistrarAcao
        // Chamado sempre que a criança interage com o jogo.
        // Atualiza o tempo da última ação e reseta o controle de inatividade.
        // =========================================================================

        public void RegistrarAcao()
        {
            if (!_sessionActive) return;

            // Se é a primeira ação da sessão, registra o tempo até ela
            if (_currentSession.metrics.firstActionMs == -1)
            {
                _currentSession.metrics.firstActionMs =
                    (long)((Time.time - _sessionStartTime) * 1000f);
            }

            _lastActionTime = Time.time;
            _inactivityDispatched = false; // Reseta para detectar próxima inatividade

            _currentSession.metrics.totalClicks++;
        }

        // =========================================================================
        // RegistrarEvento
        // Adiciona um evento semântico à lista da sessão
        // Chamado pelos métodos públicos do LudusGameEvents
        // =========================================================================

        public void RegistrarEvento(string eventType, string payload = "")
        {
            if (!_sessionActive) return;

            long timestampMs = (long)((Time.time - _sessionStartTime) * 1000f);
            var gameEvent = new LudusGameEvent(eventType, timestampMs, payload);

            _currentSession.gameEvents.Add(gameEvent);

            if (_config.debugMode)
                Debug.Log("[LUDUS] Evento: " + eventType + " | " + payload);
        }

        // =========================================================================
        // RegistrarClique
        // Adiciona um clique/toque à lista de cliques da sessão
        // =========================================================================

        public void RegistrarClique(string element, float x, float y)
        {
            if (!_sessionActive) return;

            long timestampMs = (long)((Time.time - _sessionStartTime) * 1000f);
            var click = new LudusClickEvent(element, x, y, timestampMs);

            _currentSession.clicks.Add(click);
            RegistrarAcao(); // Toda vez que clica, registra como ação
        }

        // =========================================================================
        // RegistrarPontoPath
        // Adiciona um ponto ao caminho do mouse/dedo (para o heatmap)
        // =========================================================================

        public void RegistrarPontoPath(float x, float y)
        {
            if (!_sessionActive) return;

            long timestampMs = (long)((Time.time - _sessionStartTime) * 1000f);
            _currentSession.mousePath.Add(new LudusPathPoint(x, y, timestampMs));
        }

        // =========================================================================
        // VerificarInatividade
        // Roda todo frame — dispara evento se o threshold for atingido
        // =========================================================================

        private void VerificarInatividade()
        {
            if (_inactivityDispatched) return;

            float tempoSemAcao = Time.time - _lastActionTime;

            if (tempoSemAcao >= _config.inactivityThresholdSeconds)
            {
                _inactivityDispatched = true;

                // Atualiza métricas de inatividade
                _currentSession.metrics.inactivityCount++;
                _currentSession.metrics.totalInactivityMs += tempoSemAcao * 1000f;

                // Registra o evento semântico
                RegistrarEvento("InactivityDetected",
                    "{\"durationMs\":" + (long)(tempoSemAcao * 1000f) + "}");

                if (_config.debugMode)
                    Debug.LogWarning("[LUDUS] Inatividade detectada: " +
                                    tempoSemAcao.ToString("F1") + "s");
            }
        }

        // =========================================================================
        // GetTimestampAtual
        // Utilitário — retorna ms desde o início da sessão
        // =========================================================================

        public long GetTimestampAtual()
        {
            if (!_sessionActive) return 0;
            return (long)((Time.time - _sessionStartTime) * 1000f);
        }
    }
}