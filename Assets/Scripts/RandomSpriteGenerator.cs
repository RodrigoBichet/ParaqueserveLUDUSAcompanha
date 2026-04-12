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

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class NewBehaviourScript : MonoBehaviour
// {
//     public Image[] imageComponents; // Array de referências aos componentes de imagem
//     public Sprite[] images; // Array para armazenar as imagens
//     public AudioClip[] audioClips; // Array para armazenar os áudios correspondentes
//     public string[] legendas; // Array para armazenar as legendas correspondentes

//     void Start()
//     {
//         List<Sprite> imagensDisponiveis = new List<Sprite>(images);
//         List<AudioClip> audiosDisponiveis = new List<AudioClip>(audioClips);
//         bool aguaEscolhida = false;

//         for (int i = 0; i < imageComponents.Length; i++)
//         {
//             int indiceAleatorio = UnityEngine.Random.Range(0, imagensDisponiveis.Count);
//             Sprite imagemSelecionada = imagensDisponiveis[indiceAleatorio];
//             AudioClip audioSelecionado = audiosDisponiveis[indiceAleatorio];

//             // Atualiza a imagem correspondente ao item gerado aleatoriamente
//             imageComponents[i].sprite = imagemSelecionada;

//             // Verifica se a imagem selecionada é "Agua"
//             if (imagemSelecionada == images[0])
//             {
//                 imageComponents[i].gameObject.tag = "agua"; // Adiciona a tag "agua"
//                 aguaEscolhida = true;
//             }

//             // Define a legenda correspondente à imagem selecionada
//             int indiceLegenda = Array.IndexOf(images, imagemSelecionada);
//             if (indiceLegenda >= 0 && indiceLegenda < legendas.Length)
//             {
//                 imageComponents[i].GetComponent<DragDrop>().legendaText.text = legendas[indiceLegenda];
//             }
//             else
//             {
//                 Debug.LogError("Erro: Índice de legenda inválido para a imagem " + imagemSelecionada.name);
//             }

//             // Associa o áudio correto à imagem
//             AudioSource audioSource = imageComponents[i].gameObject.GetComponent<AudioSource>();
//             if (audioSource != null)
//             {
//                 audioSource.clip = audioSelecionado; // Associa o áudio correto à imagem
//             }
//             else
//             {
//                 Debug.LogError("Erro: AudioSource não encontrado no objeto " + imageComponents[i].gameObject.name);
//             }

//             imagensDisponiveis.RemoveAt(indiceAleatorio); // Remove a imagem escolhida da lista de opções
//             audiosDisponiveis.RemoveAt(indiceAleatorio); // Remove o áudio escolhido da lista de opções
//         }

//         // Se a água não foi escolhida ainda, substitui uma imagem aleatória pela "Agua"
//         if (!aguaEscolhida)
//         {
//             int indiceAleatorio = UnityEngine.Random.Range(0, imageComponents.Length);
//             imageComponents[indiceAleatorio].sprite = images[0];
//             imageComponents[indiceAleatorio].gameObject.tag = "agua"; // Adiciona a tag "agua"

//             // Define a legenda correspondente à "Agua"
//             imageComponents[indiceAleatorio].GetComponent<DragDrop>().legendaText.text = legendas[0];

//             // Associa o áudio correto à imagem
//             AudioSource audioSource = imageComponents[indiceAleatorio].gameObject.GetComponent<AudioSource>();
//             if (audioSource != null && audioClips.Length > 0)
//             {
//                 audioSource.clip = audioClips[0]; // Áudio correspondente à "Agua"
//             }
//             else
//             {
//                 Debug.LogError("Erro: Áudio correspondente não encontrado para a imagem 'Agua'");
//             }
//         }
//     }


// }
