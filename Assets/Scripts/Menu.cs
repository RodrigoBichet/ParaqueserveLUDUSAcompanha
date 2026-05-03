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

        // Ao selecionar uma categoria, reinicia a sessão e registra o evento
        string[] categorias = { "Fase01", "Fase02", "Fase03", "Fase04", "Fase05" };
        if (System.Array.Exists(categorias, c => c == cena))
        {
            string nomeAmigavel = NomeCategoria.ContainsKey(cena) ? NomeCategoria[cena] : cena;

            // ALTERADO — era CategorySelected(), agora usa NovaSessaoCategoria()
            // para garantir que uma nova sessão seja iniciada a cada categoria
            LudusSDK.LudusGameEvents.NovaSessaoCategoria(nomeAmigavel);
        }

        SceneManager.LoadScene(cena);
    }

    public void Quit()
    {
        Application.Quit();
    }


}