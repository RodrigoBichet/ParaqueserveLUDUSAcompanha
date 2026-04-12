using UnityEngine;
using UnityEngine.UI;

public class TelaFinalAlimento : MonoBehaviour
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
        int errorCountAlimentos = GameManagerAlimento.instance.errorCountAlimentos;

        if (errorCountAlimentos == 0)
        {
            imagemAcerto.SetActive(true);
            imagemAcerto2.SetActive(true);
            imagemAcerto3.SetActive(true);
            imagemAcertoTitle.SetActive(true);
            GameManagerAlimento.resultadoAvaliacao = "Acerto";
        }
        else if (errorCountAlimentos <= 4)
        {
            imagemQuase.SetActive(true);
            imagemQuase2.SetActive(true);
            imagemQuaseTitle.SetActive(true);
            GameManagerAlimento.resultadoAvaliacao = "Quase";

        }
        else
        {
            imagemErro.SetActive(true);
            imagemErroTitle.SetActive(true);
            GameManagerAlimento.resultadoAvaliacao = "Erro";
        }
    }

    public void ButtonReset()
    {
        GameManagerAlimento.instance.errorCountAlimentos = 0; // Reseta a contagem de erros
    }
}
