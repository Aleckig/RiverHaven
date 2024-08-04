using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPlayerController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 5f; // Normal movement speed of the player
    [SerializeField] private float sprintSpeed = 8f; // Sprinting speed of the player
    [SerializeField] private Animator animator; // Animator component for controlling player animations

    private PlayerControls playerControls; // PlayerControls object for handling player input
    private Rigidbody rb; // Rigidbody component for controlling player movement
    private Vector3 movement; // Direction of player movement
    private float currentSpeed; // Current speed of the player
    private float rotationSpeed = 720f; // Speed at which the player rotates (degrees per second)

    private const string SPEED_PARAM = "Speed"; // Animator parameter for controlling speed-based animations

    private void Awake()
    {
        // Initialize PlayerControls
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Enable PlayerControls input actions
        playerControls.Enable();
    }

    private void OnDisable()
    {
        // Disable PlayerControls input actions
        playerControls.Disable();
    }

    private void Start()
    {
        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();
        // Set initial speed to normal speed
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        // Read player input for movement
        Vector2 input = playerControls.Player.Move.ReadValue<Vector2>();

        // Calculate movement vector
        movement = new Vector3(input.x, 0, input.y).normalized;

        // Determine current speed based on movement vector
        float speed = movement.magnitude * currentSpeed;

        // Set animator speed parameter
        animator.SetFloat(SPEED_PARAM, speed);

        // Adjust speed based on sprinting
        currentSpeed = playerControls.Player.Sprint.IsPressed() ? sprintSpeed : normalSpeed;
    }

    private void FixedUpdate()
    {
        // Move the player based on movement input
        MovePlayer();

        // Rotate the character to face the movement direction
        RotateCharacter();
    }

    private void MovePlayer()
    {
        // Move the player based on movement input
        Vector3 targetPosition = rb.position + movement * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    private void RotateCharacter()
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            float step = rotationSpeed * Time.fixedDeltaTime; // Calculate rotation step size
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, step);

            // Debugging logs
            // Debug.Log("Movement Vector: " + movement);
            // Debug.Log("Current Rotation: " + rb.rotation.eulerAngles);
            // Debug.Log("Target Rotation: " + targetRotation.eulerAngles);
        }
    }
}
