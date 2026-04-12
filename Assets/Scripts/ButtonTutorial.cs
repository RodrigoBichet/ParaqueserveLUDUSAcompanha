//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Video;
//using UnityEngine.SceneManagement; // Adicionando o namespace para gerenciamento de cena

//public class ButtonTutorial : MonoBehaviour
//{
//    public Button botao;
//    public VideoPlayer videoPlayer;
//    public GameObject screenObject;

//    private AudioManager audioManager; // Referência para o AudioManager na cena Menu

//    private bool videoPlaying = false; // Variável para controlar se o vídeo está tocando

//    void Start()
//    {
//        videoPlayer.loopPointReached += OnVideoEnd;
//        videoPlayer.gameObject.SetActive(false);
//        screenObject.SetActive(false);



//        botao.onClick.AddListener(AtivarVideoPlayer);

//        // Procura o AudioManager na cena Menu
//        audioManager = FindObjectOfType<AudioManager>();
//    }

//    void AtivarVideoPlayer()
//    {
//        // Pausa a música de fundo se o AudioManager for encontrado
//        if (audioManager != null)
//        {
//            audioManager.ToggleMute();
//        }

//        videoPlayer.gameObject.SetActive(true);
//        screenObject.SetActive(true);



//        videoPlayer.Play();

//        videoPlaying = true; // Marca que o vídeo está tocando
//    }

//    void OnVideoEnd(VideoPlayer vp)
//    {
//        // Desativa os elementos quando o vídeo termina
//        videoPlayer.gameObject.SetActive(false);
//        screenObject.SetActive(false);



//        // Retoma a música de fundo apenas se o vídeo estiver tocando e o AudioManager for encontrado
//        if (videoPlaying && audioManager != null)
//        {
//            audioManager.ToggleMute();
//        }

//        videoPlaying = false; // Marca que o vídeo não está mais tocando
//    }

//    void OnDestroy()
//    {
//        // Se o script for destruído (como durante a troca de cena), retoma a música de fundo apenas se o vídeo estiver tocando e o AudioManager for encontrado
//        if (videoPlaying && audioManager != null)
//        {
//            audioManager.ToggleMute();
//        }
//    }
//}


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ButtonTutorial : MonoBehaviour
{
    public Button botao;
    public VideoPlayer videoPlayer;
    public GameObject screenObject;
    public GameObject border;

    [Header("Videos")]
    public VideoClip tutorialNormal;
    public VideoClip tutorialAutista;

    private AudioManager audioManager;

    private bool videoPlaying = false;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.gameObject.SetActive(false);
        screenObject.SetActive(false);
        border.SetActive(false);

        botao.onClick.AddListener(AtivarVideoPlayer);

        audioManager = FindObjectOfType<AudioManager>();
    }

    void AtivarVideoPlayer()
    {
        // Escolhe o vídeo baseado no tema atual
        if (ThemeManager.Instance.currentThemeType == ThemeType.Normal)
        {
            videoPlayer.clip = tutorialNormal;
        }
        else
        {
            videoPlayer.clip = tutorialAutista;
        }

        // Pausa música
        if (audioManager != null)
        {
            audioManager.ToggleMute();
        }

        videoPlayer.gameObject.SetActive(true);
        screenObject.SetActive(true);
        border.SetActive(true);

        videoPlayer.Play();

        videoPlaying = true;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        screenObject.SetActive(false);

        if (videoPlaying && audioManager != null)
        {
            audioManager.ToggleMute();
        }

        videoPlaying = false;
    }

    void OnDestroy()
    {
        if (videoPlaying && audioManager != null)
        {
            audioManager.ToggleMute();
        }
    }
}