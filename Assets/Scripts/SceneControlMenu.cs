using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControlMenu : MonoBehaviour
{
    public GameObject scene1;
    public GameObject scene2;

    public GameObject scene3;

    void Start()
    {
        // Inicialize o jogo com a Cena 1 ativa e a Cena 2 desativada
        scene1.SetActive(true);
        scene2.SetActive(false);
        scene3.SetActive(false);
    }

    public void IrParaCena1()
    {
        AtivarCena(1);
    }

    public void IrParaCena2()
    {
        AtivarCena(2);
    }

    public void IrParaCena3()
    {
        AtivarCena(3);
    }

    void AtivarCena(int cenaAtiva)
    {
        // Crie um array para armazenar todas as cenas
        GameObject[] cenas = { scene1, scene2, scene3 };

        // Desativa todas as cenas
        foreach (var cena in cenas)
        {
            cena.SetActive(false);
        }

        // Ativa apenas a cena desejada
        cenas[cenaAtiva - 1].SetActive(true);
    }
}
