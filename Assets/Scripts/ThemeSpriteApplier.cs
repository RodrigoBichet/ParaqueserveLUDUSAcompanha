using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ThemeSpriteApplier : MonoBehaviour
{
    //ADICIONAR AQUI
    public enum SpriteRole
    {
        acoes,
        alimentos,
        apoio,
        avancar,
        background,
        botaoTutorial,
        configuracao,
        cotidiano,
        creditos,
        diversao,
        equipe,
        estrela,
        higiene,
        informacao,
        instituicoes,
        interrogacao,
        menuPrincipal,
        musica,
        muitoBem,
        nomes,
        parceria,
        parcerias,
        playButton,
        quaseLa,
        recomecar,
        tenteNovamente,
        titleInterrogacao,
        titlePara,
        titleQue,
        titleServe,
        tutorial,
        tutorialPlay,
        tutorialVoltar,
        volume,
        volumeGeral,
        volumeLevel,
        voltar,
        web,
        instagram,
        interrogacaoParaqueserve,
        estrelaEsquerda,
        estrelaDireita,
        conjuntoAcao,
        conjuntoAlimentos,
        conjuntoCotidiano,
        conjuntoDiversao,
        conjuntoHigiene,
        fundoAudio,
        fundoItem,
        erradoSelectLevel,
        quaseSelectLevel,
        certoSelectLevel,
        paraqueserveDesativado,
        tema,
        handleVolume,
        titleConfirm,
        iconConfirm,
        iconCancel,
        interrogacaoLoader


    }

    public SpriteRole spriteRole;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        ThemeManager.OnThemeChanged += ApplyTheme;
        ApplyTheme();
    }

    private void OnDisable()
    {
        ThemeManager.OnThemeChanged -= ApplyTheme;
    }

    //ADICIONAR AQUI
    void ApplyTheme()
    {
        if (ThemeManager.Instance == null) return;

        var theme = ThemeManager.Instance.CurrentTheme;
        if (theme == null) return;

        RectTransform rt = image.rectTransform;

        switch (spriteRole)
        {
            case SpriteRole.acoes:
                image.sprite = theme.acoes;
                rt.sizeDelta = theme.acoesSize;
                break;

            case SpriteRole.alimentos:
                image.sprite = theme.alimentos;
                rt.sizeDelta = theme.alimentosSize;
                break;

            case SpriteRole.apoio:
                image.sprite = theme.apoio;
                rt.sizeDelta = theme.apoioSize;
                break;

            case SpriteRole.avancar:
                image.sprite = theme.avancar;
                rt.sizeDelta = theme.avancarSize;
                break;

            case SpriteRole.background:
                image.sprite = theme.background;
                rt.sizeDelta = theme.backgroundSize;
                break;

            case SpriteRole.botaoTutorial:
                image.sprite = theme.botaoTutorial;
                rt.sizeDelta = theme.botaoTutorialSize;
                break;

            case SpriteRole.configuracao:
                image.sprite = theme.configuracao;
                rt.sizeDelta = theme.configuracaoSize;
                break;

            case SpriteRole.cotidiano:
                image.sprite = theme.cotidiano;
                rt.sizeDelta = theme.cotidianoSize;
                break;

            case SpriteRole.creditos:
                image.sprite = theme.creditos;
                rt.sizeDelta = theme.creditosSize;
                break;

            case SpriteRole.diversao:
                image.sprite = theme.diversao;
                rt.sizeDelta = theme.diversaoSize;
                break;

            case SpriteRole.equipe:
                image.sprite = theme.equipe;
                rt.sizeDelta = theme.equipeSize;
                break;

            case SpriteRole.estrela:
                image.sprite = theme.estrela;
                rt.sizeDelta = theme.estrelaSize;
                break;

            case SpriteRole.higiene:
                image.sprite = theme.higiene;
                rt.sizeDelta = theme.higieneSize;
                break;

            case SpriteRole.informacao:
                image.sprite = theme.informacao;
                rt.sizeDelta = theme.informacaoSize;
                break;

            case SpriteRole.instituicoes:
                image.sprite = theme.instituicoes;
                rt.sizeDelta = theme.instituicoesSize;
                break;

            case SpriteRole.interrogacao:
                image.sprite = theme.interrogacao;
                rt.sizeDelta = theme.interrogacaoSize;
                break;

            case SpriteRole.menuPrincipal:
                image.sprite = theme.menuPrincipal;
                rt.sizeDelta = theme.menuPrincipalSize;
                break;

            case SpriteRole.musica:
                image.sprite = theme.musica;
                rt.sizeDelta = theme.musicaSize;
                break;

            case SpriteRole.muitoBem:
                image.sprite = theme.muitoBem;
                rt.sizeDelta = theme.muitoBemSize;
                break;

            case SpriteRole.nomes:
                image.sprite = theme.nomes;
                rt.sizeDelta = theme.nomesSize;
                break;

            case SpriteRole.parceria:
                image.sprite = theme.parceria;
                rt.sizeDelta = theme.parceriaSize;
                break;

            case SpriteRole.parcerias:
                image.sprite = theme.parcerias;
                rt.sizeDelta = theme.parceriasSize;
                break;

            case SpriteRole.playButton:
                image.sprite = theme.playButton;
                rt.sizeDelta = theme.playButtonSize;
                break;

            case SpriteRole.quaseLa:
                image.sprite = theme.quaseLa;
                rt.sizeDelta = theme.quaseLaSize;
                break;

            case SpriteRole.recomecar:
                image.sprite = theme.recomecar;
                rt.sizeDelta = theme.recomecarSize;
                break;

            case SpriteRole.tenteNovamente:
                image.sprite = theme.tenteNovamente;
                rt.sizeDelta = theme.tenteNovamenteSize;
                break;

            case SpriteRole.titleInterrogacao:
                image.sprite = theme.titleInterrogacao;
                rt.sizeDelta = theme.titleInterrogacaoSize;
                break;

            case SpriteRole.titlePara:
                image.sprite = theme.titlePara;
                rt.sizeDelta = theme.titleParaSize;
                break;

            case SpriteRole.titleQue:
                image.sprite = theme.titleQue;
                rt.sizeDelta = theme.titleQueSize;
                break;

            case SpriteRole.titleServe:
                image.sprite = theme.titleServe;
                rt.sizeDelta = theme.titleServeSize;
                break;

            case SpriteRole.tutorial:
                image.sprite = theme.tutorial;
                rt.sizeDelta = theme.tutorialSize;
                break;

            case SpriteRole.tutorialPlay:
                image.sprite = theme.tutorialPlay;
                rt.sizeDelta = theme.tutorialPlaySize;
                break;

            case SpriteRole.tutorialVoltar:
                image.sprite = theme.tutorialVoltar;
                rt.sizeDelta = theme.tutorialVoltarSize;
                break;

            case SpriteRole.volume:
                image.sprite = theme.volume;
                rt.sizeDelta = theme.volumeSize;
                break;

            case SpriteRole.volumeGeral:
                image.sprite = theme.volumeGeral;
                rt.sizeDelta = theme.volumeGeralSize;
                break;

            case SpriteRole.volumeLevel:
                image.sprite = theme.volumeLevel;
                rt.sizeDelta = theme.volumeLevelSize;
                break;

            case SpriteRole.voltar:
                image.sprite = theme.voltar;
                rt.sizeDelta = theme.voltarSize;
                break;

            case SpriteRole.web:
                image.sprite = theme.web;
                rt.sizeDelta = theme.webSize;
                break;

            case SpriteRole.instagram:
                image.sprite = theme.instagram;
                rt.sizeDelta = theme.instagramSize;
                break;

            case SpriteRole.interrogacaoParaqueserve:
                image.sprite = theme.interrogacaoParaqueserve;
                rt.sizeDelta = theme.interrogacaoParaqueserveSize;
                break;

            case SpriteRole.estrelaEsquerda:
                image.sprite = theme.estrelaEsquerda;
                rt.sizeDelta = theme.estrelaEsquerdaSize;
                break;

            case SpriteRole.estrelaDireita:
                image.sprite = theme.estrelaDireita;
                rt.sizeDelta = theme.estrelaDireitaSize;
                break;

            case SpriteRole.conjuntoAcao:
                image.sprite = theme.conjuntoAcao;
                rt.sizeDelta = theme.conjuntoAcaoSize;
                break;

            case SpriteRole.conjuntoAlimentos:
                image.sprite = theme.conjuntoAlimentos;
                rt.sizeDelta = theme.conjuntoAlimentosSize;
                break;

            case SpriteRole.conjuntoCotidiano:
                image.sprite = theme.conjuntoCotidiano;
                rt.sizeDelta = theme.conjuntoCotidianoSize;
                break;

            case SpriteRole.conjuntoDiversao:
                image.sprite = theme.conjuntoDiversao;
                rt.sizeDelta = theme.conjuntoDiversaoSize;
                break;

            case SpriteRole.conjuntoHigiene:
                image.sprite = theme.conjuntoHigiene;
                rt.sizeDelta = theme.conjuntoHigieneSize;
                break;

            case SpriteRole.fundoAudio:
                image.sprite = theme.fundoAudio;
                rt.sizeDelta = theme.fundoAudioSize;
                break;

            case SpriteRole.fundoItem:
                image.sprite = theme.fundoItem;
                rt.sizeDelta = theme.fundoItemSize;
                break;

            case SpriteRole.erradoSelectLevel:
                image.sprite = theme.erradoSelectLevel;
                rt.sizeDelta = theme.erradoSelectLevelSize;
                break;

            case SpriteRole.quaseSelectLevel:
                image.sprite = theme.quaseSelectLevel;
                rt.sizeDelta = theme.quaseSelectLevelSize;
                break;

            case SpriteRole.certoSelectLevel:
                image.sprite = theme.certoSelectLevel;
                rt.sizeDelta = theme.certoSelectLevelSize;
                break;

            case SpriteRole.paraqueserveDesativado:
                image.sprite = theme.paraqueserveDesativado;
                rt.sizeDelta = theme.paraqueserveDesativadoSize;
                break;

            case SpriteRole.tema:
                image.sprite = theme.tema;
                rt.sizeDelta = theme.temaSize;
                break;

            case SpriteRole.handleVolume:
                image.sprite = theme.handleVolume;
                rt.sizeDelta = theme.handleVolumeSize;
                break;

            case SpriteRole.titleConfirm:
                image.sprite = theme.titleConfirm;
                rt.sizeDelta = theme.titleConfirmSize;
                break;

            case SpriteRole.iconConfirm:
                image.sprite = theme.iconConfirm;
                rt.sizeDelta = theme.iconConfirmSize;
                break;

            case SpriteRole.iconCancel:
                image.sprite = theme.iconCancel;
                rt.sizeDelta = theme.iconCancelSize;
                break;

            case SpriteRole.interrogacaoLoader:
                image.sprite = theme.interrogacaoLoader;
                rt.sizeDelta = theme.interrogacaoLoaderSize;
                break;

        }
    }

}
