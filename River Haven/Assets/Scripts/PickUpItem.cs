using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private Transform pickUpPoint;
    private Transform player;
    public float pickUpDistance;
    public float force = 30f;
    public bool readyToThrow;
    public bool itemPickedUp;
    private Rigidbody rb;
    [SerializeField] private Animator animator; // Ensure Animator is assigned either in the Inspector or through code

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Initialize Rigidbody
        player = GameObject.Find("Player")?.transform; // Find the player object
        pickUpPoint = GameObject.Find("PickUpPoint")?.transform; // Find the pick up point object

        if (player == null)
        {
            Debug.LogError("Player object not found. Make sure there is a GameObject named 'Player' in the scene.");
        }

        if (pickUpPoint == null)
        {
            Debug.LogError("PickUpPoint object not found. Make sure there is a GameObject named 'PickUpPoint' in the scene.");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found. Make sure this script is attached to a GameObject with a Rigidbody component.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned. Assign it in the Inspector or use GetComponent<Animator>() if on the same GameObject.");
        }
    }

    void Update()
    {
        if (player == null || pickUpPoint == null || rb == null || animator == null)
        {
            return; // Exit Update if critical components are missing
        }

        pickUpDistance = Vector3.Distance(player.position, transform.position);

        if (pickUpDistance <= 2f)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !itemPickedUp && pickUpPoint.childCount < 1)
            {
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                this.transform.parent = pickUpPoint; // Set the parent to pickUpPoint
                itemPickedUp = true;
                animator.SetBool("itemPicked", true); // Trigger animation
            }
        }

        if (itemPickedUp)
        {
            this.transform.position = pickUpPoint.position; // Move item to pickUpPoint position
        }

        if (Input.GetKeyUp(KeyCode.Space) && itemPickedUp)
        {
            readyToThrow = true;
            rb.AddForce(player.transform.forward * force); // Add force to the Rigidbody
            this.transform.parent = null; // Unparent the item
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            animator.SetBool("itemPicked", false); // Stop animation
            itemPickedUp = false;
            readyToThrow = false;
        }
    }
}
