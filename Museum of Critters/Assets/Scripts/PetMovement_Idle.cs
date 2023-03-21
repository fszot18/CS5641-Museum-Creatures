using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement_Idle : MonoBehaviour
{
    Rigidbody rb;

    public float speed;
    public float minWait;
    public float maxWait;
    public float minPause;
    public float maxPause;
    public float rotateSpeed;

    float timer;
    float randWait;
    float randPause;
    float timer2;

    bool shouldMove;

    //Something here for animation connection in the future

    //FOR NOW IT SEEMS THIS WORKS, BUT IT IS VERY JANKY AND I SHOULD MAKE AN AI VERSION (BETTER VERSION)

    // Start is called before the first frame update
    void Start()
    {
        //transform.Rotate(new Vector3(0.0f, Random.Range(-45.0f, 45.0f), 0.0f), Space.Self);
        rb = GetComponent<Rigidbody>();
        transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f), Space.World);
        timer = 0;
        timer2 = 0;
        randWait = Random.Range(minWait, maxWait);
        randPause = Random.Range(minPause, maxPause);
        shouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
       
        if (timer <= (randWait - randPause) && shouldMove) {
            rb.velocity = transform.forward * (speed / 2);
        } else if (timer > randWait && shouldMove) {
            rb.velocity = Vector3.zero;
            MoveRand();
            timer = 0;
        }

        Repositioning();
    }

    private void MoveRand()
    {
        // Added 1.5f into the rotation so it's rotation is more noticable
        // Need to add a smoother rotation next
        transform.Rotate(new Vector3(0.0f, Random.Range(-45.0f, 45.0f), 0.0f) * 1.5f, Space.Self);
        randWait = Random.Range(minWait, maxWait);
        randPause = Random.Range(minPause, maxPause);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Pen Border")) {
            rb.velocity = -1.0f * transform.forward * speed;
            shouldMove = false;
            transform.Rotate(new Vector3(0.0f, Random.Range(-90.0f, 90.0f) * 1.5f, 0.0f), Space.Self);
        }
    }

    private void Repositioning()
    {
        if (!shouldMove)
        {
            timer2 += Time.deltaTime;
        }

        if (timer2 > 3)
        {
            shouldMove = true;
            timer2 = 0;
        }
    }
}
