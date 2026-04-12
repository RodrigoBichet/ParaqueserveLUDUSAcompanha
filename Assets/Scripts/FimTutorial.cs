using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button buttonStart;

    public GameObject title;


    private bool isPlaying;

    void Start()
    {
        videoPlayer.loopPointReached += OnLoopReached;

        // Garante que os elementos estejam ativados no início
        buttonStart.gameObject.SetActive(true);

        title.SetActive(true);

    }

    void Update()
    {
        // Verifica se o vídeo está tocando
        isPlaying = videoPlayer.isPlaying;

        // Desativa os botões e o título se o vídeo estiver tocando
        buttonStart.gameObject.SetActive(!isPlaying);

        title.SetActive(!isPlaying);

    }

    void OnLoopReached(VideoPlayer vp)
    {
        // Para a reprodução do vídeo quando o loop for alcançado
        videoPlayer.Stop();

        // Ativa os elementos quando o vídeo terminar
        buttonStart.gameObject.SetActive(true);

        title.SetActive(true);

    }
}
