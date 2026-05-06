using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;


public class LudusCapturaToggle : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("URL base do backend. Ex: http://localhost:3000")]
    public string backendUrl = "http://localhost:3000";

    [Header("UI")]
    public Button botaoToggle;
    public Image imagemToggle;
    public Sprite spriteLigado;
    public Sprite spriteDesligado;

    public TMP_Text textoFeedback;


    private bool _capturaAtiva;
    private string _origemCaptura;
    private bool _bloqueadoPorOutraOrigem;
    private bool _enviando;

    private void Start()
    {
        _capturaAtiva =
            LudusSDK.LudusMonitor.Instance != null &&
            LudusSDK.LudusMonitor.Instance.CapturaSolicitada;

        _origemCaptura = PlayerPrefs.GetString("LUDUSCapturaOrigem", "");

        _bloqueadoPorOutraOrigem =
            _capturaAtiva && _origemCaptura == "dashboard";

        if (botaoToggle != null)
        {
            botaoToggle.onClick.RemoveListener(AlternarCaptura);
            botaoToggle.onClick.AddListener(AlternarCaptura);
        }

        AtualizarVisual();
        if (_bloqueadoPorOutraOrigem)
        {
            MostrarFeedback("Imagem ativada pelo dashboard.");
        }
        else
        {
            MostrarFeedback("");
        }

    }

    private void AlternarCaptura()
    {
        if (_enviando)
        {
            return;
        }

        if (_bloqueadoPorOutraOrigem)
        {
            MostrarFeedback("Imagem ativada pelo dashboard.");
            return;
        }


        bool novoEstado = !_capturaAtiva;
        StartCoroutine(EnviarCaptura(novoEstado));
    }

    private IEnumerator EnviarCaptura(bool ativo)
    {
        string alunoId = PlayerPrefs.GetString("LUDUSAlunoId", "");

        if (string.IsNullOrEmpty(alunoId))
        {
            Debug.LogWarning("[LUDUS] Não foi possível alterar captura: aluno sem ID salvo.");
            yield break;
        }

        _enviando = true;
        AtualizarVisual();

        string url = backendUrl + "/api/unity/students/" + alunoId + "/solicitar-captura";
        string json = "{\"ativo\":" + (ativo ? "true" : "false") + "}";

        using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = 10;

            yield return req.SendWebRequest();

            _enviando = false;

            if (req.result != UnityWebRequest.Result.Success)
            {
                MostrarFeedback("Não foi possível alterar. Verifique o dashboard.");
                Debug.LogWarning("[LUDUS] Não foi possível alterar captura pelo jogo: " + req.downloadHandler.text);
                AtualizarVisual();
                yield break;
            }

            _capturaAtiva = ativo;
            _origemCaptura = ativo ? "unity" : "";

            PlayerPrefs.SetString("LUDUSCapturaOrigem", _origemCaptura);
            PlayerPrefs.Save();

            if (LudusSDK.LudusMonitor.Instance != null)
            {
                LudusSDK.LudusMonitor.Instance.DefinirCapturaSolicitada(_capturaAtiva);
            }

            MostrarFeedback(_capturaAtiva ? "Imagem no mapa ligada." : "Imagem no mapa desligada.");
            Debug.Log("[LUDUS] Imagem no mapa de calor: " + (_capturaAtiva ? "ligada" : "desligada"));

            AtualizarVisual();
        }
    }

    private void AtualizarVisual()
    {
        if (imagemToggle != null)
        {
            imagemToggle.sprite = _capturaAtiva ? spriteLigado : spriteDesligado;
        }

        if (botaoToggle != null)
        {
            botaoToggle.interactable = !_enviando && !_bloqueadoPorOutraOrigem;
        }
    }

    private void MostrarFeedback(string mensagem)
    {
        if (textoFeedback == null) return;
        textoFeedback.text = mensagem;
    }

}
