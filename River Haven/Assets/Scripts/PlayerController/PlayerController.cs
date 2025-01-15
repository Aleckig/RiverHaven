using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private Animator animator;
    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private float currentSpeed;
    private float rotationSpeed = 720f;
    private bool isPaused = false;
    private const string SPEED_PARAM = "Speed";
    private bool animationToggle;
    private bool movementToggle;
    public bool holdingObject;

    private float targetNormalizedTime = 0f;
    private float transitionSpeed = 1f;

    private float currentNormalizedTime = 0f;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        if (holdingObject == true && movement == Vector3.zero)
        {
            Invoke("SetAnimationSpeedToZero", 0.05f);
            animationToggle = true;
        }
        else if (holdingObject == true && movement != Vector3.zero && animationToggle == true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime > 0)
            {
                targetNormalizedTime = 0f;
                currentNormalizedTime = Mathf.MoveTowards(currentNormalizedTime, targetNormalizedTime, transitionSpeed * Time.deltaTime);
                animator.Play("WalkingWithBox", 0, currentNormalizedTime);
            }
            else
            {
                animator.speed = 1;
            }
            animationToggle = false;

        }
        else
        {
            animator.speed = 1;
        }
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            MovePlayer();
            RotateCharacter();
        }
    }

    private void MovePlayer()
    {
        Vector2 input = playerControls.Player.Move.ReadValue<Vector2>();
        movement = new Vector3(input.x, 0, input.y).normalized;
        float speed = movement.magnitude * currentSpeed;
        animator.SetFloat(SPEED_PARAM, speed);
        currentSpeed = playerControls.Player.Sprint.IsPressed() ? sprintSpeed : normalSpeed;
        Vector3 targetPosition = rb.position + movement * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    private void RotateCharacter()
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            float step = rotationSpeed * Time.fixedDeltaTime;
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, step);
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // Pause the game
        PauseMenu.Open(); // Opens the pause menu
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Resume the game
        MenuManager.Instance.CloseMenu(); // Close the pause menu using MenuManager
    }

    void SetAnimationSpeedToZero()
    {
        animator.speed = 0f;
    }
}