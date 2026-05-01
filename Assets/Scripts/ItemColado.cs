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

        // Pega o nome do sprite do item arrastado (nome amigável)
        Image imgArrastada = eventData.pointerDrag.GetComponent<Image>();
        string itemArrastado = imgArrastada != null && imgArrastada.sprite != null
            ? imgArrastada.sprite.name
            : eventData.pointerDrag.gameObject.name;

        // Pega o nome amigável do item-alvo via NewBehaviourScript do Canvas pai
        NewBehaviourScript gerador = transform.root.GetComponentInChildren<NewBehaviourScript>();
        string itemAlvo = gerador != null ? gerador.ItemCorreto : gameObject.tag;

        bool acertou = eventData.pointerDrag.gameObject.tag.Equals(gameObject.tag);
        float tempoFase = Time.time - _tempoInicioFase;

        // Registra tentativa com nomes amigáveis
        LudusSDK.LudusGameEvents.DragAttempt(itemArrastado, itemAlvo, acertou);

        if (acertou)
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

            // Registra acerto com nome amigável do item arrastado
            LudusSDK.LudusGameEvents.CorrectMatch(itemArrastado, tempoFase);
        }
        else
        {
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

            // Registra erro com nomes amigáveis
            LudusSDK.LudusGameEvents.WrongMatch(itemArrastado, itemAlvo);
        }
    }
}