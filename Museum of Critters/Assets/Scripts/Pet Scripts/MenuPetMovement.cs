using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes pet follow mouse around on main menu and turn towards player after catching up
// Also calculates mouse's position in relation to its position
// MAY HAVE TO TWEAK WHEN ADDING ANIMATION AND MODELS

public class MenuPetMovement : MonoBehaviour
{
    public GameObject playerClass;  // Target position pet will turn to if not following the mouse
    public float petSpeed;          // Pet's movement speed
    public float rotateSpeed;       // Pet's rotation speed when turning to player or mouse

    Vector3 currentPos; // The pet's current position in the scene (looking mainly at x axis)
    Vector3 targetPos;  // The mouse's current position after calculated into world point values
    Vector3 theMouse;   // The mouse's current position (related to scene) (do not use this for targeting)

    private Quaternion lookRot; // Direction of where to look based on which side cursor is in relation to pet
    private Vector3 dir;        // Direction (x axis) pet has to go to catch up to mouse

    void Start()
    {
        // Initialize pet's current position
        currentPos = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Get pet's current position and mouse's current position
        // Update pet's current position so calculations stay accurate
        // Get mouse's position in relation to screen values (need to calc to world points)
        currentPos = transform.position;
        theMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        //Debug.Log(cameraObj.ScreenToWorldPoint(theMouse));
        
        // Set target position (world points of mouse) and pet's speed towards mouse
        targetPos = Camera.main.ScreenToWorldPoint(theMouse);
        targetPos = new Vector3(targetPos.x * 65.0f, currentPos.y, currentPos.z);
        var step = petSpeed * Time.deltaTime;

        // Make pet move towards target position
        transform.position = Vector3.MoveTowards(currentPos, targetPos, step);

        // If the pet has caught up to the mouse, turn towards player
        if (Vector3.Distance(currentPos, targetPos) < 0.5f)
        {
            //Debug.Log("I caught up!");

            // Make pet stop, calculate direction of player, turn towards them at roatation speed
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            dir = (playerClass.transform.position - currentPos).normalized;
            lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
        }
        else
        {
            // Mouse has moved! Pet chases mouse position again (target position)
            // Calculates direction to rotate towards at rotation speed
            dir = (targetPos - currentPos).normalized;
            lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
        }
    }
}
