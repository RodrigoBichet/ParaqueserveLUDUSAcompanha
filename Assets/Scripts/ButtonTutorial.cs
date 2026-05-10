using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class ButtonTutorial : MonoBehaviour
{
    public Button botao;
    public VideoPlayer videoPlayer;
    public GameObject screenObject;
    public GameObject border;

    [Header("Videos WebGL")]
    public string tutorialNormalArquivo = "LEGENDATUTORIALPARAQUESERVE.mp4";
    public string tutorialAutistaArquivo = "LEGENDATUTORIALPARAQUESERVEAUTISMO.mp4";

    [Header("Controles do video")]
    public GameObject controlesVideo;
    public Button botaoPauseContinuar;
    public Button botaoVoltar;
    public Button botaoAvancar;
    public float segundosPular = 5f;

    [Header("Linha do tempo")]
    public Slider sliderTempo;
    public Text textoTempoAtual;
    public Text textoTempoTotal;

    private AudioManager audioManager;
    private bool videoPlaying = false;
    private bool usuarioArrastandoSlider = false;
    private bool estavaTocandoAntesDoArraste = false;

    private double duracaoVideo = 0;
    private double tempoAtualExibido = 0;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.errorReceived += OnVideoError;

        videoPlayer.gameObject.SetActive(false);
        screenObject.SetActive(false);
        border.SetActive(false);

        if (controlesVideo != null)
        {
            controlesVideo.SetActive(false);
        }

        botao.onClick.AddListener(AtivarVideoPlayer);

        if (botaoPauseContinuar != null)
        {
            botaoPauseContinuar.onClick.AddListener(PausarOuContinuar);
        }

        if (botaoVoltar != null)
        {
            botaoVoltar.onClick.AddListener(VoltarVideo);
        }

        if (botaoAvancar != null)
        {
            botaoAvancar.onClick.AddListener(AvancarVideo);
        }

        ConfigurarSliderTempo();
        AtualizarInterfaceTempo(0, 0);

        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (!videoPlaying)
        {
            return;
        }

        if (duracaoVideo <= 0)
        {
            duracaoVideo = ObterDuracaoVideo();
        }

        if (!usuarioArrastandoSlider && videoPlayer.isPlaying)
        {
            tempoAtualExibido += Time.unscaledDeltaTime;
            tempoAtualExibido = LimitarTempo(tempoAtualExibido);
        }

        if (!usuarioArrastandoSlider)
        {
            AtualizarInterfaceTempo(tempoAtualExibido, duracaoVideo);
        }
    }

    void AtivarVideoPlayer()
    {
        string arquivoVideo = ThemeManager.Instance.currentThemeType == ThemeType.Normal
            ? tutorialNormalArquivo
            : tutorialAutistaArquivo;

        string videoUrl = $"{Application.streamingAssetsPath}/Videos/{arquivoVideo}";

        videoPlayer.Stop();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;

        duracaoVideo = 0;
        tempoAtualExibido = 0;
        usuarioArrastandoSlider = false;
        estavaTocandoAntesDoArraste = false;

        AtualizarInterfaceTempo(0, 0);

        if (audioManager != null && !videoPlaying)
        {
            audioManager.ToggleMute();
        }

        videoPlayer.gameObject.SetActive(true);
        screenObject.SetActive(true);
        border.SetActive(true);

        if (controlesVideo != null)
        {
            controlesVideo.SetActive(true);
        }

        videoPlaying = true;

        videoPlayer.Prepare();

        Debug.Log($"[Tutorial] Preparando video: {videoUrl}");
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        duracaoVideo = ObterDuracaoVideo();
        tempoAtualExibido = 0;

        AtualizarInterfaceTempo(tempoAtualExibido, duracaoVideo);

        vp.Play();

        Debug.Log($"[Tutorial] Video iniciado. Duracao: {duracaoVideo:0.00}s");
    }

    void PausarOuContinuar()
    {
        if (!videoPlaying)
        {
            return;
        }

        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            SincronizarPlayerComInterface();
            videoPlayer.Play();
        }
    }

    void VoltarVideo()
    {
        if (!videoPlaying)
        {
            return;
        }

        DefinirTempoVideo(tempoAtualExibido - segundosPular);
    }

    void AvancarVideo()
    {
        if (!videoPlaying)
        {
            return;
        }

        DefinirTempoVideo(tempoAtualExibido + segundosPular);
    }

    void ConfigurarSliderTempo()
    {
        if (sliderTempo == null)
        {
            return;
        }

        sliderTempo.minValue = 0;
        sliderTempo.maxValue = 1;
        sliderTempo.wholeNumbers = false;
        sliderTempo.SetValueWithoutNotify(0);
        sliderTempo.onValueChanged.AddListener(OnSliderTempoAlterado);

        EventTrigger trigger = sliderTempo.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = sliderTempo.gameObject.AddComponent<EventTrigger>();
        }

        AdicionarEventoSlider(trigger, EventTriggerType.PointerDown, IniciarArrasteSlider);
        AdicionarEventoSlider(trigger, EventTriggerType.PointerUp, FinalizarArrasteSlider);
    }

    void AdicionarEventoSlider(EventTrigger trigger, EventTriggerType tipo, UnityEngine.Events.UnityAction acao)
    {
        EventTrigger.Entry entrada = new EventTrigger.Entry();
        entrada.eventID = tipo;
        entrada.callback.AddListener((_) => acao.Invoke());
        trigger.triggers.Add(entrada);
    }

    void IniciarArrasteSlider()
    {
        if (!videoPlaying)
        {
            return;
        }

        usuarioArrastandoSlider = true;
        estavaTocandoAntesDoArraste = videoPlayer.isPlaying;

        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    void OnSliderTempoAlterado(float valor)
    {
        if (!videoPlaying || duracaoVideo <= 0)
        {
            return;
        }

        if (usuarioArrastandoSlider)
        {
            tempoAtualExibido = LimitarTempo(valor * duracaoVideo);
            AtualizarInterfaceTempo(tempoAtualExibido, duracaoVideo);
        }
    }

    void FinalizarArrasteSlider()
    {
        if (!videoPlaying || sliderTempo == null || duracaoVideo <= 0)
        {
            usuarioArrastandoSlider = false;
            estavaTocandoAntesDoArraste = false;
            return;
        }

        tempoAtualExibido = LimitarTempo(sliderTempo.value * duracaoVideo);
        SincronizarPlayerComInterface();
        AtualizarInterfaceTempo(tempoAtualExibido, duracaoVideo);

        usuarioArrastandoSlider = false;

        if (estavaTocandoAntesDoArraste)
        {
            videoPlayer.Play();
        }

        estavaTocandoAntesDoArraste = false;
    }

    void DefinirTempoVideo(double tempo)
    {
        tempoAtualExibido = LimitarTempo(tempo);
        SincronizarPlayerComInterface();
        AtualizarInterfaceTempo(tempoAtualExibido, duracaoVideo);
    }

    void SincronizarPlayerComInterface()
    {
        if (duracaoVideo <= 0)
        {
            duracaoVideo = ObterDuracaoVideo();
        }

        videoPlayer.time = tempoAtualExibido;
    }

    double ObterDuracaoVideo()
    {
        if (videoPlayer.length > 0)
        {
            return videoPlayer.length;
        }

        if (videoPlayer.frameRate > 0 && videoPlayer.frameCount > 0)
        {
            return videoPlayer.frameCount / videoPlayer.frameRate;
        }

        return 0;
    }

    double LimitarTempo(double tempo)
    {
        if (duracaoVideo <= 0)
        {
            return Mathf.Max(0f, (float)tempo);
        }

        return Mathf.Clamp((float)tempo, 0f, (float)duracaoVideo);
    }

    void AtualizarInterfaceTempo(double tempoAtual, double tempoTotal)
    {
        if (sliderTempo != null)
        {
            float progresso = tempoTotal > 0 ? (float)(tempoAtual / tempoTotal) : 0f;
            sliderTempo.SetValueWithoutNotify(progresso);
        }

        if (textoTempoAtual != null)
        {
            textoTempoAtual.text = FormatarTempo(tempoAtual);
        }

        if (textoTempoTotal != null)
        {
            textoTempoTotal.text = FormatarTempo(tempoTotal);
        }
    }

    string FormatarTempo(double segundos)
    {
        if (segundos < 0 || double.IsNaN(segundos) || double.IsInfinity(segundos))
        {
            segundos = 0;
        }

        int minutos = Mathf.FloorToInt((float)segundos / 60f);
        int segundosRestantes = Mathf.FloorToInt((float)segundos % 60f);

        return $"{minutos:00}:{segundosRestantes:00}";
    }

    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogWarning($"[Tutorial] Erro ao reproduzir video: {message}");
        EncerrarVideo();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        EncerrarVideo();
    }

    void EncerrarVideo()
    {
        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
        screenObject.SetActive(false);
        border.SetActive(false);

        tempoAtualExibido = 0;
        usuarioArrastandoSlider = false;
        estavaTocandoAntesDoArraste = false;

        if (controlesVideo != null)
        {
            controlesVideo.SetActive(false);
        }

        AtualizarInterfaceTempo(0, duracaoVideo);

        if (videoPlaying && audioManager != null)
        {
            audioManager.ToggleMute();
        }

        videoPlaying = false;
    }

    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        videoPlayer.errorReceived -= OnVideoError;

        if (sliderTempo != null)
        {
            sliderTempo.onValueChanged.RemoveListener(OnSliderTempoAlterado);
        }

        if (videoPlaying && audioManager != null)
        {
            audioManager.ToggleMute();
        }
    }
}
