using UnityEngine;
using UnityEngine.UI;

public class TelaFinalAcao : MonoBehaviour
{
    public GameObject imagemErro;
    public GameObject imagemErroTitle;
    public GameObject imagemQuase;
    public GameObject imagemQuase2;
    public GameObject imagemQuaseTitle;
    public GameObject imagemAcerto;
    public GameObject imagemAcerto2;
    public GameObject imagemAcerto3;

    public GameObject imagemAcertoTitle;

    void Start()
    {
        int errorCountAcao = GameManagerAcao.instance.errorCountAcao;

        if (errorCountAcao == 0)
        {
            imagemAcerto.SetActive(true);
            imagemAcerto2.SetActive(true);
            imagemAcerto3.SetActive(true);
            imagemAcertoTitle.SetActive(true);
            GameManagerAcao.resultadoAvaliacao = "Acerto";
        }
        else if (errorCountAcao <= 4)
        {
            imagemQuase.SetActive(true);
            imagemQuase2.SetActive(true);
            imagemQuaseTitle.SetActive(true);
            GameManagerAcao.resultadoAvaliacao = "Quase";

        }
        else
        {
            imagemErro.SetActive(true);
            imagemErroTitle.SetActive(true);
            GameManagerAcao.resultadoAvaliacao = "Erro";
        }
    }

    public void ButtonReset()
    {
        GameManagerAcao.instance.errorCountAcao = 0; // Reseta a contagem de erros
    }
}
