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
    [SerializeField] private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        pickUpPoint = GameObject.Find("PickUpPoint").transform;
    }

    void Update()
    {
         pickUpDistance = Vector3.Distance(player.position, transform.position);

        if (pickUpDistance <= 2f)
        {
            if (Input.GetKeyDown(KeyCode.Space) && itemPickedUp == false && pickUpPoint.childCount < 1)
            {
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                this.transform.parent = pickUpPoint;
                itemPickedUp = true;
                animator.SetBool("itemPicked", true);

                // Check the type of item and call the appropriate function
                if (gameObject.CompareTag("Shovel"))
                {
                    player.GetComponent<TreePlanting>().PickUpShovel();
                }
                else if (gameObject.CompareTag("Plant"))
                {
                    player.GetComponent<TreePlanting>().PickUpPlant();
                }
                else if (gameObject.CompareTag("Bucket"))
                {
                    player.GetComponent<TreePlanting>().PickUpBucket();
                }
            }
        }

        if (itemPickedUp == true)
        {
            this.transform.position = pickUpPoint.position;
        }

        if (Input.GetKeyUp(KeyCode.Space) && itemPickedUp == true)
        {
            readyToThrow = true;
            rb.AddForce(player.transform.forward * force);
            this.transform.parent = null;
            GetComponent<Rigidbody>().useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            animator.SetBool("itemPicked", false);
            itemPickedUp = false;
            readyToThrow = false;
        }
    }
}
