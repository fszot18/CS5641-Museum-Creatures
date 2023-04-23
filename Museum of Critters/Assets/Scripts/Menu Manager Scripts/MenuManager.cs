using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] AudioSource backgroundM;
    [SerializeField] AudioClip[] availableMusic;

    public Camera mainCam;
    public float camSpeed;
    public float maxPause;
    public Slider sensSlider;
    public Slider volSlider;
    public TMP_Dropdown musicDropdown;

    private bool isActive;
    private bool enterMuseum;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        // The default active canvas is true
        isActive = true;
        enterMuseum = false;
        mainCanvas.SetActive(isActive);
        settingsCanvas.SetActive(!isActive);
        timer = 0.0f;

        //SettingsManager.audioSelect = PlayerPrefs.GetInt("SelectAudio", 0);
        //SettingsManager.sens = PlayerPrefs.GetFloat("SensitivitySens", 400.0f);
        //SettingsManager.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        //Debug.Log("From start: " + PlayerPrefs.GetFloat("MusicVolume"));

        // May have to change once we create a save system
        // Set slider values to global variables in Settings Manager
        sensSlider.value = SettingsManager.sens;
        volSlider.value = SettingsManager.volume;
        musicDropdown.value = SettingsManager.audioSelect;

        string audioName = musicDropdown.options[musicDropdown.value].text;
        AudioClip sound = null;
        sound = Array.Find(availableMusic, sound => sound.name == audioName);

        backgroundM.volume = SettingsManager.volume;
        if (SettingsManager.music != null)
        {
            backgroundM.clip = SettingsManager.music;
        } else {
            backgroundM.clip = sound;
            SettingsManager.music = sound;
        }
        backgroundM.time = SettingsManager.audioTime;
        backgroundM.Play();
    }

    void Update()
    {
        if (enterMuseum && timer < maxPause)
        {
            timer += Time.deltaTime;
            //mainCam.transform.position = mainCam.transform.forward * 1.0f;
            mainCam.fieldOfView = mainCam.fieldOfView - camSpeed;
        }
    }

    public void Play(string newScene)
    {
        // Load main scene which is the museum
        // Move camera towards door
        enterMuseum = true;
        mainCanvas.SetActive(!isActive);
        settingsCanvas.SetActive(!isActive);
        StartCoroutine(enteringPause(newScene));
    }

    IEnumerator enteringPause(string newScene)
    {
        yield return new WaitForSeconds(maxPause);
        SettingsManager.music = backgroundM.clip;
        SettingsManager.audioTime = backgroundM.time;
        Save();
        SceneManager.LoadScene(newScene);
    }

    public void loadCanvas(GameObject turnOn)
    {
        // Display settings canvas
        mainCanvas.SetActive(!isActive);
        settingsCanvas.SetActive(!isActive);

        turnOn.SetActive(isActive);
    }

    public void Quit()
    {
        // "Walk Away" option, makes the game quit
        Debug.Log("The game has exited");
        Save();
        Application.Quit();
    }

    public void Delete()
    {
        // Deletes saved progress and starts a fresh new museum
        backgroundM.Stop();
        Debug.Log("The game may be deleted");
        PlayerPrefs.DeleteAll();

        SettingsManager.audioSelect = PlayerPrefs.GetInt("SelectAudio", 0);
        SettingsManager.sens = PlayerPrefs.GetFloat("SensitivitySens", 400.0f);
        SettingsManager.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        //Debug.Log("From delete: " + PlayerPrefs.GetFloat("MusicVolume"));

        SettingsManager.audioTime = 0.0f;
        SettingsManager.music = null;

        sensSlider.value = SettingsManager.sens;
        volSlider.value = SettingsManager.volume;

        musicDropdown.value = SettingsManager.audioSelect;
        backgroundM.volume = SettingsManager.volume;

        string audioName = musicDropdown.options[musicDropdown.value].text;
        AudioClip sound = null;
        sound = Array.Find(availableMusic, sound => sound.name == audioName);
        backgroundM.clip = sound;
        SettingsManager.music = sound;

        //backgroundM.clip = SettingsManager.music;
        backgroundM.time = SettingsManager.audioTime;
        backgroundM.Play();
        // Ask user for permission first
        // Then delete
    }

    public void Save()
    {
        // Ask for confirmation
        // Save
        Debug.Log("I have saved!");
        PlayerPrefs.SetInt("SelectAudio", SettingsManager.audioSelect);
        PlayerPrefs.SetFloat("SensitivitySens", SettingsManager.sens);  //change?
        PlayerPrefs.SetFloat("MusicVolume", SettingsManager.volume);    //change?

        //Debug.Log("From Save: " + PlayerPrefs.GetFloat("MusicVolume"));
        PlayerPrefs.Save();
    }

    public void SensChange()
    {
        SettingsManager.sens = sensSlider.value;
        //PlayerPrefs.SetFloat("SensitivitySens", SettingsManager.sens);  //change?
        //PlayerPrefs.Save();
        //Debug.Log("Sensitivity value: " + SettingsManager.sensX);
        //Debug.Log("Slider value: " + sensSlider.value);
    }

    public void VolChange()
    {
        SettingsManager.volume = volSlider.value;
        backgroundM.volume = SettingsManager.volume;
        //Debug.Log("From vol change: " + PlayerPrefs.GetFloat("MusicVolume"));
        //PlayerPrefs.Save();
    }

    public void MusicChange()
    {
        string audioName = musicDropdown.options[musicDropdown.value].text;
        AudioClip sound = null;
        sound = Array.Find(availableMusic, sound => sound.name == audioName);

        if (SettingsManager.audioSelect != musicDropdown.value)
        {
            backgroundM.Stop();
            backgroundM.clip = sound;
            backgroundM.time = 0.0f;
            SettingsManager.music = sound;
            SettingsManager.audioTime = 0.0f;
            SettingsManager.audioSelect = musicDropdown.value;
            backgroundM.Play();
        }
        PlayerPrefs.SetInt("SelectAudio", SettingsManager.audioSelect);
        PlayerPrefs.Save();
    }
}
