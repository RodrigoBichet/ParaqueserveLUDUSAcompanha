using System;
using UnityEngine;

public enum ThemeType
{
    Normal,
    Autista
}

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    public ThemeData themeNormal;
    public ThemeData themeAutista;

    public ThemeType currentThemeType;

    public ThemeData CurrentTheme =>
        currentThemeType == ThemeType.Normal ? themeNormal : themeAutista;

    public static event Action OnThemeChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTheme();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTheme(ThemeType type)
    {
        if (currentThemeType == type) return;

        currentThemeType = type;
        SaveTheme();
        OnThemeChanged?.Invoke();
    }

    void SaveTheme()
    {
        PlayerPrefs.SetInt("theme", (int)currentThemeType);
    }

    void LoadTheme()
    {
        currentThemeType = (ThemeType)PlayerPrefs.GetInt("theme", 0);
    }
}
