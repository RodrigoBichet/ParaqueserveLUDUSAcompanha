using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("AlternarParticulas", 0f, 10f); // Chama a função a cada 7 segundos
    }

    void AlternarParticulas()
    {
        var emission = GetComponent<ParticleSystem>().emission;

        if (emission.enabled)
        {
            emission.enabled = false; // Desativa as partículas
        }
        else
        {
            emission.enabled = true; // Ativa as partículas
        }
    }


}
