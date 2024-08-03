using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPlayerController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 5f; // Normal movement speed of the player
    [SerializeField] private float sprintSpeed = 10f; // Sprinting speed of the player
    [SerializeField] private Animator animator; // Animator component for controlling player animations

    // PlayerControls object for handling player input
    private PlayerControls playerControls;
    private Rigidbody rb; // Rigidbody component for controlling player movement
    private Vector3 movement; // Direction of player movement
    private float currentSpeed; // Current speed of the player
    private float rotationSpeed = 10f; // Speed at which the player rotates

    private const string IS_WALK_PARAM = "IsWalk"; // Animator parameter for controlling walk animation

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
        rb = gameObject.GetComponent<Rigidbody>();
        // Set initial speed to normal speed
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        // Read player input for movement
        Vector2 input = playerControls.Player.Move.ReadValue<Vector2>();

        // Normalize movement vector
        movement = new Vector3(input.x, 0, input.y).normalized;

        // Set walk animation parameter
        animator.SetBool(IS_WALK_PARAM, movement != Vector3.zero);

        // Check if Shift key is held down to adjust speed
        if (playerControls.Player.Sprint.IsPressed())
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }
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
        // Rotate the character to face the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
