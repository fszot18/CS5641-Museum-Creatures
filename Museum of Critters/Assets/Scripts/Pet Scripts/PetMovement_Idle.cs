using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to the pet object itself so that it can randomly roam
// Only controls the 'idle' movement of the pet
// Works in tandem with the pet's "PetVision" script so it can maneuver around the environment

public class PetMovement_Idle : MonoBehaviour
{
    Rigidbody rb;

    public float speed;         // Controls how fast pet moves forward
    public float minWait;       // Minimum waiting time for pet movement
    public float maxWait;       // Maximum waiting time for pet movement
    public float minPause;      // Minimum waiting time for pet pauses (for turning)
    public float maxPause;      // Maximum waiting time for pet pauses (for turning)
    public float minTurn;       // Minimum time for pet to turn
    public float maxTurn;       // Maximum time for pet to turn
    public float faceForward;   // Angle pet should turn and face to at the beginning of the pet simulator
    public float rotateSpeed;   // Controls how fast pet will turn

    float moveTimer;            // Keeps track of when pet should do random pauses and turns during walking
    float rotateTimer;          // Keeps track of how long pet should turn before walking forward again
    float randWait;             // Holds random value for how long pet should move forward
    float randPause;            // Holds random value for how long pet should pause between moving and turning
    float randTurn;             // Holds random value for how long pet should turn

    public bool shouldMove;     // Bool that determines whether pet should move in this instant
    public bool shouldRotate;   // Bool that determines whether pet should rotate in this instant

    int[] rotateDir;            // Holds -1 and 1 for random rotation direction
    int randDir;                // Holds random value for which direction pet should turn
    int randIndex;              // Holds random index from randDir for random rotation direction

    // Something here for animation connection in the future
    // FOR NOW IT SEEMS THIS WORKS, BUT IT IS VERY JANKY AND I SHOULD MAKE AN AI VERSION (BETTER VERSION)

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();                                         // Fetches this pet's rigidbody gameobject
        transform.Rotate(new Vector3(0.0f, faceForward, 0.0f), Space.World);    // Makes pet start facing at certain direction
        moveTimer = 0;
        rotateTimer = 0;
        randWait = Random.Range(minWait, maxWait);      // Randomly set how much time to move forward
        randPause = Random.Range(minPause, maxPause);   // Randomly set how much time to pause after movement
        randTurn = Random.Range(minTurn, maxTurn);      // Randomly set how much time to turn after pausing

        shouldMove = true;
        shouldRotate = false;

        rotateDir = new int[] {-1, 1};
        randIndex = Random.Range(0, rotateDir.Length);
        randDir = rotateDir[randIndex];
    }

    // Update is called once per frame
    void Update()
    {
        // This section focuses on how long and if the pet is moving forwards
        if (shouldMove)
        {
            // May have to tweak this still (could be out of the if statement?)
            moveTimer += Time.deltaTime;
        }
       
        if (moveTimer <= (randWait - randPause) && shouldMove)
        {
            // Pet moves forward for a random period if it is able to move
            // (randWait - randPause) makes it so there is a small period where the pet isn't moving
            rb.velocity = transform.forward * (speed / 2);
        }
        else if (moveTimer > randWait && shouldMove) 
        {
            // If the timer is above random set of time that pet should move, stop and turn at random angle
            // This is done by setting shouldRotate to true
            shouldRotate = true;
        }

        // This section focuses on how long and if the pet is turning
        if (shouldRotate && shouldMove)
        {
            rotateTimer += Time.deltaTime;
        }

        if (rotateTimer <= randTurn && shouldRotate && shouldMove)
        {
            // Pet turns for a random period if it is able to move
            transform.Rotate(0.0f, Time.deltaTime * ((float)randDir * rotateSpeed), 0.0f, Space.Self);
        }
        else if (rotateTimer > randTurn && shouldRotate && shouldMove)
        {
            // Pet should stop turning and switch back to moving forward
            randIndex = Random.Range(0, rotateDir.Length);
            randDir = rotateDir[randIndex];

            // Reset moveTimer and rotateTimer back to 0 so that it moves and rotates for a different amount of random time
            shouldRotate = false;
            //shouldMove = true;
            moveTimer = 0;
            rotateTimer = 0;

            // Choose different random values for next forward motion and turn
            randWait = Random.Range(minWait, maxWait);
            randPause = Random.Range(minPause, maxPause);
            randTurn = Random.Range(minTurn, maxTurn);
        }
    }
}
