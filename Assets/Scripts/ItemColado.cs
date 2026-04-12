using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Adicionando para acessar componentes de UI

public class ItemColado : MonoBehaviour, IDropHandler
{
    public AudioSource soundCongratulation;
    public AudioSource soundWrong;
    public GameObject imagemAtivada;
    public GameObject imagemDesativada;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.gameObject.tag.Equals(gameObject.tag))
            {
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
            }
            else
            {
                // Verificar o nome da cena atual
                string currentSceneName = SceneManager.GetActiveScene().name;

                if (currentSceneName == "Fase01")
                {
                    GameManagerAcao.instance.errorCountAcao++;
                }
                else if (currentSceneName == "Fase02")
                {
                    GameManagerAlimento.instance.errorCountAlimentos++;
                }

                else if (currentSceneName == "Fase03")
                {
                    GameManagerCotidiano.instance.errorCountCotidiano++;
                }

                else if (currentSceneName == "Fase04")
                {
                    GameManagerDiversao.instance.errorCountDiversao++;
                }

                else if (currentSceneName == "Fase05")
                {
                    GameManagerHigiene.instance.errorCountHigiene++;
                }

                soundWrong.Play();
            }
        }
    }
}
