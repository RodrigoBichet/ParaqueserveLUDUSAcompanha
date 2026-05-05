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

    // =========================================================================
    // VoltarIdentificacao
    // Chamado pelo botão "Trocar Aluno" nas cenas Menu e SelectLevel.
    // Encerra a sessão ativa (se houver), preserva configurações do dispositivo
    // (volume e tema), limpa os dados do aluno anterior e volta à identificação.
    // =========================================================================

    public void VoltarIdentificacao()
    {
        // Encerra sessão ativa antes de sair, se houver
        if (LudusSDK.LudusMonitor.Instance != null &&
            LudusSDK.LudusMonitor.Instance.CurrentSession != null)
        {
            LudusSDK.LudusGameEvents.SessionEnded();
            Debug.Log("[LUDUS] Sessão encerrada por troca de aluno.");
        }

        // Preserva configurações do dispositivo antes de limpar
        float master = PlayerPrefs.GetFloat("Master", 0.5f);
        float music = PlayerPrefs.GetFloat("Music", 0.5f);
        int tema = PlayerPrefs.GetInt("theme", 0);

        // Limpa todos os dados salvos do aluno anterior
        PlayerPrefs.DeleteAll();

        // Restaura configurações do dispositivo
        PlayerPrefs.SetFloat("Master", master);
        PlayerPrefs.SetFloat("Music", music);
        PlayerPrefs.SetInt("theme", tema);
        PlayerPrefs.Save();

        // Reseta o feedback em memória de todas as categorias
        // As variáveis são estáticas e não são afetadas pelo PlayerPrefs.DeleteAll()
        GameManagerAcao.resultadoAvaliacao = "";
        GameManagerAlimento.resultadoAvaliacao = "";
        GameManagerCotidiano.resultadoAvaliacao = "";
        GameManagerDiversao.resultadoAvaliacao = "";
        GameManagerHigiene.resultadoAvaliacao = "";

        Debug.Log("[LUDUS] Dados do aluno anterior limpos. Pronto para novo aluno.");

        SceneManager.LoadScene(0); // Cena 0 = Identificacao
    }
}