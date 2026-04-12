using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonNext : MonoBehaviour
{
    public Button meuBotao;

    void Start()
    {
        meuBotao.interactable = false; // Desativa o botão no início
    }

    void Update()
    {
        //Debug.Log("Entrou no update");
        //Debug.Log("DragDrop: " + DragDrop.coloucerto);
        //string currentScene = SceneManager.GetActiveScene().name;
        //Debug.Log("CURRENT CENA BUTTONNEXT" + currentScene);
        // Lógica para determinar quando o botão deve aparecer

        if (DragDrop.coloucerto == true)
        {
            //Debug.Log("Entrou no if");
            meuBotao.interactable = true; // Ativa o botão
            //PlayerPrefs.SetInt("faseAtual", SceneManager.GetActiveScene().buildIndex); 
            //salva o progresso do usuário e a cena em que parou
            if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("faseCompletada"))
            {
                PlayerPrefs.SetInt("faseCompletada", SceneManager.GetActiveScene().buildIndex);
                PlayerPrefs.Save();
            }
        }

        //!CASO DO GAME OVER
        // if (currentScene == "Gameover")
        // {
        //     Debug.Log("CURRENT DO IF LEVELMANAGER" + currentScene);
        //     SceneManager.LoadScene("Selectlevel");
        //     //!REDIRECIONAR PARA ALGUMA TELA APOS 10 SEGUNDOS
        //     //Invoke("ReturnSelectLevel", 10f);
        //     //!FUNÇÃO PARA REDIRECIONAR (ir fora de voids e outras funções)
        //     // public void ReturnSelectLevel()
        //     // {
        //     //     SceneManager.LoadScene("Selectlevel");
        //     // }
        // }
        else
        {
            //Debug.Log("DragDrop Entrou no else" + DragDrop.coloucerto);
        }
    }


}

