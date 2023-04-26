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

    //public Transform playerCamPos;
    Vector3 playerCameraVector;
    GameObject petHeart;
    public GameObject petFood;
    public GameObject ballObject;

    float petTimer;
    float petY;
    public bool isPetting;
    public bool isFeeding;
    public bool isFetching;
    bool goTowardPlayer;

    Vector3 currentPos;
    Vector3 targetPos;
    Vector3 originPos;

    //BUGS OUT WHEN TWO PETS ARE NEAR, FIX? (or we just have strictly one pet in one pen)

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        hasTarget = false;
        petTimer = 0.0f;

        isPetting = false;
        isFeeding = false;
        isFetching = false;
        goTowardPlayer = false;
        petFood.SetActive(false);


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
                petTimer = 0.0f;
                petHeart.transform.position = new Vector3(petHeart.transform.position.x, petY, petHeart.transform.position.z);

                cameraObj.transform.position = playerCameraVector;
                //playerCamPos.transform.position = playerCameraVector;
                isPetting = false;
            }

        }

        if (isFeeding)
        {
            petTimer += Time.deltaTime;

            if (petTimer < 5.0f)
            {
                Debug.Log("Feeding");
                petFood.SetActive(true);

                // Move pet
                //pet.transform.LookAt(petFood.transform);
                var rotation = Quaternion.LookRotation(petFood.transform.position - pet.transform.position);
                pet.transform.rotation = Quaternion.Slerp(pet.transform.rotation, rotation, Time.deltaTime * 4.25f);

                // Make pet jump three times to give illusion of eating
                if (petTimer >= 1.0f && petTimer < 1.5f) {
                    pet.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 1.25f, ForceMode.Impulse);
                    pet.transform.GetComponent<Rigidbody>().AddForce(Vector3.down * 1f, ForceMode.Impulse);
                }
                else if (petTimer >= 2.0f && petTimer < 2.5f)
                {
                    pet.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 1.25f, ForceMode.Impulse);
                    pet.transform.GetComponent<Rigidbody>().AddForce(Vector3.down * 1f, ForceMode.Impulse);
                }
                else if (petTimer >= 4.0f && petTimer < 4.5f)
                {
                    pet.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 1.25f, ForceMode.Impulse);
                    pet.transform.GetComponent<Rigidbody>().AddForce(Vector3.down * 1f, ForceMode.Impulse);
                }
            }
            else
            {
                petFood.SetActive(false);
                petTimer = 0.0f;


                //playerCamPos.transform.position = playerCameraVector;
                cameraObj.transform.position = playerCameraVector;
                isFeeding = false;
            }
        }

        if (isFetching)
        {
            petTimer += Time.deltaTime;
            //playerCamPos.transform.LookAt(pet.transform);

            currentPos = pet.transform.position;
            var step = (pet.GetComponent<PetMovement_Idle>().speed / 2.0f) * Time.deltaTime;

            // First go towards ball, then go towards player
            if (!goTowardPlayer)
            {
                targetPos = ballObject.transform.position;
            }
            else
            {
                // Pet goes back to its original position
                targetPos = originPos;
            }

            pet.transform.position = Vector3.MoveTowards(currentPos, targetPos, step);

            if (Vector3.Distance(currentPos, targetPos) <= 0.0f)
            {
                // If pet caught up to player
                if (goTowardPlayer)
                {
                    pet.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    ballObject.SetActive(false);
                    petTimer = 0.0f;

                    //playerCamPos.transform.position = playerCameraVector;
                    cameraObj.transform.position = playerCameraVector;
                    isFetching = false;
                    goTowardPlayer = false;
                }
                else
                {
                    // Make pet come back to player
                    Debug.Log("Caught up!");
                    goTowardPlayer = true;
                }
            }
            else
            {
                var fetchRot = Quaternion.LookRotation(targetPos - currentPos);
                pet.transform.rotation = Quaternion.Slerp(pet.transform.rotation, fetchRot, Time.deltaTime * 4.25f);

                Debug.Log("Running");
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
        originPos = pet.transform.position;
        //petHeart.transform.position = new Vector3(petHeart.transform.position.x, petY, petHeart.transform.position.z);

        // While selecting an action, player holds still and camera is restricted
        // Sets target pet and player movement/camera restrictions
        // FIGURE OUT WAY TO GET CAMERA DIRECTION POINT TOWARDS PET (CHANGE HOW DO RESTRICTIONS, ITS ALWAYS POINTING A CERTAIN WAY)
        cameraObj.GetComponent<PlayerCamera>().lockRotation(pet);

        // PUT IN CROUTINE??? OR PUT THIS IN PlayerCamera
        cameraObj.GetComponent<PlayerCamera>().isRestricted = true;
        playerClass.GetComponent<PlayerMovement>().enabled = false;
        playerCameraVector = cameraObj.transform.position;
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
        if (!isPetting && !isFeeding && !isFetching)
        {
            // Crouch and 'pet' the pet
            //playerCameraVector = playerCamPos.transform.position;
            if (playerClass.GetComponent<PlayerMovement>().isCrouched == false)
            {
                cameraObj.transform.position = new Vector3(cameraObj.transform.position.x, cameraObj.transform.position.y - 0.75f, cameraObj.transform.position.z);
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
        if (!isPetting && !isFeeding && !isFetching)
        {
            // Crouch to 'feed' the pet (decided it looked weird
            //playerCameraVector = playerCamPos.transform.position;
            //if (playerClass.GetComponent<PlayerMovement>().isCrouched == false)
            //{
            //    cameraObj.transform.position = new Vector3(cameraObj.transform.position.x, cameraObj.transform.position.y - 0.75f, cameraObj.transform.position.z);
            //}

            // This is all still janky, but it works good enough considering it's 2D in a 3D environment
            // Position food bowl into the correct spot
            petFood.transform.position = pet.transform.position + pet.transform.right;

            // Make food bowl's plane face the player's camera
            petFood.transform.LookAt(cameraObj.transform.position);
            petFood.transform.rotation = Quaternion.Euler(petFood.transform.rotation.eulerAngles.x, petFood.transform.rotation.eulerAngles.y + 180, petFood.transform.rotation.eulerAngles.z);

            // Play animation for turning and eating
            isFeeding = true;
            Debug.Log("I have fed the pet!");
        }
    }

    public void playFetch()
    {
        if (!isPetting && !isFeeding && !isFetching)
        {
            // Player stands to play fetch
            //playerCameraVector = playerCamPos.transform.position;
            if (playerClass.GetComponent<PlayerMovement>().isCrouched == true)
            {
                cameraObj.transform.position = new Vector3(cameraObj.transform.position.x, cameraObj.transform.position.y + 0.75f, cameraObj.transform.position.z);
            }

            // Grab pet's original position before it starts fetching (does this when setting the pet)
            // Make sure ball touches the ground
            ballObject.transform.position = playerClass.transform.position + (playerClass.transform.Find("Orientation").forward * 7.0f);
            ballObject.transform.position = new Vector3(ballObject.transform.position.x, 0.25f, ballObject.transform.position.z);
            // Somehow throw it?

            isFetching = true;
            ballObject.SetActive(true);
            Debug.Log("Pet and I are playing fetch!");
        }
    }

    public void Exit()
    {
        if (!isPetting && !isFeeding && !isFetching)
        {
            // Exit out of interaction menu (presses button instead of 'E' command)
            FreePet();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            this.gameObject.SetActive(false);
        }
    }
}
