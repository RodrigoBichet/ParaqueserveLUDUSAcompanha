using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Image[] imageComponents; // Array de referências aos componentes de imagem
    public Sprite[] images; // Array para armazenar as imagens
    public AudioClip[] audioClips; // Array para armazenar os áudios correspondentes

    void Start()
    {
        List<Sprite> imagensDisponiveis = new List<Sprite>(images); // Cria uma lista das imagens disponíveis
        bool aguaEscolhida = false;

        for (int i = 0; i < imageComponents.Length; i++)
        {
            int indiceImagem = UnityEngine.Random.Range(0, imagensDisponiveis.Count);

            if (imagensDisponiveis[indiceImagem] == images[0])
            {
                imageComponents[i].sprite = images[0]; // Atribui "Agua"
                imageComponents[i].gameObject.tag = "agua"; // Adiciona a tag "agua"
                aguaEscolhida = true;
            }
            else
            {
                imageComponents[i].sprite = imagensDisponiveis[indiceImagem];
                imageComponents[i].gameObject.tag = "tagerrada"; // Adiciona a tag "tagerrada"
            }

            // Associa o áudio correto à imagem
            AudioSource audioSource = imageComponents[i].gameObject.GetComponent<AudioSource>();
            audioSource.clip = audioClips[Array.IndexOf(images, imagensDisponiveis[indiceImagem])];

            imagensDisponiveis.RemoveAt(indiceImagem); // Remove a imagem escolhida da lista de opções
        }

        // Se a água não foi escolhida ainda, substitui uma imagem aleatória pela "Agua"
        if (!aguaEscolhida)
        {
            int indiceAleatorio = UnityEngine.Random.Range(0, imageComponents.Length);
            imageComponents[indiceAleatorio].sprite = images[0];
            imageComponents[indiceAleatorio].gameObject.tag = "agua"; // Adiciona a tag "agua"

            // Associa o áudio correto à imagem
            AudioSource audioSource = imageComponents[indiceAleatorio].gameObject.GetComponent<AudioSource>();
            audioSource.clip = audioClips[0]; // Áudio correspondente à "Agua"
        }
    }
}
