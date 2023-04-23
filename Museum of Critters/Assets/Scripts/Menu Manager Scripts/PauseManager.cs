using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool isActive = false;
    public KeyCode pauseKey = KeyCode.Escape;

    [SerializeField] AudioSource backgroundM;

    public GameObject pauseMenu;
    public GameObject interMenu;
    public GameObject playerClass;
    public GameObject cameraClass;
    public Camera cameraObj;

    public Slider sensSlider;
    public Slider volSlider;

    void Start()
    {
        // Makes sure everything is activated correctly at start-up
        pauseMenu.SetActive(isActive);

        playerClass.GetComponent<PlayerMovement>().enabled = !isActive;
        cameraClass.GetComponent<MoveCamera>().enabled = !isActive;
        cameraObj.GetComponent<PlayerCamera>().enabled = !isActive;

        // Make sure settings are saved between scenes (sensX and sensY should be the same)
        sensSlider.value = SettingsManager.sens;
        volSlider.value = SettingsManager.volume;
        backgroundM.volume = SettingsManager.volume;
        backgroundM.clip = SettingsManager.music;
        backgroundM.time = SettingsManager.audioTime;
        backgroundM.Play();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey) && interMenu.activeSelf == false)
        {
            // This is opposite so it can also be used to get out of the pause menu instead of clicking resume
            isActive = !isActive;

            // Brings up pause menu and deactivates camera and player movement
            pauseMenu.SetActive(isActive);

            playerClass.GetComponent<PlayerMovement>().enabled = !isActive;
            cameraClass.GetComponent<MoveCamera>().enabled = !isActive;
            cameraObj.GetComponent<PlayerCamera>().enabled = !isActive;

            // Unfreeze cursor and make it visible if pause is active
            Cursor.visible = isActive;
            if (isActive)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Save();
            }
        }
    }

    public void Resume()
    {
        Save();
        isActive = false;
        pauseMenu.SetActive(isActive);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerClass.GetComponent<PlayerMovement>().enabled = !isActive;
        cameraClass.GetComponent<MoveCamera>().enabled = !isActive;
        cameraObj.GetComponent<PlayerCamera>().enabled = !isActive;
    }

    public void SensChange()
    {
        SettingsManager.sens = sensSlider.value;
        //PlayerPrefs.SetFloat("Sens", SettingsManager.sens); //change?
        //PlayerPrefs.Save();
    }

    public void VolChange()
    {
        SettingsManager.volume = volSlider.value;
        backgroundM.volume = SettingsManager.volume;
        //PlayerPrefs.SetFloat("Volume", SettingsManager.volume); //change?
        //PlayerPrefs.Save();
    }

    public void BackMainMenu(string newScene)
    {
        // Ask if want to save here???
        Save();

        isActive = false;
        pauseMenu.SetActive(isActive);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SettingsManager.audioTime = backgroundM.time;

        SceneManager.LoadScene(newScene);
    }

    public void Save()
    {
        // Ask for confirmation
        // Save
        Debug.Log("I have saved!");
        PlayerPrefs.SetInt("SelectAudio", SettingsManager.audioSelect);
        PlayerPrefs.SetFloat("SensitivitySens", SettingsManager.sens); //change?
        PlayerPrefs.SetFloat("MusicVolume", SettingsManager.volume); //change?
        PlayerPrefs.Save();
    }
}
