// =============================================================================
// LudusClickable.cs
// Parte do LUDUS Monitor SDK — LUDUS Acompanha (UFPel, 2026)
// Autor: Rodrigo Leitzke Bichet
// Orientador: Prof. Dr. Leomar Soares da Rosa Júnior
//
// Componente de nomeação semântica.
// Adicione este componente em qualquer botão ou objeto interativo do jogo
// que você queira identificar nos dados coletados.
//
// Exemplo de uso:
//   - Arraste o componente LudusClickable para o botão "Alimentos"
//   - Defina elementName como "btn_categoria_alimentos"
//   - O SDK vai registrar esse nome toda vez que o botão for clicado
// =============================================================================

using UnityEngine;

namespace LudusSDK
{
    public class LudusClickable : MonoBehaviour
    {
        [Tooltip("Nome semântico deste objeto para fins de monitoramento.\n" +
                "Use nomes descritivos. Exemplos:\n" +
                "'btn_categoria_alimentos'\n" +
                "'img_opcao_maca'\n" +
                "'btn_menu_jogar'")]
        public string elementName = "elemento_sem_nome";
    }
}