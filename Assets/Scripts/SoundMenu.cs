using UnityEngine;

public class SoundMenu : MonoBehaviour
{
    public AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.Instance;
        audioManager.PlayAudio();
    }

    public void ToggleSound()
    {
        audioManager.ToggleMute();
    }
}
