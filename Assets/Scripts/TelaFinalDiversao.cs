using UnityEngine;
using UnityEngine.UI;

public class TelaFinalDiversao : MonoBehaviour
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
        int errorCountDiversao = GameManagerDiversao.instance.errorCountDiversao;

        if (errorCountDiversao == 0)
        {
            imagemAcerto.SetActive(true);
            imagemAcerto2.SetActive(true);
            imagemAcerto3.SetActive(true);
            imagemAcertoTitle.SetActive(true);
            GameManagerDiversao.resultadoAvaliacao = "Acerto";
        }
        else if (errorCountDiversao <= 4)
        {
            imagemQuase.SetActive(true);
            imagemQuase2.SetActive(true);
            imagemQuaseTitle.SetActive(true);
            GameManagerDiversao.resultadoAvaliacao = "Quase";

        }
        else
        {
            imagemErro.SetActive(true);
            imagemErroTitle.SetActive(true);
            GameManagerDiversao.resultadoAvaliacao = "Erro";
        }
    }

    public void ButtonReset()
    {
        GameManagerDiversao.instance.errorCountDiversao = 0; // Reseta a contagem de erros
    }
}
