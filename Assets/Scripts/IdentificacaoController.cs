// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.SceneManagement;
// using TMPro;

// public class IdentificacaoController : MonoBehaviour
// {
//     // -------------------------------------------------------------------------
//     // Referências da UI
//     // -------------------------------------------------------------------------

//     [Header("Dropdowns")]
//     public TMP_Dropdown dropdownEscola;
//     public TMP_Dropdown dropdownTurma;
//     public TMP_Dropdown dropdownAluno;

//     [Header("Botão")]
//     public GameObject botaoJogar;

//     // -------------------------------------------------------------------------
//     // Configuração
//     // -------------------------------------------------------------------------

//     [Header("Configuração")]
//     [Tooltip("URL base do backend. Ex: http://localhost:3000")]
//     public string backendUrl = "http://localhost:3000";

//     // -------------------------------------------------------------------------
//     // Dados internos
//     // -------------------------------------------------------------------------

//     // Listas de IDs para mapear seleção do dropdown com o ID real
//     private List<string> _escolaIds = new List<string>();
//     private List<string> _turmaIds = new List<string>();
//     private List<string> _alunoIds = new List<string>();
//     private List<string> _alunoNomes = new List<string>();

//     // =========================================================================
//     // Unity — Start
//     // =========================================================================

//     private void Start()
//     {
//         // Desativa turma, aluno e botão até seleção em cascata
//         dropdownTurma.interactable = false;
//         dropdownAluno.interactable = false;
//         botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;

//         // Listeners dos dropdowns
//         dropdownEscola.onValueChanged.AddListener(OnEscolaSelected);
//         dropdownTurma.onValueChanged.AddListener(OnTurmaSelected);

//         // Busca escolas ao abrir a cena
//         StartCoroutine(BuscarEscolas());
//     }

//     // =========================================================================
//     // BuscarEscolas
//     // =========================================================================

//     private IEnumerator BuscarEscolas()
//     {
//         string url = backendUrl + "/api/unity/schools";

//         using (UnityWebRequest req = UnityWebRequest.Get(url))
//         {
//             yield return req.SendWebRequest();

//             if (req.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError("[LUDUS] Erro ao buscar escolas: " + req.error);
//                 yield break;
//             }

//             EscolasResponse resposta = JsonUtility.FromJson<EscolasResponse>(req.downloadHandler.text);

//             dropdownEscola.ClearOptions();
//             _escolaIds.Clear();

//             var opcoes = new List<string>();
//             opcoes.Add("Selecione a escola...");
//             _escolaIds.Add("");

//             foreach (var escola in resposta.escolas)
//             {
//                 opcoes.Add(escola.name);
//                 _escolaIds.Add(escola._id);
//             }

//             dropdownEscola.AddOptions(opcoes);
//             dropdownEscola.value = 0;
//         }
//     }

//     // =========================================================================
//     // OnEscolaSelected — chamado ao selecionar escola
//     // =========================================================================

//     private void OnEscolaSelected(int index)
//     {
//         // Reseta turma e aluno
//         dropdownTurma.ClearOptions();
//         dropdownAluno.ClearOptions();
//         dropdownTurma.interactable = false;
//         dropdownAluno.interactable = false;
//         botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;

//         if (index == 0) return; // "Selecione a escola..."

//         string escolaId = _escolaIds[index];
//         StartCoroutine(BuscarTurmas(escolaId));
//     }

//     // =========================================================================
//     // BuscarTurmas
//     // =========================================================================

//     private IEnumerator BuscarTurmas(string escolaId)
//     {
//         string url = backendUrl + "/api/unity/groups/" + escolaId;

//         using (UnityWebRequest req = UnityWebRequest.Get(url))
//         {
//             yield return req.SendWebRequest();

//             if (req.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError("[LUDUS] Erro ao buscar turmas: " + req.error);
//                 yield break;
//             }

//             TurmasResponse resposta = JsonUtility.FromJson<TurmasResponse>(req.downloadHandler.text);

//             dropdownTurma.ClearOptions();
//             _turmaIds.Clear();

//             var opcoes = new List<string>();
//             opcoes.Add("Selecione a turma...");
//             _turmaIds.Add("");

//             foreach (var turma in resposta.turmas)
//             {
//                 opcoes.Add(turma.name);
//                 _turmaIds.Add(turma._id);
//             }

