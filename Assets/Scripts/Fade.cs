using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public Animator anim;

    // Método chamado quando o botão é clicado
    public void TrocarDeCena(string nomeDaCena)
    {
        StartCoroutine(CarregarCena(nomeDaCena));
    }

    private IEnumerator CarregarCena(string nomeDaCena)
    {
        yield return null;

        // Ativar a animação de fade
        anim.SetTrigger("fade");

        // Aguardar o fade completar
        yield return new WaitForSeconds(1.5f);

        // Mudar para a cena com o nome definido
        SceneManager.LoadScene(nomeDaCena);
    }
}
