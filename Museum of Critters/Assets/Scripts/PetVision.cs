using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetVision : MonoBehaviour
{
    public GameObject petClass;
    public GameObject playerClass;
    bool isLooking;

    //FOR NOW JUST LOOKS AT PLAYER, BUT COULD BE USED FOR OTHER ACTIONS (NEED TO TWEAK/CHANGE)

    private void Start()
    {
        isLooking = false;
        petClass.transform.GetComponent<PetMovement_Idle>().enabled = true;
    }

    private void Update()
    {
        if (isLooking) {
            //Do some fun action, probably an animation
            //petClass.transform.RotateAround(playerClass.transform.position, playerClass.transform.up, 90f * Time.deltaTime);
            petClass.transform.LookAt(playerClass.transform);

            //I made 'em jump cause its cute, even if it is janky
            if (playerClass.transform.GetComponent<PlayerMovement>().isJumping == true) {
                petClass.transform.GetComponent<Rigidbody>().AddForce(petClass.transform.up * 1.5f, ForceMode.Impulse);
                petClass.transform.GetComponent<Rigidbody>().AddForce(Vector3.down * 10 * petClass.transform.GetComponent<Rigidbody>().mass);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Will probably hold all of the pet's "actions"
        //Right now it just looks at the player

        if (collision.gameObject.name == "Player Obj")
        {
            //Disable idle script, cause otherwise we have problems
            petClass.transform.GetComponent<PetMovement_Idle>().enabled = false;

            petClass.transform.LookAt(playerClass.transform);
            isLooking = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        petClass.transform.eulerAngles = new Vector3(0.0f, petClass.transform.eulerAngles.y, 0.0f);
        isLooking = false;

        petClass.transform.GetComponent<PetMovement_Idle>().enabled = true;
    }
}
