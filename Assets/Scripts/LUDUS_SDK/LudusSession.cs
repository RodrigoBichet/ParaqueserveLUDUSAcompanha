// =============================================================================
// LudusSession.cs
// Parte do LUDUS Monitor SDK — LUDUS Acompanha (UFPel, 2026)
// Autor: Rodrigo Leitzke Bichet
// Orientador: Prof. Dr. Leomar Soares da Rosa Júnior
//
// Modelo de dados da sessão em memória.
// Esta classe representa a "ficha" de uma sessão de jogo completa.
// Todos os eventos registrados durante o gameplay ficam aqui até
// serem serializados e enviados ao backend pelo LudusExporter.
// =============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LudusSDK
{
    // -------------------------------------------------------------------------
    // Classe principal da sessão
    // -------------------------------------------------------------------------

    [Serializable]
    public class LudusSession
    {
        // Identificação
        public string sessionId;        // UUID único gerado ao iniciar a sessão
        public string playerId;         // ID da criança (definido pelo professor)
        public string gameId;           // Ex: "para-que-serve"
        public string gameVersion;      // Ex: "1.0.0"
        public string platform;         // "WebGL" ou "Android"

        // Tempo
        public string startedAt;        // ISO 8601 — ex: "2026-04-12T14:00:00Z"
        public string endedAt;          // Preenchido ao encerrar a sessão
        public long durationMs;         // Duração total em milissegundos

        // Métricas agregadas (calculadas ao longo da sessão)
        public LudusMetrics metrics;

        // Listas de eventos detalhados
        public List<LudusClickEvent> clicks;
        public List<LudusPathPoint> mousePath;
        public List<LudusGameEvent> gameEvents;

        // -------------------------------------------------------------------------
        // Construtor — inicializa a sessão com os dados básicos
        // -------------------------------------------------------------------------
        public LudusSession(string playerId, string gameId, string gameVersion)
        {
            this.sessionId = Guid.NewGuid().ToString();   // ID único e automático
            this.playerId = playerId;
            this.gameId = gameId;
            this.gameVersion = gameVersion;
            this.platform = Application.platform == RuntimePlatform.Android
                                ? "Android" : "WebGL";
            this.startedAt = DateTime.UtcNow.ToString("o"); // Formato ISO 8601
            this.endedAt = "";
            this.durationMs = 0;

            // Inicializa métricas zeradas
            this.metrics = new LudusMetrics();

            // Inicializa as listas vazias
            this.clicks = new List<LudusClickEvent>();
            this.mousePath = new List<LudusPathPoint>();
            this.gameEvents = new List<LudusGameEvent>();
        }
    }

    // -------------------------------------------------------------------------
    // Métricas agregadas da sessão
    // Calculadas progressivamente durante o jogo
    // -------------------------------------------------------------------------

    [Serializable]
    public class LudusMetrics
    {
        public int totalClicks;                         // Total de cliques/toques
        public int totalCorrect;                        // Total de acertos
        public int totalWrong;                          // Total de erros
        public long firstActionMs;                      // Tempo até a primeira ação (ms)
        public float avgTimeBetweenActionsMs;           // Média de tempo entre ações (ms)
        public int inactivityCount;                     // Quantas vezes ficou inativo
        public float totalInactivityMs;                 // Tempo total inativo (ms)

        public LudusMetrics()
        {
            totalClicks = 0;
            totalCorrect = 0;
            totalWrong = 0;
            firstActionMs = -1;  // -1 = ainda não houve nenhuma ação
            avgTimeBetweenActionsMs = 0f;
            inactivityCount = 0;
            totalInactivityMs = 0f;
        }
    }

    // -------------------------------------------------------------------------
    // Evento de clique ou toque na tela
    // Registra onde e em quê a criança tocou
    // -------------------------------------------------------------------------

    [Serializable]
    public class LudusClickEvent
    {
        public string element;      // Nome semântico do objeto clicado (ex: "btn_alimento_maca")
        public float x;             // Posição X na tela (pixels)
        public float y;             // Posição Y na tela (pixels)
        public long timestamp;      // Ms desde o início da sessão

        public LudusClickEvent(string element, float x, float y, long timestamp)
        {
            this.element = element;
            this.x = x;
            this.y = y;
            this.timestamp = timestamp;
        }
    }

    // -------------------------------------------------------------------------
    // Ponto do caminho do mouse/dedo
    // Usado para montar o heatmap no dashboard
    // -------------------------------------------------------------------------

    [Serializable]
    public class LudusPathPoint
    {
        public float x;     // Posição X
        public float y;     // Posição Y
        public long t;      // Timestamp em ms desde início da sessão

        public LudusPathPoint(float x, float y, long t)
        {
            this.x = x;
            this.y = y;
            this.t = t;
        }
    }

    // -------------------------------------------------------------------------
    // Evento semântico do jogo
    // Representa ações específicas do Para Que Serve? (e futuros jogos)
    // -------------------------------------------------------------------------

    [Serializable]
    public class LudusGameEvent
    {
        public string eventType;    // Ex: "CorrectMatch", "DragAttempt", "CategorySelected"
        public long timestamp;      // Ms desde o início da sessão
        public string payload;      // JSON com dados específicos do evento (flexível)

        public LudusGameEvent(string eventType, long timestamp, string payload = "")
        {
            this.eventType = eventType;
            this.timestamp = timestamp;
            this.payload = payload;
        }
    }
}