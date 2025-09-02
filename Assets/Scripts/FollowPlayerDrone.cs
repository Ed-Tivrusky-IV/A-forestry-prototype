using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class FollowPlayerDrone : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    private PlayerController playerController; // Reference to the PlayerController script
    private GameObject menu;
    private MenuNavigation menuNavigation; // Reference to the MenuNavigation script
    private Animator[] fanAnimator; // Reference to the Animator for the fan animation

    private Vector3 offset = new(0, 1.33f, 0); // Offset from the player position 

    private bool isTakingOff = false; // Flag to check if the drone is taking off
    private bool isAscending = true; // Flag to check if the drone is ascending

    private float takeOffHeight = 4f; // Height value to take off
    private float takeOffSpeed = 2f; // Speed of the take-off
    private float pauseDuration = 3f; // Duration to pause at the top
    private Vector3 rotationSpeed = new(0, 120f, 0);
    private float rotationAngle = 360f; // Current rotation angle of the drone
    private float rotatedAngle = 0f; // Total rotated angle

    public Material mat;
    private Vector4[] circles = new Vector4[50];
    private int circleCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player"); // Find the player by tag
        playerController = player.GetComponent<PlayerController>();

        menu = GameObject.FindWithTag("Menu");
        menuNavigation = menu.GetComponent<MenuNavigation>();

        fanAnimator = GetComponentsInChildren<Animator>(); // Get all Animator components in children

        //For Debugging
        //UpdateMinimap();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTakingOff)
        {
            TakeOff();
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isTakingOff = true; // Set the flag to true when space is pressed
            ChangeFanState(); // Change the fan state animation
            playerController.StopPlayer(); // Stop the player when taking off
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            menuNavigation.EnableMenu(); // Enable the menu when taking off
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuNavigation.DisableMenu(); // Disable the menu when taking off
        }
    }

    void TakeOff()
    {
        // Logic for taking off, if needed
        float originalY = player.transform.position.y + offset.y;
        if (isAscending)
        {
            // Ascend to the take-off height
            if (transform.position.y < originalY + takeOffHeight)
            {
                transform.Translate(Vector3.up * takeOffSpeed * Time.deltaTime);
            }
            else if(rotatedAngle < rotationAngle)
            {
                // Pause at the top for a while
                //StartCoroutine(PauseAtTop());
                rotatedAngle += rotationSpeed.y * Time.deltaTime;
                transform.Rotate(rotationSpeed * Time.deltaTime); // Rotate the drone
            }
            else
            {
                rotatedAngle = 0f; // Reset the rotated angle
                isAscending = false; // Stop ascending after reaching the height
            }
        }
        else
        {
            if (transform.position.y > originalY)
            {
                // Descend back to the player's height
                transform.Translate(Vector3.down * takeOffSpeed * Time.deltaTime);
            }
            else
            {
                // Reset the drone's position and stop taking off
                isTakingOff = false;
                isAscending = true;
                ChangeFanState(); // Change the fan state animation back
                transform.position = player.transform.position + offset; // Reset position
                playerController.StartPlayer(); // Restart the player movement
                UpdateMinimap(); // Update the minimap with the drone's position
                //menuNavigation.EnableMenu(); // Enable the menu after landing
            }
        }

        //playerController.StartPlayer(); // Start the player again after taking off
    }

    private void ChangeFanState()
    {
        // Change the state of the fan animation
        foreach (var animator in fanAnimator)
        {
            if (animator != null)
            {
                animator.SetBool("IsTakingOff", isTakingOff);
            }
        }
    }

    private void UpdateMinimap()
    {
        if (circleCount < circles.Length)
        {
            // Example: put circle at random UV (replace with real position logic)
            float playerXParam = (player.transform.position.x + 40) / 80;
            float playerZParam = (player.transform.position.z + 40) / 80;
            Debug.Log($"Player Position: {player.transform.position}, UV: ({playerXParam}, {playerZParam})");
            Vector2 uv = new(playerXParam, playerZParam);
            //Vector2 uv = new(0f, 0f);
            circles[circleCount] = new Vector4(uv.x, uv.y, 0.2f, 0);
            circleCount++;

            mat.SetInt("_CircleCount", circleCount);
            mat.SetVectorArray("_CircleData", circles);
        }
    }

    private IEnumerator PauseAtTop()
    {
        yield return new WaitForSeconds(pauseDuration); // Wait for the specified duration
        isAscending = false; // Stop ascending after reaching the height
        //UpdateMinimap(); // Update the minimap with the drone's position
    }
}
