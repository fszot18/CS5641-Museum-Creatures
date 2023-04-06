using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script that saves variables between scenes and menu settings

public class SettingsManager : MonoBehaviour
{
    // Stores sensitivity settings, starts at 400 (for save system, take from save file)
    //public static float sensX = 400;
    //public static float sensY = 400;
    public static float sens = 400;

    // Stores volume settings (for save system, take from save file)
    public static int volume = 25;

    // Stores dominant hand preferance (changes which side hand will appear to pet critter) (for save system, take from save file)
    public static bool isLeftHanded = false;
}
