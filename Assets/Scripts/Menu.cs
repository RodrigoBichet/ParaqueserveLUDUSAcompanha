using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Mapeamento de nome interno → nome amigável da categoria
    private static readonly Dictionary<string, string> NomeCategoria = new Dictionary<string, string>
    {
        { "Fase01", "Ações" },
        { "Fase02", "Alimentos" },
        { "Fase03", "Cotidiano" },
        { "Fase04", "Diversão" },
        { "Fase05", "Higiene" }
    };

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
        {
            // Passa o nome amigável em vez do nome interno da cena
            string nomeAmigavel = NomeCategoria.ContainsKey(cena) ? NomeCategoria[cena] : cena;
            LudusSDK.LudusGameEvents.CategorySelected(nomeAmigavel);
        }

        SceneManager.LoadScene(cena);
    }

    public void Quit()
    {
        Application.Quit();
    }
}