using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that helps with idle movement and maneuvering around ojects if they are in the way
// Attach this script to the pet's line of sight object specifically

public class PetVision : MonoBehaviour
{
    public GameObject petClass;
    public GameObject petInteract;

    public float rotateSpeed;       // Controls how fast pet will turn
    bool reposition;                // Bool that determines whether pet should reposition themselves because of an object
    int[] rotateDir;                // Holds -1 and 1 for random rotation direction
    int randDir;                    // Holds random value for which direction pet should turn
    int randIndex;                  // Holds random index from randDir for random rotation direction

    private void Start()
    {
        reposition = false;
        rotateDir = new int[] {-1, 1};
        randIndex = Random.Range(0, rotateDir.Length);
        randDir = rotateDir[randIndex];
        //Debug.Log("randDir : " + randDir);
    }

    private void Update()
    {
        if (reposition)
        {
            petClass.transform.Rotate(0.0f, Time.deltaTime * ((float) randDir * rotateSpeed), 0.0f, Space.Self);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        //For AI wall detection Iif something is tagged "Uninteractable" the pets sees it as an obstacle to avoid)
        if (collision.gameObject.CompareTag("Uninteractable") && petInteract.GetComponent<PetInteraction>().isLooking == false)
        {
            //petClass.GetComponent<Rigidbody>().velocity = -1.0f * petClass.transform.forward * 5;
            petClass.GetComponent<PetMovement_Idle>().shouldMove = false;
            //petClass.GetComponent<Rigidbody>().velocity = Vector3.zero;
            reposition = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Uninteractable") && petInteract.GetComponent<PetInteraction>().isLooking == false)
        {
            randIndex = Random.Range(0, rotateDir.Length);
            randDir = rotateDir[randIndex];
            //Debug.Log("randDir redone: " + randDir);
            reposition = false;
            petClass.GetComponent<PetMovement_Idle>().shouldMove = true;
        }
    }
}
