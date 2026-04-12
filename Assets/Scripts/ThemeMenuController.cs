using UnityEngine;
using UnityEngine.UI;

public class ThemeMenuController : MonoBehaviour
{
    [Header("Buttons")]
    public Button btnNormal;
    public Button btnAutista;

    [Header("Images")]
    public Image imgNormal;
    public Image imgAutista;

    [Header("Sprites")]
    public Sprite normalSelected;
    public Sprite normalUnselected;

    public Sprite autistaSelected;
    public Sprite autistaUnselected;

    [Header("Confirmation")]
    public GameObject confirmPanel;
    public Animator confirmAnimator;

    private ThemeType pendingTheme;

    private void Start()
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        ThemeType current = ThemeManager.Instance.currentThemeType;

        if (current == ThemeType.Normal)
        {
            imgNormal.sprite = normalSelected;
            imgAutista.sprite = autistaUnselected;
        }
        else
        {
            imgNormal.sprite = normalUnselected;
            imgAutista.sprite = autistaSelected;
        }
    }

    public void OnClickNormal()
    {
        if (ThemeManager.Instance.currentThemeType == ThemeType.Normal)
            return;

        OpenConfirmation(ThemeType.Normal);
    }

    public void OnClickAutista()
    {
        if (ThemeManager.Instance.currentThemeType == ThemeType.Autista)
            return;

        OpenConfirmation(ThemeType.Autista);
    }

    void OpenConfirmation(ThemeType theme)
    {
        pendingTheme = theme;

        confirmPanel.SetActive(true);
        confirmAnimator.SetTrigger("Open");
    }

    public void ConfirmChange()
    {
        ThemeManager.Instance.SetTheme(pendingTheme);

        confirmAnimator.SetTrigger("Close");
        Invoke(nameof(DisableConfirmPanel), 0.75f);

        UpdateVisual();
    }

    public void CancelChange()
    {
        confirmAnimator.SetTrigger("Close");
        Invoke(nameof(DisableConfirmPanel), 0.75f);
    }

    void DisableConfirmPanel()
    {
        confirmPanel.SetActive(false);
    }
}