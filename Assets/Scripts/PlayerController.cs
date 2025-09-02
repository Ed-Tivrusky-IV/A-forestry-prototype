using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;
    public float turnSpeed = 45f;
    private float verticalInput;
    private float horizontalInput;

    private readonly float xRange = 50f;
    private readonly float zRange = 50f;

    private bool shouldStop = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    // If you wanna move the player, use Transform.Translate
    // If you wanna use Physics, use Rigidbody and AddForce
    void Update()
    {
        if (!shouldStop)
        {
            MovePlayer();
        }

        KeepPlayerInBounds();
    }

    // Moves the player using arrow keys
    void MovePlayer()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        transform.Translate(speed * Time.deltaTime * verticalInput * Vector3.forward);
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
    }

    void KeepPlayerInBounds()
    {
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }
        else if (transform.position.z < -zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zRange);
        }
    }

    public void StopPlayer()
    {
        shouldStop = true;
    }

    public void StartPlayer()
    {
        shouldStop = false;
    }
}
