using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonNextMenu : MonoBehaviour
{
    public Button meuBotao;

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("faseCompletada"))
        {
            PlayerPrefs.SetInt("faseCompletada", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
        }
    }


}




