// =============================================================================
// LudusGameEvents.cs
// Parte do LUDUS Monitor SDK — LUDUS Acompanha (UFPel, 2026)
// Autor: Rodrigo Leitzke Bichet
// Orientador: Prof. Dr. Leomar Soares da Rosa Júnior
//
// API pública estática do SDK.
// Esta é a ÚNICA interface que os scripts do jogo devem usar para
// comunicar eventos ao SDK. O jogo não precisa conhecer o LudusMonitor
// diretamente — apenas chama os métodos desta classe.
//
// Exemplo de uso no Para Que Serve?:
//   LudusGameEvents.CorrectMatch("água", 3.5f);
//   LudusGameEvents.CategorySelected("Alimentos");
// =============================================================================

using UnityEngine;

namespace LudusSDK
{
    public static class LudusGameEvents
    {
        // =========================================================================
        // CategorySelected
        // Chamado quando a criança escolhe uma categoria na cena de seleção
        // Exemplo: LudusGameEvents.CategorySelected("Alimentos");
        // =========================================================================

        // =========================================================================
        // DefinirJogador
        // Registra o jogador atual SEM iniciar sessão.
        // Chamado pela tela de identificação. A sessão só começa quando
        // uma categoria é selecionada via NovaSessaoCategoria().
        // =========================================================================
        public static void DefinirJogador(string playerId)
        {
            if (LudusMonitor.Instance == null)
            {
                Debug.LogError("[LUDUS] LudusMonitor não encontrado.");
                return;
            }

            LudusMonitor.Instance.DefinirJogador(playerId);
        }

        public static void CategorySelected(string category)
        {
            if (!ValidarMonitor("CategorySelected")) return;

            string payload = "{" +
                "\"category\":\"" + category + "\"" +
            "}";

            LudusMonitor.Instance.RegistrarEvento("CategorySelected", payload);
        }

        // =========================================================================
        // PhaseStarted
        // Chamado quando uma nova fase começa — registra o item-alvo e as opções
        // Exemplo: LudusGameEvents.PhaseStarted("maçã", new string[]{"maçã","bola","carro","peixe"});
        // =========================================================================

        public static void PhaseStarted(string targetItem, string[] options)
        {
            if (!ValidarMonitor("PhaseStarted")) return;

            // Monta o array de opções em formato JSON manualmente
            string optionsJson = "[\"" + string.Join("\",\"", options) + "\"]";

            string payload = "{" +
                "\"targetItem\":\"" + targetItem + "\"," +
                "\"options\":" + optionsJson +
            "}";

            LudusMonitor.Instance.RegistrarEvento("PhaseStarted", payload);
        }

        // =========================================================================
        // DragAttempt
        // Chamado toda vez que a criança arrasta um item, acertando ou errando
        // Exemplo: LudusGameEvents.DragAttempt("bola", "maçã", false);
        // =========================================================================

        public static void DragAttempt(string draggedItem, string targetItem, bool correct)
        {
            if (!ValidarMonitor("DragAttempt")) return;

            // Registra como ação (atualiza métricas de interação)
            LudusMonitor.Instance.RegistrarAcao();

            string payload = "{" +
                "\"draggedItem\":\"" + draggedItem + "\"," +
                "\"targetItem\":\"" + targetItem + "\"," +
                "\"correct\":" + correct.ToString().ToLower() +
            "}";

            LudusMonitor.Instance.RegistrarEvento("DragAttempt", payload);
        }

        // =========================================================================
        // CorrectMatch
        // Chamado quando a criança acerta o pareamento
        // Exemplo: LudusGameEvents.CorrectMatch("maçã", 3.5f);
        // =========================================================================

        public static void CorrectMatch(string item, float timeSeconds)
        {
            if (!ValidarMonitor("CorrectMatch")) return;

            // Atualiza métrica de acertos
            LudusMonitor.Instance.CurrentSession.metrics.totalCorrect++;

            string payload = "{" +
                "\"item\":\"" + item + "\"," +
                "\"timeSeconds\":" + timeSeconds.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) +
            "}";

            LudusMonitor.Instance.RegistrarEvento("CorrectMatch", payload);
        }

        // =========================================================================
        // WrongMatch
        // Chamado quando a criança erra o pareamento
        // Exemplo: LudusGameEvents.WrongMatch("bola", "maçã");
        // =========================================================================

        public static void WrongMatch(string draggedItem, string expectedItem)
        {
            if (!ValidarMonitor("WrongMatch")) return;

            // Atualiza métrica de erros
            LudusMonitor.Instance.CurrentSession.metrics.totalWrong++;

            string payload = "{" +
                "\"draggedItem\":\"" + draggedItem + "\"," +
                "\"expectedItem\":\"" + expectedItem + "\"" +
            "}";

            LudusMonitor.Instance.RegistrarEvento("WrongMatch", payload);
        }

        // =========================================================================
        // PhaseCompleted
        // Chamado ao final de cada fase, com o resumo de desempenho
        // Exemplo: LudusGameEvents.PhaseCompleted(3, 1, 45.2f, 2);
        // =========================================================================

        public static void PhaseCompleted(int acertos, int erros, float timeSeconds, int stars)
        {
            if (!ValidarMonitor("PhaseCompleted")) return;

            string payload = "{" +
                "\"acertos\":" + acertos + "," +
                "\"erros\":" + erros + "," +
                "\"timeSeconds\":" + timeSeconds.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) + "," +
                "\"stars\":" + stars +
            "}";

            LudusMonitor.Instance.RegistrarEvento("PhaseCompleted", payload);
        }

        // =========================================================================
        // SessionEnded
        // Chamado quando o jogador encerra a sessão (sai do jogo ou volta ao menu)
        // Exemplo: LudusGameEvents.SessionEnded();
        // =========================================================================

        public static void SessionEnded()
        {
            if (!ValidarMonitor("SessionEnded")) return;

            LudusMonitor.Instance.RegistrarEvento("SessionEnded");
            LudusMonitor.Instance.EndSession();
        }

        // =========================================================================
        // ValidarMonitor
        // Utilitário interno — verifica se o LudusMonitor está ativo antes de
        // qualquer chamada. Evita erros caso o SDK não tenha sido inicializado.
        // =========================================================================

        private static bool ValidarMonitor(string nomeEvento)
        {
            if (LudusMonitor.Instance == null)
            {
                Debug.LogError("[LUDUS] LudusGameEvents." + nomeEvento +
                    " chamado mas LudusMonitor não está na cena. " +
                    "Adicione o componente LudusMonitor ao GameObject inicial.");
                return false;
            }

            if (LudusMonitor.Instance.CurrentSession == null)
            {
                Debug.LogWarning("[LUDUS] LudusGameEvents." + nomeEvento +
                    " chamado mas nenhuma sessão foi iniciada. " +
                    "Chame LudusMonitor.Instance.StartSession() primeiro.");
                return false;
            }

            return true;
        }

        // =========================================================================
        // NovaSessaoCategoria
        // Reinicia a sessão e registra a categoria selecionada.
        // Chamado pelo Menu ao entrar em uma nova categoria após a sessão anterior
        // já ter sido encerrada. Garante que cada categoria gere uma sessão própria.
        // =========================================================================

        public static void NovaSessaoCategoria(string categoria)
        {
            if (LudusMonitor.Instance == null)
            {
                Debug.LogError("[LUDUS] LudusMonitor não encontrado.");
                return;
            }

            LudusMonitor.Instance.ReiniciarSessao();
            CategorySelected(categoria); // Registra o evento normalmente
        }
    }

}