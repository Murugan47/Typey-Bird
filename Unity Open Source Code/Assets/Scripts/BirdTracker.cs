using UnityEngine;

public class BirdTracker : MonoBehaviour
{
    
    public GameObject player;
    public PipeTracker pipeTracker;
    public float gravity = 2000f;
    public float flapForce = 550;

    public float velocityChecker = -350;
    private Vector3 velocity;
    private Quaternion targetRotation;
    public bool canMove = true;

    void Start()
    {
        StartingFlap();
    }

    void Update()
    {
        if (!canMove) return; // If the game is paused, stop the bird from moving

        velocity.y -= gravity * Time.deltaTime;
        player.transform.position += velocity * Time.deltaTime;

        // Flap logic (bird flap action when space or mouse button is pressed)
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = flapForce;
            pipeTracker.FlapAudio();
        }

        player.transform.position += velocity * Time.deltaTime;

        // Rotate when going up or down
        if (velocity.y > 0)
        {
            // When going up, rotate upwards (bird flaps upwards)
            float angle = Mathf.Lerp(0f, 45f, velocity.y / 10f); // Adjust speed of upward tilt
            targetRotation = Quaternion.Euler(0f, 0f, angle);
        }
        else if (velocity.y < velocityChecker)
        {
            // When falling, rotate downwards (bird falls)
            float angle = Mathf.Lerp(0f, -45f, Mathf.Abs(velocity.y) / 10f); // Adjust speed of downward tilt
            targetRotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Smoothly rotate toward the target rotation
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    public void StartingFlap()
    {
        velocity.y = flapForce;
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }

    // Method to allow the bird to move again
    public void ResumeMovement()
    {
        canMove = true;
    }

    // Method to stop the bird from moving
    public void PauseMovement()
    {
        canMove = false;
    }
}