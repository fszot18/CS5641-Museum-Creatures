using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform playerClass;

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
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

        yRot += mouseX;
        xRot -= mouseY;

        //So player can't look too far down or up
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRot, yRot + playerClass.eulerAngles.y, 0);
        orientation.rotation = Quaternion.Euler(0, yRot + playerClass.eulerAngles.y, 0);
    }
}
