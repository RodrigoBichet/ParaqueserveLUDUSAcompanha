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
        // Usa HasKey para detectar primeira vez, em vez de comparar com 0
        if (!PlayerPrefs.HasKey("Master"))
        {
            volumeMaster = 0.5f;
            volumeMusic = 0.5f;

            sliderMaster.value = volumeMaster;
            sliderMusic.value = volumeMusic;
            PlayerPrefs.SetFloat("Master", volumeMaster);
            PlayerPrefs.SetFloat("Music", volumeMusic);
        }
        else
        {
            sliderMaster.value = PlayerPrefs.GetFloat("Master");
            sliderMusic.value = PlayerPrefs.GetFloat("Music");

            // Aplica o volume de fato, nao so visualmente
            AudioListener.volume = sliderMaster.value;

            GameObject[] musicas = GameObject.FindGameObjectsWithTag("musica");
            foreach (var m in musicas)
                m.GetComponent<AudioSource>().volume = sliderMusic.value;
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
