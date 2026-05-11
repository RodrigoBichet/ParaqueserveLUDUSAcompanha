using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using LudusSDK;


public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rt;
    private CanvasGroup grupo;
    private Vector2 posicaooriginal;
    private bool estaArrastando = false;
    private static bool outroItemArrastando = false;
    public static bool coloucerto;
    public Sprite[] imagens;
    public Image imageComponent;
    private AudioSource sound;
    private string nomeItem;

    private float ultimoRegistroArraste = 0f;
    private const float INTERVALO_REGISTRO_ARRASTE = 0.05f;


    [SerializeField] private Canvas canvas;
    public TMP_Text legendaText;
    public string[] listaLegendas;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        grupo = GetComponent<CanvasGroup>();
        posicaooriginal = rt.anchoredPosition;
        coloucerto = false;
        sound = GetComponent<AudioSource>();

        // Desativar a legenda no início
        legendaText.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!estaArrastando && !outroItemArrastando && !coloucerto)
        {
            // Limpar a legenda antes de atualizar
            legendaText.text = "";

            nomeItem = this.gameObject.name;
            estaArrastando = true;
            outroItemArrastando = true;
            grupo.alpha = 0.5f;
            grupo.blocksRaycasts = false;
            sound.Play();

            // Ativar e atualizar a legenda ao iniciar o arraste
            legendaText.gameObject.SetActive(true);
            AtualizarLegenda(nomeItem);
            RegistrarArraste(eventData, "start", true);

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (estaArrastando)
        {
            // Verificar se está arrastando antes de atualizar a posição
            rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
            Debug.Log("Dragou");
            RegistrarArraste(eventData, "move");

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        estaArrastando = false;
        outroItemArrastando = false;
        grupo.alpha = 1f;
        grupo.blocksRaycasts = true;

        if (coloucerto == false)
        {
            rt.anchoredPosition = posicaooriginal;
        }

        // Atualizar a legenda ao finalizar o arraste
        AtualizarLegenda(nomeItem);

        // Desativar a legenda após 1 segundo
        Invoke("DesativarLegenda", 1f);
        RegistrarArraste(eventData, "end", true);

    }

    private void DesativarLegenda()
    {
        legendaText.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Se necessário, adicione comportamento de clique aqui
    }

    private void AtualizarLegenda(string nomeItem)
    {
        int index = System.Array.IndexOf(imagens, imageComponent.sprite);
        if (index != -1 && index < listaLegendas.Length)
        {
            legendaText.text = listaLegendas[index];
        }
        else
        {
            Debug.LogError("Legenda não encontrada para o item: " + nomeItem);
        }
    }

    private void RegistrarArraste(PointerEventData eventData, string estado, bool forcar = false)
    {
        if (LudusMonitor.Instance == null) return;

        if (!forcar && Time.unscaledTime - ultimoRegistroArraste < INTERVALO_REGISTRO_ARRASTE)
        {
            return;
        }

        ultimoRegistroArraste = Time.unscaledTime;

        string item = ObterNomeItem();

        LudusMonitor.Instance.RegistrarPontoArraste(
            item,
            eventData.position.x,
            eventData.position.y,
            estado
        );
    }

    private string ObterNomeItem()
    {
        if (imageComponent != null && imageComponent.sprite != null)
        {
            return imageComponent.sprite.name;
        }

        return gameObject.name;
    }

}
