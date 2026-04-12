using UnityEngine;
using UnityEngine.UI;

public class ExibirAvaliacaoHigiene : MonoBehaviour
{
    public GameObject imagemErro;
    public GameObject imagemQuase;
    public GameObject imagemAcerto;

    void Start()
    {
        if (GameManagerHigiene.resultadoAvaliacao == "Acerto")
        {
            imagemAcerto.SetActive(true);
        }
        else if (GameManagerHigiene.resultadoAvaliacao == "Quase")
        {
            imagemQuase.SetActive(true);
        }
        else if (GameManagerHigiene.resultadoAvaliacao == "Erro")
        {
            imagemErro.SetActive(true);
        }

    }
}
