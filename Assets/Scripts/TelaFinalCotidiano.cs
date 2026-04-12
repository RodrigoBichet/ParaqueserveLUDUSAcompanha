using UnityEngine;
using UnityEngine.UI;

public class TelaFinalCotidiano : MonoBehaviour
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
        int errorCountCotidiano = GameManagerCotidiano.instance.errorCountCotidiano;

        if (errorCountCotidiano == 0)
        {
            imagemAcerto.SetActive(true);
            imagemAcerto2.SetActive(true);
            imagemAcerto3.SetActive(true);
            imagemAcertoTitle.SetActive(true);
            GameManagerCotidiano.resultadoAvaliacao = "Acerto";
        }
        else if (errorCountCotidiano <= 4)
        {
            imagemQuase.SetActive(true);
            imagemQuase2.SetActive(true);
            imagemQuaseTitle.SetActive(true);
            GameManagerCotidiano.resultadoAvaliacao = "Quase";

        }
        else
        {
            imagemErro.SetActive(true);
            imagemErroTitle.SetActive(true);
            GameManagerCotidiano.resultadoAvaliacao = "Erro";
        }
    }

    public void ButtonReset()
    {
        GameManagerCotidiano.instance.errorCountCotidiano = 0; // Reseta a contagem de erros
    }
}
