using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject settingsCanvas;

    public Camera mainCam;
    public float camSpeed;
    public float maxPause;
    public Slider sensSlider;
    public Slider volSlider;

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
        timer = 0;

        // May have to change once we create a save system
        // Set slider values to global variables in Settings Manager
        sensSlider.value = SettingsManager.sens;
        volSlider.value = SettingsManager.volume;
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
        Application.Quit();
    }

    public void Delete()
    {
        // Deletes saved progress and starts a fresh new museum
        Debug.Log("The game may be deleted");
        // Ask user for permission first
        // Then delete
    }

    public void SensChange()
    {
        SettingsManager.sens = sensSlider.value;
        //Debug.Log("Sensitivity value: " + SettingsManager.sensX);
        //Debug.Log("Slider value: " + sensSlider.value);
    }

    public void VolChange()
    {
        SettingsManager.volume = (int)volSlider.value;
    }
}
