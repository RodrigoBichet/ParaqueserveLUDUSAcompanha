using UnityEngine;
using UnityEngine.UI;

public class ExibirAvaliacaoCotidiano : MonoBehaviour
{
    public GameObject imagemErro;
    public GameObject imagemQuase;
    public GameObject imagemAcerto;

    void Start()
    {
        if (GameManagerCotidiano.resultadoAvaliacao == "Acerto")
        {
            imagemAcerto.SetActive(true);
        }
        else if (GameManagerCotidiano.resultadoAvaliacao == "Quase")
        {
            imagemQuase.SetActive(true);
        }
        else if (GameManagerCotidiano.resultadoAvaliacao == "Erro")
        {
            imagemErro.SetActive(true);
        }

    }
}