//             dropdownTurma.AddOptions(opcoes);
//             dropdownTurma.value = 0;
//             dropdownTurma.interactable = true;
//         }
//     }

//     // =========================================================================
//     // OnTurmaSelected — chamado ao selecionar turma
//     // =========================================================================

//     private void OnTurmaSelected(int index)
//     {
//         dropdownAluno.ClearOptions();
//         dropdownAluno.interactable = false;
//         botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;

//         if (index == 0) return; // "Selecione a turma..."

//         string turmaId = _turmaIds[index];
//         StartCoroutine(BuscarAlunos(turmaId));
//     }

//     // =========================================================================
//     // BuscarAlunos
//     // =========================================================================

//     private IEnumerator BuscarAlunos(string turmaId)
//     {
//         string url = backendUrl + "/api/unity/students/" + turmaId;

//         using (UnityWebRequest req = UnityWebRequest.Get(url))
//         {
//             yield return req.SendWebRequest();

//             if (req.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.LogError("[LUDUS] Erro ao buscar alunos: " + req.error);
//                 yield break;
//             }

//             AlunosResponse resposta = JsonUtility.FromJson<AlunosResponse>(req.downloadHandler.text);

//             dropdownAluno.ClearOptions();
//             _alunoIds.Clear();
//             _alunoNomes.Clear();

//             var opcoes = new List<string>();
//             opcoes.Add("Selecione o aluno...");
//             _alunoIds.Add("");
//             _alunoNomes.Add("");

//             foreach (var aluno in resposta.alunos)
//             {
//                 opcoes.Add(aluno.name);
//                 _alunoIds.Add(aluno._id);
//                 _alunoNomes.Add(aluno.name);
//             }

//             dropdownAluno.AddOptions(opcoes);
//             dropdownAluno.value = 0;
//             dropdownAluno.interactable = true;

//             // Listener do aluno — ativa botão quando selecionado
//             dropdownAluno.onValueChanged.RemoveAllListeners();
//             dropdownAluno.onValueChanged.AddListener(OnAlunoSelected);
//         }
//     }

//     // =========================================================================
//     // OnAlunoSelected — ativa botão ao selecionar aluno
//     // =========================================================================

//     private void OnAlunoSelected(int index)
//     {
//         botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = index > 0;
//     }

//     // =========================================================================
//     // BotaoJogar — chamado pelo botão de play
//     // =========================================================================

//     public void BotaoJogar()
//     {
//         int indexAluno = dropdownAluno.value;

//         if (indexAluno == 0 || indexAluno >= _alunoNomes.Count)
//         {
//             Debug.LogWarning("[LUDUS] Nenhum aluno selecionado.");
//             return;
//         }

//         string nomeAluno = _alunoNomes[indexAluno];

//         if (LudusSDK.LudusMonitor.Instance != null)
//         {
//             LudusSDK.LudusMonitor.Instance.StartSession(nomeAluno);
//         }
//         else
//         {
//             Debug.LogWarning("[LUDUS] LudusMonitor não encontrado.");
//         }

//         SceneManager.LoadScene(1);
//     }

//     // =========================================================================
//     // Classes para deserialização do JSON
//     // =========================================================================

//     [Serializable] private class Escola { public string _id; public string name; }
//     [Serializable] private class Turma { public string _id; public string name; }
//     [Serializable] private class Aluno { public string _id; public string name; }

//     [Serializable] private class EscolasResponse { public List<Escola> escolas; }
//     [Serializable] private class TurmasResponse { public List<Turma> turmas; }
//     [Serializable] private class AlunosResponse { public List<Aluno> alunos; }
// }

