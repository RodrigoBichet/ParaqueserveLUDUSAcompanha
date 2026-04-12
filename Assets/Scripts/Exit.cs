using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // Método chamado quando o botão é clicado
    public void FecharJogo()
    {
        // Fecha o jogo
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Para o editor do Unity
#else
        Application.Quit(); // Para as versões compiladas (Windows, Mac, Linux)
#endif
    }
}
