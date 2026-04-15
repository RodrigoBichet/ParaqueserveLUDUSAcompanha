// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI; // Adicionando para acessar componentes de UI

// public class ItemColado : MonoBehaviour, IDropHandler
// {
//     public AudioSource soundCongratulation;
//     public AudioSource soundWrong;
//     public GameObject imagemAtivada;
//     public GameObject imagemDesativada;

//     public void OnDrop(PointerEventData eventData)
//     {
//         if (eventData.pointerDrag != null)
//         {
//             if (eventData.pointerDrag.gameObject.tag.Equals(gameObject.tag))
//             {
//                 if (imagemAtivada != null)
//                 {
//                     gameObject.SetActive(false);
//                     imagemAtivada.SetActive(true);
//                     RectTransform rtAtivada = imagemAtivada.GetComponent<Image>().rectTransform;
//                     RectTransform rtItemArrastado = eventData.pointerDrag.GetComponent<Image>().rectTransform;
//                     rtItemArrastado.anchoredPosition = rtAtivada.anchoredPosition;
//                 }
//                 else
//                 {
//                     Debug.LogError("A referência à imagem ativada é nula!");
//                 }

//                 DragDrop.coloucerto = true;
//                 PlayerPrefs.SetInt("faseAtual", SceneManager.GetActiveScene().buildIndex);
//                 soundCongratulation.Play();
//             }
//             else
//             {
//                 // Verificar o nome da cena atual
//                 string currentSceneName = SceneManager.GetActiveScene().name;

//                 if (currentSceneName == "Fase01")
//                 {
//                     GameManagerAcao.instance.errorCountAcao++;
//                 }
//                 else if (currentSceneName == "Fase02")
//                 {
//                     GameManagerAlimento.instance.errorCountAlimentos++;
//                 }

//                 else if (currentSceneName == "Fase03")
//                 {
//                     GameManagerCotidiano.instance.errorCountCotidiano++;
//                 }

//                 else if (currentSceneName == "Fase04")
//                 {
//                     GameManagerDiversao.instance.errorCountDiversao++;
//                 }

//                 else if (currentSceneName == "Fase05")
//                 {
//                     GameManagerHigiene.instance.errorCountHigiene++;
//                 }

//                 soundWrong.Play();
//             }
//         }
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemColado : MonoBehaviour, IDropHandler
{
    public AudioSource soundCongratulation;
    public AudioSource soundWrong;
    public GameObject imagemAtivada;
    public GameObject imagemDesativada;

    // Guarda o tempo em que a fase começou para calcular duração
    private float _tempoInicioFase;

    private void OnEnable()
    {
        // Registra o tempo de início toda vez que este Canvas é ativado
        _tempoInicioFase = Time.time;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        // Nomes dos itens para o SDK
        string itemArrastado = eventData.pointerDrag.gameObject.name;
        string itemAlvo = gameObject.name;
        bool acertou = eventData.pointerDrag.gameObject.tag.Equals(gameObject.tag);
        float tempoFase = Time.time - _tempoInicioFase;

        // Registra a tentativa de arraste no SDK (sempre, acerto ou erro)
        LudusSDK.LudusGameEvents.DragAttempt(itemArrastado, itemAlvo, acertou);

        if (acertou)
        {
            // --- ACERTO ---
            if (imagemAtivada != null)
            {
                gameObject.SetActive(false);
                imagemAtivada.SetActive(true);
                RectTransform rtAtivada = imagemAtivada.GetComponent<Image>().rectTransform;
                RectTransform rtItemArrastado = eventData.pointerDrag.GetComponent<Image>().rectTransform;
                rtItemArrastado.anchoredPosition = rtAtivada.anchoredPosition;
            }
            else
            {
                Debug.LogError("A referência à imagem ativada é nula!");
            }

            DragDrop.coloucerto = true;
            PlayerPrefs.SetInt("faseAtual", SceneManager.GetActiveScene().buildIndex);
            soundCongratulation.Play();

            // Registra acerto no SDK com o tempo que levou
            LudusSDK.LudusGameEvents.CorrectMatch(itemArrastado, tempoFase);
        }
        else
        {
            // --- ERRO ---
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName == "Fase01")
                GameManagerAcao.instance.errorCountAcao++;
            else if (currentSceneName == "Fase02")
                GameManagerAlimento.instance.errorCountAlimentos++;
            else if (currentSceneName == "Fase03")
                GameManagerCotidiano.instance.errorCountCotidiano++;
            else if (currentSceneName == "Fase04")
                GameManagerDiversao.instance.errorCountDiversao++;
            else if (currentSceneName == "Fase05")
                GameManagerHigiene.instance.errorCountHigiene++;

            soundWrong.Play();

            // Registra erro no SDK
            LudusSDK.LudusGameEvents.WrongMatch(itemArrastado, itemAlvo);
        }
    }
}