// =============================================================================
// IdentificacaoController.cs
// Para Que Serve? — LUDUS Acompanha (UFPel, 2026)
// Autor: Rodrigo Leitzke Bichet
//
// Controla a cena de identificação do jogador.
// Busca escolas, turmas e alunos do backend via HTTP
// com feedback de loading, erro e botão de retry.
// =============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class IdentificacaoController : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Referências da UI
    // -------------------------------------------------------------------------

    [Header("Dropdowns")]
    public TMP_Dropdown dropdownEscola;
    public TMP_Dropdown dropdownTurma;
    public TMP_Dropdown dropdownAluno;

    [Header("Botões")]
    public GameObject botaoJogar;
    public GameObject botaoRetry;

    [Header("Feedback")]
    public TMP_Text textoStatus;

    // -------------------------------------------------------------------------
    // Configuração
    // -------------------------------------------------------------------------

    [Header("Configuração")]
    [Tooltip("URL base do backend. Ex: http://localhost:3000")]
    public string backendUrl = "http://localhost:3000";

    // -------------------------------------------------------------------------
    // Dados internos
    // -------------------------------------------------------------------------

    private List<string> _escolaIds = new List<string>();
    private List<string> _turmaIds = new List<string>();
    private List<string> _alunoIds = new List<string>();
    private List<string> _alunoNomes = new List<string>();

    // =========================================================================
    // Unity — Start
    // =========================================================================

    private void Start()
    {
        // Estado inicial
        dropdownEscola.interactable = false;
        dropdownTurma.interactable = false;
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;
        botaoRetry.SetActive(false);

        // Listeners
        dropdownEscola.onValueChanged.AddListener(OnEscolaSelected);
        dropdownTurma.onValueChanged.AddListener(OnTurmaSelected);

        // Busca escolas
        StartCoroutine(BuscarEscolas());
    }

    // =========================================================================
    // Helpers de feedback
    // =========================================================================

    private void MostrarStatus(string mensagem, bool erro = false)
    {
        if (textoStatus == null) return;
        textoStatus.text = mensagem;
        textoStatus.color = erro
            ? new Color(0.9f, 0.3f, 0.3f)   // vermelho
            : new Color(0.4f, 0.4f, 0.4f);  // cinza
    }

    private void LimparStatus()
    {
        if (textoStatus != null) textoStatus.text = "";
    }

    // =========================================================================
    // BuscarEscolas
    // =========================================================================

    private IEnumerator BuscarEscolas()
    {
        MostrarStatus("Carregando escolas...");
        botaoRetry.SetActive(false);
        dropdownEscola.interactable = false;

        string url = backendUrl + "/api/unity/schools";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.timeout = 10; // timeout de 10 segundos

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                MostrarStatus("❌ Servidor indisponível. Verifique a conexão.", erro: true);
                botaoRetry.SetActive(true);
                Debug.LogError("[LUDUS] Erro ao buscar escolas: " + req.error);
                yield break;
            }

            EscolasResponse resposta = JsonUtility.FromJson<EscolasResponse>(req.downloadHandler.text);

            if (resposta.escolas == null || resposta.escolas.Count == 0)
            {
                MostrarStatus("⚠️ Nenhuma escola cadastrada no sistema.", erro: true);
                botaoRetry.SetActive(true);
                yield break;
            }

            dropdownEscola.ClearOptions();
            _escolaIds.Clear();

            var opcoes = new List<string>();
            opcoes.Add("Selecione a escola...");
            _escolaIds.Add("");

            foreach (var escola in resposta.escolas)
            {
                opcoes.Add(escola.name);
                _escolaIds.Add(escola._id);
            }

            dropdownEscola.AddOptions(opcoes);
            dropdownEscola.value = 0;
            dropdownEscola.interactable = true;
            LimparStatus();
        }
    }

    // =========================================================================
    // OnEscolaSelected
    // =========================================================================

    private void OnEscolaSelected(int index)
    {
        dropdownTurma.ClearOptions();
        dropdownAluno.ClearOptions();
        dropdownTurma.interactable = false;
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;
        LimparStatus();

        if (index == 0) return;

        string escolaId = _escolaIds[index];
        StartCoroutine(BuscarTurmas(escolaId));
    }

    // =========================================================================
    // BuscarTurmas
    // =========================================================================

    private IEnumerator BuscarTurmas(string escolaId)
    {
        MostrarStatus("Carregando turmas...");

        string url = backendUrl + "/api/unity/groups/" + escolaId;

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.timeout = 10;

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                MostrarStatus("❌ Erro ao carregar turmas. Tente novamente.", erro: true);
                botaoRetry.SetActive(true);
                yield break;
            }

            TurmasResponse resposta = JsonUtility.FromJson<TurmasResponse>(req.downloadHandler.text);

            if (resposta.turmas == null || resposta.turmas.Count == 0)
            {
                MostrarStatus("⚠️ Nenhuma turma cadastrada nesta escola.", erro: true);
                yield break;
            }

            dropdownTurma.ClearOptions();
            _turmaIds.Clear();

            var opcoes = new List<string>();
            opcoes.Add("Selecione a turma...");
            _turmaIds.Add("");

            foreach (var turma in resposta.turmas)
            {
                opcoes.Add(turma.name);
                _turmaIds.Add(turma._id);
            }

            dropdownTurma.AddOptions(opcoes);
            dropdownTurma.value = 0;
            dropdownTurma.interactable = true;
            LimparStatus();
        }
    }

    // =========================================================================
    // OnTurmaSelected
    // =========================================================================

    private void OnTurmaSelected(int index)
    {
        dropdownAluno.ClearOptions();
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;
        LimparStatus();

        if (index == 0) return;

        string turmaId = _turmaIds[index];
        StartCoroutine(BuscarAlunos(turmaId));
    }

    // =========================================================================
    // BuscarAlunos
    // =========================================================================

    private IEnumerator BuscarAlunos(string turmaId)
    {
        MostrarStatus("Carregando alunos...");

        string url = backendUrl + "/api/unity/students/" + turmaId;

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.timeout = 10;

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                MostrarStatus("❌ Erro ao carregar alunos. Tente novamente.", erro: true);
                botaoRetry.SetActive(true);
                yield break;
            }

            AlunosResponse resposta = JsonUtility.FromJson<AlunosResponse>(req.downloadHandler.text);

            if (resposta.alunos == null || resposta.alunos.Count == 0)
            {
                MostrarStatus("⚠️ Nenhum aluno cadastrado nesta turma.", erro: true);
                yield break;
            }

            dropdownAluno.ClearOptions();
            _alunoIds.Clear();
            _alunoNomes.Clear();

            var opcoes = new List<string>();
            opcoes.Add("Selecione o aluno...");
            _alunoIds.Add("");
            _alunoNomes.Add("");

            foreach (var aluno in resposta.alunos)
            {
                opcoes.Add(aluno.name);
                _alunoIds.Add(aluno._id);
                _alunoNomes.Add(aluno.name);
            }

            dropdownAluno.AddOptions(opcoes);
            dropdownAluno.value = 0;
            dropdownAluno.interactable = true;

            dropdownAluno.onValueChanged.RemoveAllListeners();
            dropdownAluno.onValueChanged.AddListener(OnAlunoSelected);
            LimparStatus();
        }
    }

    // =========================================================================
    // OnAlunoSelected
    // =========================================================================

    private void OnAlunoSelected(int index)
    {
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = index > 0;
    }

    // =========================================================================
    // BotaoRetry — chamado pelo botão de retry
    // =========================================================================

    public void BotaoRetry()
    {
        botaoRetry.SetActive(false);
        dropdownEscola.ClearOptions();
        dropdownTurma.ClearOptions();
        dropdownAluno.ClearOptions();
        dropdownTurma.interactable = false;
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;
        StartCoroutine(BuscarEscolas());
    }

    // =========================================================================
    // BotaoJogar
    // =========================================================================

    public void BotaoJogar()
    {
        int indexAluno = dropdownAluno.value;

        if (indexAluno == 0 || indexAluno >= _alunoNomes.Count)
        {
            Debug.LogWarning("[LUDUS] Nenhum aluno selecionado.");
            return;
        }

        string nomeAluno = _alunoNomes[indexAluno];

        if (LudusSDK.LudusMonitor.Instance != null)
            LudusSDK.LudusMonitor.Instance.StartSession(nomeAluno);
        else
            Debug.LogWarning("[LUDUS] LudusMonitor não encontrado.");

        SceneManager.LoadScene(1);
    }

    // =========================================================================
    // Classes para deserialização do JSON
    // =========================================================================

    [Serializable] private class Escola { public string _id; public string name; }
    [Serializable] private class Turma { public string _id; public string name; }
    [Serializable] private class Aluno { public string _id; public string name; }

    [Serializable] private class EscolasResponse { public List<Escola> escolas; }
    [Serializable] private class TurmasResponse { public List<Turma> turmas; }
    [Serializable] private class AlunosResponse { public List<Aluno> alunos; }
}