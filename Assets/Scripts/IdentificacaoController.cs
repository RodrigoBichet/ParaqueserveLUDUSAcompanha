// =============================================================================
// IdentificacaoController.cs
// Para Que Serve? — LUDUS Acompanha (UFPel, 2026)
// Autor: Rodrigo Leitzke Bichet
//
// Controla a cena de identificação do jogador.
// Pega o nome digitado, inicia a sessão no SDK e avança para o Menu.
// =============================================================================

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IdentificacaoController : MonoBehaviour
{
    [Header("Referências da UI")]
    [Tooltip("Campo de texto onde o professor digita o nome da criança")]
    public TMP_InputField campoNome;

    [Header("Configuração")]
    [Tooltip("Nome usado quando nenhum nome é digitado")]
    public string nomePadrao = "Jogador";

    // =========================================================================
    // BotaoJogar
    // Chamado pelo botão de play da cena de identificação
    // =========================================================================

    public void BotaoJogar()
    {
        // Pega o nome digitado, remove espaços extras
        string nomeDigitado = campoNome.text.Trim();

        // Se o campo estiver vazio, usa o nome padrão
        if (string.IsNullOrEmpty(nomeDigitado))
            nomeDigitado = nomePadrao;

        // Inicia a sessão no SDK com o nome do jogador
        if (LudusSDK.LudusMonitor.Instance != null)
        {
            LudusSDK.LudusMonitor.Instance.StartSession(nomeDigitado);
        }
        else
        {
            Debug.LogWarning("[LUDUS] IdentificacaoController: LudusMonitor não encontrado. " +
                            "Verifique se o GameObject do SDK está na cena.");
        }

        // Avança para a cena do Menu (índice 1 no Build Settings)
        SceneManager.LoadScene(1);
    }
}