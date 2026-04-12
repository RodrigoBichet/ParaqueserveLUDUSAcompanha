using UnityEngine;
using UnityEngine.UI;

public class TelaFinalHigiene : MonoBehaviour
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
        int errorCountHigiene = GameManagerHigiene.instance.errorCountHigiene;

        if (errorCountHigiene == 0)
        {
            imagemAcerto.SetActive(true);
            imagemAcerto2.SetActive(true);
            imagemAcerto3.SetActive(true);
            imagemAcertoTitle.SetActive(true);
            GameManagerHigiene.resultadoAvaliacao = "Acerto";
        }
        else if (errorCountHigiene <= 4)
        {
            imagemQuase.SetActive(true);
            imagemQuase2.SetActive(true);
            imagemQuaseTitle.SetActive(true);
            GameManagerHigiene.resultadoAvaliacao = "Quase";

        }
        else
        {
            imagemErro.SetActive(true);
            imagemErroTitle.SetActive(true);
            GameManagerHigiene.resultadoAvaliacao = "Erro";
        }
    }

    public void ButtonReset()
    {
        GameManagerHigiene.instance.errorCountHigiene = 0; // Reseta a contagem de erros
    }
}
