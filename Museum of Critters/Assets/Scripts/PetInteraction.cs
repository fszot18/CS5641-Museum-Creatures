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
    public bool isLooking;          // Bool that determines whether pet is looking at the player

    private void Start()
    {
        isLooking = false;
        petClass.transform.GetComponent<PetMovement_Idle>().enabled = true;
    }

    private void Update()
    {
        if (isLooking)
        {
            // Do some fun action, probably an animation
            //petClass.transform.RotateAround(playerClass.transform.position, playerClass.transform.up, 90f * Time.deltaTime);
            petClass.transform.LookAt(playerClass.transform);

            // I made 'em jump cause its cute, even if it is janky
            if (playerClass.transform.GetComponent<PlayerMovement>().isJumping == true)
            {
                petClass.transform.GetComponent<Rigidbody>().AddForce(petClass.transform.up * 1.5f, ForceMode.Impulse);
                petClass.transform.GetComponent<Rigidbody>().AddForce(Vector3.down * 10 * petClass.transform.GetComponent<Rigidbody>().mass);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Will probably hold all of the pet's "actions"
        // Right now it just looks at the player

        if (collision.gameObject.name == "Player Obj")
        {
            // Disable idle script, cause otherwise we have problems
            petClass.transform.GetComponent<PetMovement_Idle>().enabled = false;

            petClass.transform.LookAt(playerClass.transform);
            isLooking = true;
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
        }
    }
}
