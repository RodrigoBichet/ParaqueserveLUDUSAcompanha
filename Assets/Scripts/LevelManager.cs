// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class LevelManager : MonoBehaviour
// {

//     // public void callLevels()
//     // {

//     //     //SceneManager.LoadScene(PlayerPrefs.GetInt("faseAtual") + 1);


//     //     //!CASO DO GAME OVER
//     //     // int nextScene = PlayerPrefs.GetInt("faseAtual") + 1;
//     //     // string currentScene = SceneManager.GetActiveScene().name;
//     //     // Debug.Log("CURRENT CENA LEVELMANAGER" + currentScene);

//     //     // if (currentScene == "Gameover")
//     //     // {
//     //     //     Debug.Log("CURRENT DO IF LEVELMANAGER" + currentScene);
//     //     //     SceneManager.LoadScene("Selectlevel");
//     //     // }

//     //     // else
//     //     // {
//     //     //     Debug.Log("CURRENT DO ELSE LEVELMANAGER" + currentScene);
//     //     //     SceneManager.LoadScene(nextScene);
//     //     // }
//     // }


//     public Button[] botões;

//     public void Update()
//     {
//         for (int i = 0; i < botões.Length; i++)
//         {
//             if (i + 2 > PlayerPrefs.GetInt("faseCompletada"))
//             {
//                 botões[i].interactable = false;
//             }

//         }
//     }

// }
