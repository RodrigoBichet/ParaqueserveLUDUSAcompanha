using UnityEngine;
using UnityEngine.UI;

public class ExibirAvaliacaoDiversao : MonoBehaviour
{
    public GameObject imagemErro;
    public GameObject imagemQuase;
    public GameObject imagemAcerto;

    void Start()
    {
        if (GameManagerDiversao.resultadoAvaliacao == "Acerto")
        {
            imagemAcerto.SetActive(true);
        }
        else if (GameManagerDiversao.resultadoAvaliacao == "Quase")
        {
            imagemQuase.SetActive(true);
        }
        else if (GameManagerDiversao.resultadoAvaliacao == "Erro")
        {
            imagemErro.SetActive(true);
        }

    }
}
