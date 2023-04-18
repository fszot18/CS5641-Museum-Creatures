using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FOR NOW JUST LOOKS AT PLAYER, BUT COULD BE USED FOR OTHER ACTIONS (NEED TO TWEAK/CHANGE)
// Attach this script to a collider that surrounds the pet specifically so pet can sense when player is near
// Essentially is the controller for determining actions that the pet should do with the player

public class PetInteraction : MonoBehaviour
{
    public GameObject petClass;     // Specifies which pet has these interactions
    public GameObject playerClass;  // Specifies the player gameobject, so pet can interact with them
    public GameObject interMenu;    // Specifies the interaction menu (sets it active)
    public Camera playerCam;        // Specifies player camera, lets pet know if they are busy
    public bool isLooking;          // Bool that determines whether pet is looking at the player
    public float petH;              // Float for determining pet's height
    public LayerMask isGround;      // Which tag refers to ground

    public KeyCode interactKey = KeyCode.E; // Interaction key that should only appear when near pet

    bool petIsGrounded;             // Bool that determines whether pet is touching the ground

    private void Start()
    {
        isLooking = false;
        petClass.transform.GetComponent<PetMovement_Idle>().enabled = true;

        //Debug.Log(SettingsManager.isLeftHanded);
    }

    private void Update()
    {
        //Checks if pet is touching the groud
        petIsGrounded = Physics.Raycast(petClass.transform.position, Vector3.down, petH * 0.5f, isGround);

        if (isLooking)
        {
            // Do some fun action, probably an animation
            //petClass.transform.RotateAround(playerClass.transform.position, playerClass.transform.up, 90f * Time.deltaTime);
            petClass.transform.LookAt(playerClass.transform);

            // Show 'E to interact' at top of pet

            // I made 'em jump cause its cute, even if it is janky
            if (playerClass.transform.GetComponent<PlayerMovement>().isJumping == true && petIsGrounded)
            {
                petClass.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 1.5f, ForceMode.Impulse);
                petClass.transform.GetComponent<Rigidbody>().AddForce(Vector3.down * 1.2f, ForceMode.Impulse);
            } 

            if (Input.GetKeyDown(interactKey) && petIsGrounded && interMenu.GetComponent<InteractManager>().isPetting == false)
            {
                //BUGS OUT WHEN TWO PETS ARE NEAR, FIX? (or we just have strictly one pet in one pen)

                //interMenu.SetActive(!interMenu.activeSelf);
                //Cursor.visible = interMenu.activeSelf;
                if (interMenu.activeSelf == false)
                {
                    interMenu.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    // So interact manager knows which pet to play with
                    // Affects cursor and which pet to target
                    interMenu.GetComponent<InteractManager>().SetPet(petClass);
                }
                else
                {
                    interMenu.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    // Free interact manager's target pet
                    // Affects cursor and which pet to target
                    interMenu.GetComponent<InteractManager>().FreePet();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Will probably hold all of the pet's "actions"
        // Right now it just looks at the player

        if (collision.gameObject.name == "Player Obj" && playerCam.GetComponent<PlayerCamera>().isRestricted == false)
        {
            // Disable idle script, cause otherwise we have problems
            petClass.transform.GetComponent<PetMovement_Idle>().enabled = false;

            petClass.transform.LookAt(playerClass.transform);
            isLooking = true;

            petClass.GetComponent<PetMovement_Idle>().shouldMove = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Player Obj")
        {
            // Resets pet's rotation so that it is 'flat' on the ground again
            // May need to change when we put actual models in (attach to model's head instead?)
            petClass.transform.eulerAngles = new Vector3(0.0f, petClass.transform.eulerAngles.y, 0.0f);
            isLooking = false;

            petClass.transform.GetComponent<PetMovement_Idle>().enabled = true;
            petClass.GetComponent<PetMovement_Idle>().shouldMove = true;
        }
    }
}
