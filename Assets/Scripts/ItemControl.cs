using System.Collections;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    void Start()
    {
        // Inicie a corrotina para gerenciar a exibição do item
        StartCoroutine(GerenciarExibicao());
    }

    IEnumerator GerenciarExibicao()
    {
        // Comece desativado
        gameObject.SetActive(false);

        // Aguarde 5 segundos antes de começar o ciclo
        yield return new WaitForSeconds(5f);

        while (true)
        {
            // Exibir o item
            gameObject.SetActive(true);
            Debug.Log("Exibindo o item");
            yield return new WaitForSeconds(5f); // Tempo de exibição (ajuste conforme necessário)

            // Ocultar o item
            gameObject.SetActive(false);
            Debug.Log("Ocultando o item");
            yield return new WaitForSeconds(5f); // Tempo de ocultação (ajuste conforme necessário)
        }
    }
}
