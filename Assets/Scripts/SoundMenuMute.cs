using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenuMute : MonoBehaviour
{
    public SoundMenu soundManager;

    public void ToggleSound()
    {
        soundManager.ToggleSound();
    }
}
