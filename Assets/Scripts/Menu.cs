using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadScenes(string cena)
    {
        // Encerra a sessão ao voltar para o SelectLevel (tela de feedback)
        if (cena == "SelectLevel")
        {
            LudusSDK.LudusGameEvents.SessionEnded();
        }

        // Só registra CategorySelected para cenas de categoria real
        string[] categorias = { "Fase01", "Fase02", "Fase03", "Fase04", "Fase05" };
        if (System.Array.Exists(categorias, c => c == cena))
            LudusSDK.LudusGameEvents.CategorySelected(cena);

        SceneManager.LoadScene(cena);
    }

    public void Quit()
    {
        Application.Quit();
    }
}