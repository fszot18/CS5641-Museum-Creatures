using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    float speed;

    public float groundDrag;

    public float forceJump;
    public float jumpCooldown;
    public float airMult;
    bool canJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerH;
    public LayerMask isGround;
    bool grounded;
    public bool isJumping;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
        isJumping = false;
        speed = moveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        //Checks if player is touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerH * 0.5f + 0.2f, isGround);
        
        MyInput();
        SpeedControl();

        //Handle player drag
        if(grounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0; // groundDrag / 2f;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Checks if player is ready to jump
        if (Input.GetKeyDown(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Checks if player is ready to sprint
        if (Input.GetKey(sprintKey) && grounded)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = moveSpeed;
        }

    }

    private void MovePlayer()
    {
        //Calculate movement direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded) {
            //If grounded, move normally at set movement speed
            rb.AddForce(moveDir.normalized * speed * 10f, ForceMode.Force);
        } else if(!grounded) {
            //If in the air, move at speed of air multiplier
            rb.AddForce(moveDir.normalized * speed * 10f * airMult, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit the player's velocity to max velocity
        if(flatVelocity.magnitude > speed) {
            Vector3 limitVelocity = flatVelocity.normalized * speed;
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
        }
    }

    private void Jump()
    {
        //Reset y velocity
        isJumping = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * forceJump, ForceMode.Impulse);
        rb.AddForce(Vector3.down * 7 * rb.mass, ForceMode.Force);
    }

    private void ResetJump()
    {
        //Since in Jump() we are forced to jump only once
        canJump = true;
        isJumping = false;
    }
}
