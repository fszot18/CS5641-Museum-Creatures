using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    //public float sensX;
    //public float sensY;
    public float xRotMin;
    public float xRotMax;
    public float yRotMin;
    public float yRotMax;

    public Transform orientation;
    public Transform playerClass;

    public bool isRestricted;

    float xRot;
    float yRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //May have to change this if we want the player to "pet" creatures
        //Maybe if sense creature collider, make cursor visible with hand symbol?
        //In Player Settings > Player > Default Cursor, that is how you change the cusor
        Cursor.visible = false;
    }

    private void Update()
    {
        // For future interaction scripts (probably not need?)
        //Cursor.visible = !cursorNotVisible;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * SettingsManager.sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * SettingsManager.sens;

        yRot += mouseX;
        xRot -= mouseY;

        // So player can't look too far down or up
        xRot = Mathf.Clamp(xRot, xRotMin, xRotMax);

        // If the camera is locked, clamp player's view on y axis as well
        if (isRestricted)
        {
            //Debug.Log("yMin: " + (yRot + yRotMin));
            //Debug.Log("yMax: " + (yRot + yRotMax));
            //yRotMin = yRot + yRotMin;
            //yRotMax = yRot + yRotMax;
            yRot = Mathf.Clamp(yRot, yRotMin, yRotMax);
        }

        transform.rotation = Quaternion.Euler(xRot, yRot + playerClass.eulerAngles.y, 0);
        orientation.rotation = Quaternion.Euler(0, yRot + playerClass.eulerAngles.y, 0);
    }

    // Have func that sets var on 'old' yRot, then use that in isRestricted?
    // Point saved is 'LookAt' pet?
    // NEED TO FIX, ROTATION/LOOK IS STILL OFF!
    public void lockRotation(GameObject pet)
    {
        // Get y axis coord of pet (?)
        transform.LookAt(pet.transform.position);
        //USE CURSOR AS POINTER? SINCE IN THE MIDDLE? TO DETERMINE WHICH PET THEY ARE POINTING TO?

        //yRotMin = transform.rotation.y + yRotMin;
        //yRotMax = transform.rotation.y + yRotMax;
    }
}
