// =============================================================================
// LudusExporter.cs
// Parte do LUDUS Monitor SDK — LUDUS Acompanha (UFPel, 2026)
// Autor: Rodrigo Leitzke Bichet
// Orientador: Prof. Dr. Leomar Soares da Rosa Júnior
//
// Responsável por serializar a sessão em JSON e enviá-la ao backend.
// Em caso de falha na conexão, salva localmente e tenta reenviar
// na próxima vez que o jogo for aberto.
//
// Adicione este componente no mesmo GameObject do LudusMonitor.
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace LudusSDK
{
    public class LudusExporter : MonoBehaviour
    {
        // -------------------------------------------------------------------------
        // Singleton
        // -------------------------------------------------------------------------

        public static LudusExporter Instance { get; private set; }

        // =========================================================================
        // Unity — Awake
        // =========================================================================

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // =========================================================================
        // Unity — Start
        // Tenta reenviar sessões salvas localmente ao iniciar o jogo
        // =========================================================================

        private void Start()
        {
            if (LudusMonitor.Instance != null &&
                LudusMonitor.Instance.Config != null &&
                LudusMonitor.Instance.Config.enableLocalFallback)
            {
                StartCoroutine(TentarReenviarPendentes());
            }
        }

        // =========================================================================
        // Exportar
        // Ponto de entrada principal — chamado pelo LudusMonitor ao encerrar sessão
        // =========================================================================

        public void Exportar(LudusSession sessao)
        {
            if (sessao == null)
            {
                Debug.LogError("[LUDUS] Exporter: sessão nula, nada a exportar.");
                return;
            }

            // Serializa a sessão para JSON usando o JsonUtility do Unity
            string json = JsonUtility.ToJson(sessao, prettyPrint: false);

            if (LudusMonitor.Instance.Config.debugMode)
                Debug.Log("[LUDUS] Exporter: JSON gerado (" + json.Length + " caracteres).");

            // Inicia o envio como Coroutine (necessário para chamadas HTTP no Unity)
            StartCoroutine(EnviarParaBackend(json, sessao.sessionId));
        }

        // =========================================================================
        // EnviarParaBackend
        // Coroutine que faz o HTTP POST para o backend Node.js
        // =========================================================================

        private IEnumerator EnviarParaBackend(string json, string sessionId)
        {
            LudusConfig config = LudusMonitor.Instance.Config;
            string url = config.backendUrl + "/api/sessions";

            // Converte o JSON para bytes
            byte[] corpo = System.Text.Encoding.UTF8.GetBytes(json);

            // Monta a requisição HTTP POST
            using (UnityWebRequest requisicao = new UnityWebRequest(url, "POST"))
            {
                requisicao.uploadHandler = new UploadHandlerRaw(corpo);
                requisicao.downloadHandler = new DownloadHandlerBuffer();
                requisicao.SetRequestHeader("Content-Type", "application/json");

                if (config.debugMode)
                    Debug.Log("[LUDUS] Exporter: enviando para " + url + "...");

                // Aguarda a resposta
                yield return requisicao.SendWebRequest();

                // --- Sucesso ---
                if (requisicao.result == UnityWebRequest.Result.Success)
                {
                    if (config.debugMode)
                        Debug.Log("[LUDUS] Exporter: sessão enviada com sucesso! " +
                                  "Resposta: " + requisicao.downloadHandler.text);

                    // Se havia um arquivo de fallback desta sessão, remove
                    if (config.enableLocalFallback)
                        RemoverArquivoFallback(sessionId, config);
                }
                // --- Falha ---
                else
                {
                    Debug.LogWarning("[LUDUS] Exporter: falha ao enviar sessão. " +
                                     "Erro: " + requisicao.error);

                    // Salva localmente se o fallback estiver habilitado
                    if (config.enableLocalFallback)
                        SalvarLocalmente(json, sessionId, config);
                }
            }
        }

        // =========================================================================
        // SalvarLocalmente
        // Salva o JSON da sessão em um arquivo local quando offline
        // =========================================================================

        private void SalvarLocalmente(string json, string sessionId, LudusConfig config)
        {
            try
            {
                // Monta o caminho da pasta de fallback
                // persistentDataPath é a pasta de dados do app — funciona em WebGL e Android
                string pasta = Path.Combine(
                    Application.persistentDataPath,
                    config.fallbackFolderName
                );

                // Cria a pasta se não existir
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                // Nome do arquivo = ID da sessão
                string caminhoArquivo = Path.Combine(pasta, sessionId + ".json");

                File.WriteAllText(caminhoArquivo, json);

                if (config.debugMode)
                    Debug.Log("[LUDUS] Exporter: sessão salva localmente em: " + caminhoArquivo);
            }
            catch (Exception e)
            {
                Debug.LogError("[LUDUS] Exporter: erro ao salvar localmente. " + e.Message);
            }
        }

        // =========================================================================
        // TentarReenviarPendentes
        // Coroutine que roda ao iniciar o jogo e reenvia sessões salvas localmente
        // =========================================================================

        private IEnumerator TentarReenviarPendentes()
        {
            LudusConfig config = LudusMonitor.Instance.Config;

            string pasta = Path.Combine(
                Application.persistentDataPath,
                config.fallbackFolderName
            );

            // Se a pasta não existe, não há nada pendente
            if (!Directory.Exists(pasta)) yield break;

            string[] arquivos = Directory.GetFiles(pasta, "*.json");

            if (arquivos.Length == 0) yield break;

            if (config.debugMode)
                Debug.Log("[LUDUS] Exporter: encontrados " + arquivos.Length +
                          " arquivo(s) pendente(s). Tentando reenviar...");

            // Tenta reenviar cada arquivo
            foreach (string caminhoArquivo in arquivos)
            {
                string json = File.ReadAllText(caminhoArquivo);

                // Extrai o sessionId do nome do arquivo
                string sessionId = Path.GetFileNameWithoutExtension(caminhoArquivo);

                string url = config.backendUrl + "/api/sessions";
                byte[] corpo = System.Text.Encoding.UTF8.GetBytes(json);

                using (UnityWebRequest requisicao = new UnityWebRequest(url, "POST"))
                {
                    requisicao.uploadHandler = new UploadHandlerRaw(corpo);
                    requisicao.downloadHandler = new DownloadHandlerBuffer();
                    requisicao.SetRequestHeader("Content-Type", "application/json");

                    yield return requisicao.SendWebRequest();

                    if (requisicao.result == UnityWebRequest.Result.Success)
                    {
                        if (config.debugMode)
                            Debug.Log("[LUDUS] Exporter: sessão pendente reenviada: " + sessionId);

                        // Remove o arquivo local após reenvio bem-sucedido
                        File.Delete(caminhoArquivo);
                    }
                    else
                    {
                        if (config.debugMode)
                            Debug.LogWarning("[LUDUS] Exporter: falha ao reenviar " +
                                            sessionId + ". Ficará pendente.");
                    }
                }

                // Pequena pausa entre reenvios para não sobrecarregar o backend
                yield return new WaitForSeconds(0.5f);
            }
        }

        // =========================================================================
        // RemoverArquivoFallback
        // Remove o arquivo local de uma sessão após envio bem-sucedido
        // =========================================================================

        private void RemoverArquivoFallback(string sessionId, LudusConfig config)
        {
            string caminhoArquivo = Path.Combine(
                Application.persistentDataPath,
                config.fallbackFolderName,
                sessionId + ".json"
            );

            if (File.Exists(caminhoArquivo))
            {
                File.Delete(caminhoArquivo);

                if (config.debugMode)
                    Debug.Log("[LUDUS] Exporter: arquivo de fallback removido: " + sessionId);
            }
        }
    }
}