using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Image[] imageComponents;
    public Sprite[] images;
    public AudioClip[] audioClips;

    // Propriedades públicas para o SceneControl ler após a cena ser ativada
    public string ItemCorreto { get; private set; }
    public string[] Opcoes { get; private set; }

    void Start()
    {
        List<Sprite> imagensDisponiveis = new List<Sprite>(images);
        bool aguaEscolhida = false;

        for (int i = 0; i < imageComponents.Length; i++)
        {
            int indiceImagem = UnityEngine.Random.Range(0, imagensDisponiveis.Count);

            if (imagensDisponiveis[indiceImagem] == images[0])
            {
                imageComponents[i].sprite = images[0];
                imageComponents[i].gameObject.tag = "agua";
                aguaEscolhida = true;
            }
            else
            {
                imageComponents[i].sprite = imagensDisponiveis[indiceImagem];
                imageComponents[i].gameObject.tag = "tagerrada";
            }

            AudioSource audioSource = imageComponents[i].gameObject.GetComponent<AudioSource>();
            audioSource.clip = audioClips[Array.IndexOf(images, imagensDisponiveis[indiceImagem])];

            imagensDisponiveis.RemoveAt(indiceImagem);
        }

        if (!aguaEscolhida)
        {
            int indiceAleatorio = UnityEngine.Random.Range(0, imageComponents.Length);
            imageComponents[indiceAleatorio].sprite = images[0];
            imageComponents[indiceAleatorio].gameObject.tag = "agua";

            AudioSource audioSource = imageComponents[indiceAleatorio].gameObject.GetComponent<AudioSource>();
            audioSource.clip = audioClips[0];
        }

        // Salva o item correto (images[0] é sempre o correto)
        ItemCorreto = images[0].name;

        // Salva os nomes de todas as opções exibidas
        Opcoes = new string[imageComponents.Length];
        for (int i = 0; i < imageComponents.Length; i++)
            Opcoes[i] = imageComponents[i].sprite != null ? imageComponents[i].sprite.name : "";
    }
}