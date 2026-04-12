using UnityEngine;
using UnityEngine.UI;

public class ThemeColorApplier : MonoBehaviour
{
    public Color normalColor;
    public Color autistaColor;

    private Graphic graphic;

    void Awake()
    {
        graphic = GetComponent<Graphic>();
    }

    void OnEnable()
    {
        ThemeManager.OnThemeChanged += ApplyColor;
        ApplyColor();
    }

    void OnDisable()
    {
        ThemeManager.OnThemeChanged -= ApplyColor;
    }

    void ApplyColor()
    {
        if (ThemeManager.Instance == null) return;

        if (ThemeManager.Instance.currentThemeType == ThemeType.Normal)
            graphic.color = normalColor;
        else
            graphic.color = autistaColor;
    }
}