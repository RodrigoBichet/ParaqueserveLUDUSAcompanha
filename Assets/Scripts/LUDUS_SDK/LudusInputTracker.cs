// =============================================================================
// LudusInputTracker.cs
// Parte do LUDUS Monitor SDK — LUDUS Acompanha (UFPel, 2026)
// Autor: Rodrigo Leitzke Bichet
// Orientador: Prof. Dr. Leomar Soares da Rosa Júnior
//
// Captura global de input (mouse e touch).
// Este componente fica ativo durante toda a sessão e:
//   1. Detecta cliques e toques na tela
//   2. Identifica o objeto clicado via LudusClickable (se houver)
//   3. Registra a posição e o nome do elemento no LudusMonitor
//   4. Registra o caminho do mouse/dedo para o heatmap
//
// Adicione este componente no mesmo GameObject do LudusMonitor.
// =============================================================================

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace LudusSDK
{
    public class LudusInputTracker : MonoBehaviour
    {
        // -------------------------------------------------------------------------
        // Configuração interna
        // -------------------------------------------------------------------------

        // Intervalo em segundos entre cada registro de ponto do caminho (heatmap)
        // 0.1 = 10 pontos por segundo — equilibrio entre precisão e tamanho dos dados
        private const float INTERVALO_PATH = 0.1f;

        // Layermask para o Raycast 2D — detecta objetos na cena
        [Tooltip("Layers consideradas no raycast para detectar LudusClickable.\n" +
                "Por padrão detecta tudo. Ajuste se necessário.")]
        public LayerMask layerMask = Physics2D.AllLayers;

        // -------------------------------------------------------------------------
        // Controle interno
        // -------------------------------------------------------------------------

        private float _tempoUltimoPath = 0f;    // Controla o intervalo do path
        private Camera _camera;                  // Câmera principal da cena

        // =========================================================================
        // Unity — Start
        // =========================================================================

        private void Start()
        {
            // Busca a câmera principal — necessária para converter posição de tela
            _camera = Camera.main;

            if (_camera == null)
                Debug.LogWarning("[LUDUS] LudusInputTracker: Camera.main não encontrada. " +
                                "Certifique-se de que existe uma câmera com tag 'MainCamera' na cena.");
        }

        // =========================================================================
        // Unity — Update
        // Roda a cada frame — verifica cliques e registra caminho
        // =========================================================================

        private void Update()
        {
            // Só rastreia se houver sessão ativa
            if (LudusMonitor.Instance == null) return;
            if (LudusMonitor.Instance.CurrentSession == null) return;

            VerificarClique();
            RegistrarCaminho();
        }

        // =========================================================================
        // VerificarClique
        // Detecta clique (mouse) ou toque (touch) e registra no Monitor
        // =========================================================================

        private void VerificarClique()
        {
            Vector2 posicaoTela = Vector2.zero;
            bool clicou = false;

            // --- Mouse (WebGL e Editor) ---
            if (Input.GetMouseButtonDown(0))
            {
                posicaoTela = Input.mousePosition;
                clicou = true;
            }

            // --- Touch (Android) ---
            if (Input.touchCount > 0)
            {
                Touch toque = Input.GetTouch(0);
                if (toque.phase == TouchPhase.Began)
                {
                    posicaoTela = toque.position;
                    clicou = true;
                }
            }

            if (!clicou) return;

            // Verifica se o clique foi em um elemento da UI (EventSystem)
            // Se foi, tenta encontrar o LudusClickable via UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                TratarCliqueUI(posicaoTela);
            }
            else
            {
                // Clique fora da UI — faz raycast no mundo 2D
                TratarCliqueMundo(posicaoTela);
            }
        }

        // =========================================================================
        // TratarCliqueUI
        // Trata cliques em elementos de UI (Canvas, Buttons, Images da UI)
        // =========================================================================

        private void TratarCliqueUI(Vector2 posicaoTela)
        {
            // Monta os dados para o raycast de UI
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = posicaoTela
            };

            List<RaycastResult> resultados = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, resultados);

            // Percorre os resultados buscando um LudusClickable
            string nomeElemento = "ui_sem_nome";

            foreach (RaycastResult resultado in resultados)
            {
                LudusClickable clickable = resultado.gameObject.GetComponent<LudusClickable>();

                // Sobe na hierarquia caso o componente esteja no pai
                if (clickable == null)
                    clickable = resultado.gameObject.GetComponentInParent<LudusClickable>();

                if (clickable != null)
                {
                    nomeElemento = clickable.elementName;
                    break;
                }
            }

            LudusMonitor.Instance.RegistrarClique(nomeElemento, posicaoTela.x, posicaoTela.y);

            if (LudusMonitor.Instance.Config.debugMode)
                Debug.Log("[LUDUS] Clique UI em: " + nomeElemento +
                        " | Pos: (" + posicaoTela.x.ToString("F0") +
                        ", " + posicaoTela.y.ToString("F0") + ")");
        }

        // =========================================================================
        // TratarCliqueMundo
        // Trata cliques em objetos 2D do mundo (sprites, coliders)
        // =========================================================================

        private void TratarCliqueMundo(Vector2 posicaoTela)
        {
            string nomeElemento = "mundo_sem_nome";

            if (_camera != null)
            {
                // Converte posição de tela para posição no mundo
                Vector2 posicaoMundo = _camera.ScreenToWorldPoint(posicaoTela);

                // Raycast 2D na posição clicada
                RaycastHit2D hit = Physics2D.Raycast(posicaoMundo, Vector2.zero, 0f, layerMask);

                if (hit.collider != null)
                {
                    // Tenta encontrar LudusClickable no objeto ou no pai
                    LudusClickable clickable = hit.collider.GetComponent<LudusClickable>();

                    if (clickable == null)
                        clickable = hit.collider.GetComponentInParent<LudusClickable>();

                    if (clickable != null)
                        nomeElemento = clickable.elementName;
                }
            }

            LudusMonitor.Instance.RegistrarClique(nomeElemento, posicaoTela.x, posicaoTela.y);

            if (LudusMonitor.Instance.Config.debugMode)
                Debug.Log("[LUDUS] Clique Mundo em: " + nomeElemento +
                        " | Pos: (" + posicaoTela.x.ToString("F0") +
                        ", " + posicaoTela.y.ToString("F0") + ")");
        }

        // =========================================================================
        // RegistrarCaminho
        // Registra a posição do mouse/dedo a cada INTERVALO_PATH segundos
        // Usado para montar o heatmap no dashboard
        // =========================================================================

        private void RegistrarCaminho()
        {
            if (Time.time - _tempoUltimoPath < INTERVALO_PATH) return;

            _tempoUltimoPath = Time.time;

            Vector2 posicao = Vector2.zero;

            // Mouse (WebGL e Editor)
            if (Input.touchCount == 0)
            {
                posicao = Input.mousePosition;
            }
            // Touch (Android)
            else
            {
                posicao = Input.GetTouch(0).position;
            }

            LudusMonitor.Instance.RegistrarPontoPath(posicao.x, posicao.y);
        }
    }
}