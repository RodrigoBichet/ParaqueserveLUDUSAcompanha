using UnityEngine;
using UnityEngine.UI;

public class ExibirAvaliacaoAlimento : MonoBehaviour
{
    public GameObject imagemErro;
    public GameObject imagemQuase;
    public GameObject imagemAcerto;

    void Start()
    {
        if (GameManagerAlimento.resultadoAvaliacao == "Acerto")
        {
            imagemAcerto.SetActive(true);
        }
        else if (GameManagerAlimento.resultadoAvaliacao == "Quase")
        {
            imagemQuase.SetActive(true);
        }
        else if (GameManagerAlimento.resultadoAvaliacao == "Erro")
        {
            imagemErro.SetActive(true);
        }

    }
}
