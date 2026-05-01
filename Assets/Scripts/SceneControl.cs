using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    public List<GameObject> cenasAleatorias;
    public GameObject cenaFinal;

    private int cenasVisitadas = 0;

    private float _tempoInicioFase = 0f; // controla o tempo de cada fase

    // Guarda o nome da categoria atual para o SDK
    // Pegamos pelo nome do GameObject que contém este SceneControl
    [Tooltip("Nome da categoria desta fase. Ex: Alimentos, Ações, Cotidiano...")]
    public string nomeCategoria = "Categoria";

    void Start()
    {
        DesativarTodasCenas();
        Shuffle(cenasAleatorias);
        IrParaProximaCenaAleatoria();
    }

    public void IrParaProximaCenaAleatoria()
    {
        // Todas as fases visitadas — fase completa
        if (cenasVisitadas >= 4)
        {
            // Calcula tempo total da rodada
            float tempoTotal = Time.time - _tempoInicioFase;

            LudusSDK.LudusGameEvents.PhaseCompleted(
                acertos: ObterAcertosDaCategoria(),
                erros: ObterErrosDaCategoria(),
                timeSeconds: tempoTotal,
                stars: CalcularEstrelas(ObterErrosDaCategoria())
            );

            cenaFinal.SetActive(true);
            DesativarTodasCenas();
            return;
        }

        // Define a próxima cena aleatória
        GameObject proximaCena = cenasAleatorias[cenasVisitadas];
        proximaCena.SetActive(true);
        DesativarTodasCenas(proximaCena);

        // Reinicia o cronômetro a cada nova fase
        _tempoInicioFase = Time.time;

        // Aguarda um frame para o NewBehaviourScript.Start() executar antes de ler os dados
        StartCoroutine(RegistrarPhaseStarted(proximaCena));

        cenasVisitadas++;
    }

    // =========================================================================
    // Helpers — buscam os contadores de erro de cada GameManager
    // =========================================================================

    private int ObterErrosDaCategoria()
    {
        string cena = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (cena == "Fase01" && GameManagerAcao.instance != null)
            return GameManagerAcao.instance.errorCountAcao;
        if (cena == "Fase02" && GameManagerAlimento.instance != null)
            return GameManagerAlimento.instance.errorCountAlimentos;
        if (cena == "Fase03" && GameManagerCotidiano.instance != null)
            return GameManagerCotidiano.instance.errorCountCotidiano;
        if (cena == "Fase04" && GameManagerDiversao.instance != null)
            return GameManagerDiversao.instance.errorCountDiversao;
        if (cena == "Fase05" && GameManagerHigiene.instance != null)
            return GameManagerHigiene.instance.errorCountHigiene;

        return 0;
    }

    private int ObterAcertosDaCategoria()
    {
        // São sempre 4 fases por rodada, acertos = 4 - erros
        return 4 - ObterErrosDaCategoria();
    }

    private int CalcularEstrelas(int erros)
    {
        if (erros == 0) return 3;
        if (erros <= 2) return 2;
        return 1;
    }

    void DesativarTodasCenas(GameObject cenaAtiva = null)
    {
        foreach (GameObject cena in cenasAleatorias)
        {
            if (cena != cenaAtiva)
                cena.SetActive(false);
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[randomIndex];
            list[randomIndex] = list[i];
            list[i] = temp;
        }
    }

    // Aguarda um frame para garantir que o NewBehaviourScript já inicializou
    private System.Collections.IEnumerator RegistrarPhaseStarted(GameObject cena)
    {
        yield return null; // espera 1 frame

        NewBehaviourScript gerador = cena.GetComponentInChildren<NewBehaviourScript>();

        string itemAlvo = gerador != null ? gerador.ItemCorreto : cena.name;
        string[] opcoes = gerador != null ? gerador.Opcoes : new string[] { };

        LudusSDK.LudusGameEvents.PhaseStarted(targetItem: itemAlvo, options: opcoes);
    }
}