
// Parte do LUDUS Monitor SDK — LUDUS Acompanha
// ScriptableObject de configuração do SDK.
// Crie um asset via: Assets → Create → LUDUS → Configuração SDK
// Cada jogo da plataforma deve ter seu próprio asset de configuração

using UnityEngine;

namespace LudusSDK
{
    [CreateAssetMenu(
        fileName = "LudusConfig",
        menuName = "LUDUS/Configuração SDK",
        order = 1
    )]
    public class LudusConfig : ScriptableObject
    {
        [Header("Identificação do Jogo")]

        [Tooltip("Identificador único do jogo. Ex: 'para-que-serve', 'historietas-divertidas'")]
        public string gameId = "para-que-serve";

        [Tooltip("Versão atual do jogo. Ex: '1.0.0'")]
        public string gameVersion = "1.0.0";


        [Header("Conexão com o Backend")]

        [Tooltip("URL base do servidor Node.js. Ex: 'http://localhost:3000'")]
        public string backendUrl = "http://localhost:3000";

        [Tooltip("Envia os dados automaticamente ao encerrar a sessão?")]
        public bool sendOnSessionEnd = true;


        [Header("Comportamento Offline")]

        [Tooltip("Se verdadeiro, salva os dados localmente quando não houver conexão.")]
        public bool enableLocalFallback = true;

        [Tooltip("Pasta onde os arquivos de fallback serão salvos (relativa ao persistentDataPath).")]
        public string fallbackFolderName = "ludus_offline";


        [Header("Inatividade")]

        [Tooltip("Tempo em segundos sem interação para disparar o evento de inatividade.")]
        public float inactivityThresholdSeconds = 10f;


        [Header("Desenvolvimento")]

        [Tooltip("Ativa logs detalhados no Console do Unity. Desative antes de publicar.")]
        public bool debugMode = true;
    }
}