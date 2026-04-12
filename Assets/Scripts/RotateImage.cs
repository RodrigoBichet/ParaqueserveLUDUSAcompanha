using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    private Animator animator;
    private bool animacaoAtiva = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // Inicia o ciclo de ativação/desativação da animação
        InvokeRepeating("AlternarAnimacao", 0, 3); // 8 segundos + 5 segundos = 13 segundos
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AlternarAnimacao()
    {
        // Verifica se o objeto e o componente Animator estão ativos
        if (gameObject.activeInHierarchy && animator != null && animator.isActiveAndEnabled)
        {
            // Verifica se o item já foi colado corretamente antes de alternar a animação
            if (!DragDrop.coloucerto)
            {
                animacaoAtiva = !animacaoAtiva; // Inverte o estado da animação

                if (animacaoAtiva)
                {
                    // Reinicia a animação
                    animator.Play("AnimationRotate", 0, 0); // Substitua "AnimationRotate" pelo nome da sua animação
                }
                else
                {
                    // Interrompe a animação
                    animator.StopPlayback();
                }
            }
        }
    }
}
