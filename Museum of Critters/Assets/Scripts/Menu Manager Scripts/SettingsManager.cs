using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script that saves variables between scenes and menu settings
[System.Serializable]

public class SettingsManager
{
    // Stores sensitivity settings, starts at 400 (for save system, take from save file)
    public static float sens = PlayerPrefs.GetFloat("SensitivitySens", 400.0f);

    // Stores volume settings (for save system, take from save file)
    public static float volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

    public static AudioClip music = null;
    public static int audioSelect = PlayerPrefs.GetInt("SelectAudio", 0);
    public static float audioTime = 0.0f;
}
