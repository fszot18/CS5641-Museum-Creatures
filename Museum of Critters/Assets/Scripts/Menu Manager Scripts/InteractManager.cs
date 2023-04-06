using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : MonoBehaviour
{
    //private bool isActive = false;
    public bool hasTarget;

    public GameObject pet = null;
    public GameObject playerClass; //(May not need)
    public Camera cameraObj;

    //BUGS OUT WHEN TWO PETS ARE NEAR, FIX? (or we just have strictly one pet in one pen)

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        hasTarget = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPet(GameObject targetPet)
    {
        pet = targetPet;
        hasTarget = true;

        // While selecting an action, player holds still and camera is restricted
        // Sets target pet and player movement/camera restrictions
        // FIGURE OUT WAY TO GET CAMERA DIRECTION POINT TOWARDS PET (CHANGE HOW DO RESTRICTIONS, ITS ALWAYS POINTING A CERTAIN WAY)
        cameraObj.GetComponent<PlayerCamera>().lockRotation(pet);

        // PUT IN CROUTINE??? OR PUT THIS IN PlayerCamera
        cameraObj.GetComponent<PlayerCamera>().isRestricted = true;
        playerClass.GetComponent<PlayerMovement>().enabled = false;
        Debug.Log("Interact!");
    }

    public void FreePet()
    {
        pet = null;
        hasTarget = false;

        // While not selecting an action, free player and camera
        cameraObj.GetComponent<PlayerCamera>().isRestricted = false;
        playerClass.GetComponent<PlayerMovement>().enabled = true;
        //this.gameObject.SetActive(false);
        Debug.Log("Freed pet!");
    }

    public void petThePet()
    {
        Debug.Log("I have petted the pet!");
    }

    public void feedThePet()
    {
        Debug.Log("I have fed the pet!");
    }

    public void playFetch()
    {
        Debug.Log("Pet and I are playing fetch!");
    }

    public void Exit()
    {
        // Exit out of interaction menu (presses button instead of 'E' command)
        FreePet();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.gameObject.SetActive(false);
    }
}
