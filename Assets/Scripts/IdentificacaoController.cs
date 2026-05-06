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
    public TMP_Dropdown dropdownInstituicao;
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

    private List<string> _instituicaoIds = new List<string>();
    private List<string> _turmaIds = new List<string>();
    private List<string> _alunoIds = new List<string>();
    private List<string> _alunoNomes = new List<string>();

    private List<bool> _alunoCapturaSolicitada = new List<bool>();


    // =========================================================================
    // Unity — Start
    // =========================================================================

    private void Start()
    {
        dropdownInstituicao.interactable = false;
        dropdownTurma.interactable = false;
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;
        botaoRetry.SetActive(false);

        dropdownInstituicao.onValueChanged.AddListener(OnInstituicaoSelected);
        dropdownTurma.onValueChanged.AddListener(OnTurmaSelected);

        StartCoroutine(BuscarInstituicoes());
    }

    // =========================================================================
    // Helpers de feedback
    // =========================================================================

    private void MostrarStatus(string mensagem, bool erro = false)
    {
        if (textoStatus == null) return;
        textoStatus.text = mensagem;
        textoStatus.color = erro
            ? new Color(0.9f, 0.3f, 0.3f)
            : new Color(0.4f, 0.4f, 0.4f);
    }

    private void LimparStatus()
    {
        if (textoStatus != null) textoStatus.text = "";
    }

    // =========================================================================
    // BuscarInstituicoes
    // =========================================================================

    private IEnumerator BuscarInstituicoes()
    {
        MostrarStatus("Carregando instituições...");
        botaoRetry.SetActive(false);
        dropdownInstituicao.interactable = false;

        string url = backendUrl + "/api/unity/schools";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.timeout = 10;

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                MostrarStatus("Servidor indisponível. Verifique a conexão.", erro: true);
                botaoRetry.SetActive(true);
                Debug.LogError("[LUDUS] Erro ao buscar instituições: " + req.error);
                yield break;
            }

            InstituicoesResponse resposta = JsonUtility.FromJson<InstituicoesResponse>(req.downloadHandler.text);

            if (resposta.escolas == null || resposta.escolas.Count == 0)
            {
                MostrarStatus("Nenhuma instituição cadastrada no sistema.", erro: true);
                botaoRetry.SetActive(true);
                yield break;
            }

            dropdownInstituicao.ClearOptions();
            _instituicaoIds.Clear();

            var opcoes = new List<string>();
            opcoes.Add("Selecione a instituição...");
            _instituicaoIds.Add("");

            foreach (var instituicao in resposta.escolas)
            {
                opcoes.Add(instituicao.name);
                _instituicaoIds.Add(instituicao._id);
            }

            dropdownInstituicao.AddOptions(opcoes);
            dropdownInstituicao.value = 0;
            dropdownInstituicao.interactable = true;
            LimparStatus();
        }
    }

    // =========================================================================
    // OnInstituicaoSelected
    // =========================================================================

    private void OnInstituicaoSelected(int index)
    {
        dropdownTurma.ClearOptions();
        dropdownAluno.ClearOptions();
        dropdownTurma.interactable = false;
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;
        LimparStatus();

        if (index == 0) return;

        string instituicaoId = _instituicaoIds[index];
        StartCoroutine(BuscarTurmas(instituicaoId));
    }

    // =========================================================================
    // BuscarTurmas
    // =========================================================================

    private IEnumerator BuscarTurmas(string instituicaoId)
    {
        MostrarStatus("Carregando turmas...");

        string url = backendUrl + "/api/unity/groups/" + instituicaoId;

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.timeout = 10;

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                MostrarStatus("Erro ao carregar turmas. Tente novamente.", erro: true);
                botaoRetry.SetActive(true);
                yield break;
            }

            TurmasResponse resposta = JsonUtility.FromJson<TurmasResponse>(req.downloadHandler.text);

            if (resposta.turmas == null || resposta.turmas.Count == 0)
            {
                MostrarStatus("Nenhuma turma cadastrada nesta instituição.", erro: true);
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
                MostrarStatus("Erro ao carregar alunos. Tente novamente.", erro: true);
                botaoRetry.SetActive(true);
                yield break;
            }

            AlunosResponse resposta = JsonUtility.FromJson<AlunosResponse>(req.downloadHandler.text);

            if (resposta.alunos == null || resposta.alunos.Count == 0)
            {
                MostrarStatus("Nenhum aluno cadastrado nesta turma.", erro: true);
                yield break;
            }

            dropdownAluno.ClearOptions();
            _alunoIds.Clear();
            _alunoNomes.Clear();
            _alunoCapturaSolicitada.Clear();


            var opcoes = new List<string>();
            opcoes.Add("Selecione o aluno...");
            _alunoIds.Add("");
            _alunoNomes.Add("");
            _alunoCapturaSolicitada.Add(false);


            foreach (var aluno in resposta.alunos)
            {
                opcoes.Add(aluno.name);
                _alunoIds.Add(aluno._id);
                _alunoNomes.Add(aluno.name);
                _alunoCapturaSolicitada.Add(aluno.capturaSolicitada);

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
    // BotaoRetry
    // =========================================================================

    public void BotaoRetry()
    {
        botaoRetry.SetActive(false);
        dropdownInstituicao.ClearOptions();
        dropdownTurma.ClearOptions();
        dropdownAluno.ClearOptions();
        dropdownTurma.interactable = false;
        dropdownAluno.interactable = false;
        botaoJogar.GetComponent<UnityEngine.UI.Button>().interactable = false;
        StartCoroutine(BuscarInstituicoes());
    }

    // =========================================================================
    // BotaoJogar
    // =========================================================================

    public void BotaoJogar()
    {
        int indexAluno = dropdownAluno.value;

        if (indexAluno == 0 ||
    indexAluno >= _alunoNomes.Count ||
    indexAluno >= _alunoCapturaSolicitada.Count)
        {
            Debug.LogWarning("[LUDUS] Nenhum aluno selecionado.");
            return;
        }


        string nomeAluno = _alunoNomes[indexAluno];
        bool capturaSolicitada = _alunoCapturaSolicitada[indexAluno];


        if (LudusSDK.LudusMonitor.Instance != null)
            LudusSDK.LudusGameEvents.DefinirJogador(nomeAluno, capturaSolicitada);
        else
            Debug.LogWarning("[LUDUS] LudusMonitor não encontrado.");

        SceneManager.LoadScene(1);
    }

    // =========================================================================
    // Classes para deserialização do JSON
    // =========================================================================

    [Serializable] private class Instituicao { public string _id; public string name; }
    [Serializable] private class Turma { public string _id; public string name; }
    [Serializable] private class Aluno { public string _id; public string name; public bool capturaSolicitada; }

    [Serializable] private class InstituicoesResponse { public List<Instituicao> escolas; }
    [Serializable] private class TurmasResponse { public List<Turma> turmas; }
    [Serializable] private class AlunosResponse { public List<Aluno> alunos; }
}