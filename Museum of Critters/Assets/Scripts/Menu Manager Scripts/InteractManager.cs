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

    public Transform playerCamPos;
    Vector3 playerCameraVector;
    GameObject petHeart;

    float petTimer;
    float petY;
    public bool isPetting;

    //BUGS OUT WHEN TWO PETS ARE NEAR, FIX? (or we just have strictly one pet in one pen)

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        hasTarget = false;
        petTimer = 0;

        isPetting = false;
        //playerCameraYAxis = playerCamPos.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPetting)
        {
            petTimer += Time.deltaTime;

            if (petTimer < 2.0f)
            {
                Debug.Log("Petting");
                petHeart.SetActive(true);

                petHeart.transform.position = new Vector3(petHeart.transform.position.x, petHeart.transform.position.y + 0.001f, petHeart.transform.position.z);
            }
            else
            {
                petHeart.SetActive(false);
                petTimer = 0;
                petHeart.transform.position = new Vector3(petHeart.transform.position.x, petY, petHeart.transform.position.z);

                playerCamPos.transform.position = playerCameraVector;
                isPetting = false;
            }

        }
    }

    public void SetPet(GameObject targetPet)
    {
        pet = targetPet;
        hasTarget = true;
        petHeart = pet.transform.Find("Heart").gameObject;
        petHeart.SetActive(false);
        petY = petHeart.transform.position.y;
        //petHeart.transform.position = new Vector3(petHeart.transform.position.x, petY, petHeart.transform.position.z);

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
        petHeart = null;

        // While not selecting an action, free player and camera
        cameraObj.GetComponent<PlayerCamera>().isRestricted = false;
        playerClass.GetComponent<PlayerMovement>().enabled = true;
        //this.gameObject.SetActive(false);
        Debug.Log("Freed pet!");
    }
     
    public void petThePet()
    {
        if (!isPetting)
        {
            // Crouch and 'pet' the pet
            playerCameraVector = playerCamPos.transform.position;
            if (playerClass.GetComponent<PlayerMovement>().isCrouched == false)
            {
                playerCamPos.transform.position = new Vector3(playerCamPos.transform.position.x, playerCamPos.transform.position.y - 0.75f, playerCamPos.transform.position.z);
            }

            // Play animation
            isPetting = true;
            petHeart.transform.position = new Vector3(petHeart.transform.position.x, petY, petHeart.transform.position.z);

            // Heart shows up above as indicator in update
            Debug.Log("I have petted the pet!");
        }
    }

    public void feedThePet()
    {
        if (!isPetting)
        {
            Debug.Log("I have fed the pet!");
        }
    }

    public void playFetch()
    {
        if (!isPetting)
        {
            Debug.Log("Pet and I are playing fetch!");
        }
    }

    public void Exit()
    {
        if (!isPetting)
        {
            // Exit out of interaction menu (presses button instead of 'E' command)
            FreePet();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            this.gameObject.SetActive(false);
        }
    }
}
