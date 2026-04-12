using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public float volumeMaster, volumeMusic;

    public Slider sliderMaster, sliderMusic;

    void Start()
    {
        // Verifica se é a primeira vez que o jogo está sendo iniciado
        if (PlayerPrefs.GetFloat("Master") == 0f)
        {
            // Configura os valores padrão para o volume Master e Music
            volumeMaster = 0.5f;
            volumeMusic = 0.5f;

            // Atualiza os sliders e PlayerPrefs
            sliderMaster.value = volumeMaster;
            sliderMusic.value = volumeMusic;
            PlayerPrefs.SetFloat("Master", volumeMaster);
            PlayerPrefs.SetFloat("Music", volumeMusic);
        }
        else
        {
            // Se não for a primeira vez, carrega os valores salvos
            sliderMaster.value = PlayerPrefs.GetFloat("Master");
            sliderMusic.value = PlayerPrefs.GetFloat("Music");
        }
    }

    public void VolumeMaster(float volume)
    {
        volumeMaster = volume;
        AudioListener.volume = volumeMaster;

        PlayerPrefs.SetFloat("Master", volumeMaster);
    }

    public void VolumeMusica(float volume)
    {
        volumeMusic = volume;
        GameObject[] Musica = GameObject.FindGameObjectsWithTag("musica");
        for (int i = 0; i < Musica.Length; i++)
        {
            Musica[i].GetComponent<AudioSource>().volume = volumeMusic;
        }

        PlayerPrefs.SetFloat("Music", volumeMusic);
    }
}
