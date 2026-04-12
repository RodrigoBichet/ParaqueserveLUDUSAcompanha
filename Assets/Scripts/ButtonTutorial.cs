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