using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform transformPlayer; // Ensure this is assigned in the Inspector
    private bool stickToPlayer;
    private Transform playerBallPosition;
    float speed;
    Vector3 previousLocation;
    Player scriptPlayer;

    public bool StickToPlayer { get => stickToPlayer; set => stickToPlayer = value; }

    void Start()
    {
        // Ensure transformPlayer is assigned
        if (transformPlayer == null)
        {
            Debug.LogError("transformPlayer is not assigned.");
            return;
        }

        // Find the "Geometry" child
        Transform geometryTransform = transformPlayer.Find("Geometry");
        if (geometryTransform == null)
        {
            Debug.LogError("Geometry child not found under transformPlayer.");
            return;
        }

        // Find the "BallLocation" child under "Geometry"
        playerBallPosition = geometryTransform.Find("BallLocation");
        if (playerBallPosition == null)
        {
            Debug.LogError("BallLocation child not found under Geometry.");
            return;
        }

        // Get the Player script from transformPlayer
        scriptPlayer = transformPlayer.GetComponent<Player>();
        if (scriptPlayer == null)
        {
            Debug.LogError("Player script not found on transformPlayer.");
            return;
        }

        previousLocation = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Update()
    {
        if (!stickToPlayer)
        {
            // Check the distance between the ball and the player
            float distanceToPlayer = Vector3.Distance(transformPlayer.position, transform.position);
            if (distanceToPlayer < 0.5f) // Attach the ball to the player
            {
                stickToPlayer = true;
                scriptPlayer.BallAttachedToPlayer = this;
            }
        }
        else
        {
            // Update the ball's position to match the player's BallLocation position
            if (playerBallPosition != null)
            {
                Vector2 currentLocation = new Vector2(transform.position.x, transform.position.z);
                speed = Vector2.Distance(currentLocation, previousLocation) / Time.deltaTime;
                transform.position = playerBallPosition.position;
                transform.Rotate(new Vector3(transformPlayer.right.x, 0, transformPlayer.right.z), speed, Space.World);
                previousLocation = currentLocation;
            }
            else
            {
                Debug.LogError("playerBallPosition is null, but stickToPlayer is true.");
            }
        }

        // Respawn the ball if it falls below a certain height
        if (transform.position.y < -2)
        {
            transform.position = new Vector3(Random.value * 34 - 8, -1.5f, Random.value * 63 - 19);
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
