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

    [Header("Botão")]
    public GameObject botaoJogar;

    // -------------------------------------------------------------------------
    // Configuração
    // -------------------------------------------------------------------------

    [Header("Configuração")]
    [Tooltip("URL base do backend. Ex: http://localhost:3000")]
    public string backendUrl = "http://localhost:3000";

    // -------------------------------------------------------------------------
    // Dados internos
    // -------------------------------------------------------------------------

    // Listas de IDs para mapear seleção do dropdown com o ID real
    private List<string> _escolaIds = new List<string>();
    private List<string> _turmaIds = new List<string>();
    private List<string> _alunoIds = new List<string>();
    private List<string> _alunoNomes = new List<string>();

    // =========================================================================
    // Unity — Start
    // =========================================================================

    private void Start()
    {
        // Desativa turma, aluno e botão até seleção em cascata
        dropdownTurma.interactable = false;
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;

        // Listeners dos dropdowns
        dropdownEscola.onValueChanged.AddListener(OnEscolaSelected);
        dropdownTurma.onValueChanged.AddListener(OnTurmaSelected);

        // Busca escolas ao abrir a cena
        StartCoroutine(BuscarEscolas());
    }

    // =========================================================================
    // BuscarEscolas
    // =========================================================================

    private IEnumerator BuscarEscolas()
    {
        string url = backendUrl + "/api/unity/schools";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[LUDUS] Erro ao buscar escolas: " + req.error);
                yield break;
            }

            EscolasResponse resposta = JsonUtility.FromJson<EscolasResponse>(req.downloadHandler.text);

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
        }
    }

    // =========================================================================
    // OnEscolaSelected — chamado ao selecionar escola
    // =========================================================================

    private void OnEscolaSelected(int index)
    {
        // Reseta turma e aluno
        dropdownTurma.ClearOptions();
        dropdownAluno.ClearOptions();
        dropdownTurma.interactable = false;
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;

        if (index == 0) return; // "Selecione a escola..."

        string escolaId = _escolaIds[index];
        StartCoroutine(BuscarTurmas(escolaId));
    }

    // =========================================================================
    // BuscarTurmas
    // =========================================================================

    private IEnumerator BuscarTurmas(string escolaId)
    {
        string url = backendUrl + "/api/unity/groups/" + escolaId;

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[LUDUS] Erro ao buscar turmas: " + req.error);
                yield break;
            }

            TurmasResponse resposta = JsonUtility.FromJson<TurmasResponse>(req.downloadHandler.text);

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
        }
    }

    // =========================================================================
    // OnTurmaSelected — chamado ao selecionar turma
    // =========================================================================

    private void OnTurmaSelected(int index)
    {
        dropdownAluno.ClearOptions();
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;

        if (index == 0) return; // "Selecione a turma..."

        string turmaId = _turmaIds[index];
        StartCoroutine(BuscarAlunos(turmaId));
    }

    // =========================================================================
    // BuscarAlunos
    // =========================================================================

    private IEnumerator BuscarAlunos(string turmaId)
    {
        string url = backendUrl + "/api/unity/students/" + turmaId;

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[LUDUS] Erro ao buscar alunos: " + req.error);
                yield break;
            }

            AlunosResponse resposta = JsonUtility.FromJson<AlunosResponse>(req.downloadHandler.text);

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

            // Listener do aluno — ativa botão quando selecionado
            dropdownAluno.onValueChanged.RemoveAllListeners();
            dropdownAluno.onValueChanged.AddListener(OnAlunoSelected);
        }
    }

    // =========================================================================
    // OnAlunoSelected — ativa botão ao selecionar aluno
    // =========================================================================

    private void OnAlunoSelected(int index)
    {
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = index > 0;
    }

    // =========================================================================
    // BotaoJogar — chamado pelo botão de play
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
        {
            LudusSDK.LudusMonitor.Instance.StartSession(nomeAluno);
        }
        else
        {
            Debug.LogWarning("[LUDUS] LudusMonitor não encontrado.");
        }

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