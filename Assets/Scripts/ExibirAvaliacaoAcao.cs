using UnityEngine;
using UnityEngine.UI;

public class ExibirAvaliacaoAcao : MonoBehaviour
{
    public GameObject imagemErro;
    public GameObject imagemQuase;
    public GameObject imagemAcerto;

    void Start()
    {
        if (GameManagerAcao.resultadoAvaliacao == "Acerto")
        {
            imagemAcerto.SetActive(true);
        }
        else if (GameManagerAcao.resultadoAvaliacao == "Quase")
        {
            imagemQuase.SetActive(true);
        }
        else if (GameManagerAcao.resultadoAvaliacao == "Erro")
        {
            imagemErro.SetActive(true);
        }

    }
}
