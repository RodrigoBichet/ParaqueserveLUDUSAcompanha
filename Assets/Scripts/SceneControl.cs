using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    public List<GameObject> cenasAleatorias;
    public GameObject cenaFinal;

    private int cenasVisitadas = 0;

    void Start()
    {
        // Inicialize o jogo com todas as cenas desativadas
        DesativarTodasCenas();

        // Embaralhar as cenas aleatórias
        Shuffle(cenasAleatorias);

        // Ativar a primeira cena aleatória
        IrParaProximaCenaAleatoria();
    }

    public void IrParaProximaCenaAleatoria()
    {
        // Se todas as cenas aleatórias foram visitadas, vá para a cena final
        if (cenasVisitadas >= 4)
        {
            cenaFinal.SetActive(true);
            DesativarTodasCenas();
            return;
        }

        // Ativar a próxima cena aleatória ainda não visitada
        GameObject proximaCena = cenasAleatorias[cenasVisitadas];
        proximaCena.SetActive(true);

        // Desativar todas as cenas exceto a cena ativa
        DesativarTodasCenas(proximaCena);

        cenasVisitadas++;
    }

    // Função para desativar todas as cenas exceto a cena especificada
    void DesativarTodasCenas(GameObject cenaAtiva = null)
    {
        foreach (GameObject cena in cenasAleatorias)
        {
            if (cena != cenaAtiva)
                cena.SetActive(false);
        }
    }

    // Função para embaralhar uma lista
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
}
