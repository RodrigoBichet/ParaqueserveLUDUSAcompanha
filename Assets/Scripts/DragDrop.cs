using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (estaArrastando)
        {
            // Verificar se está arrastando antes de atualizar a posição
            rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
            Debug.Log("Dragou");
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
}
