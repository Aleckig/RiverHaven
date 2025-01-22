using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        // Debug.Log("npc angels - " + transform.eulerAngles);
    }
    public void StartWalking()
    {
        animator.SetBool("isWalking", true);
    }

    public void StopWalking()
    {
        animator.SetBool("isWalking", false);
    }

    public void SitOnSomething(Transform placeToSit)
    {
        navMeshAgent.ResetPath();
        navMeshAgent.enabled = false;

        animator.SetBool("isSitting", true);


        transform.rotation = placeToSit.rotation;

        StartCoroutine(SlowMovement(placeToSit.position));
    }

    private IEnumerator SlowMovement(Vector3 newPosition)
    {
        newPosition = new(newPosition.x, transform.position.y, newPosition.z);
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, newPosition, timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if (transform.position == newPosition)
            {
                navMeshAgent.enabled = true;
                yield break;
            }

            // Otherwise, continue next frame
            yield return new WaitForFixedUpdate();
        }
    }

    public void StandUpToSomewhere(Transform placeToStand)
    {
        navMeshAgent.ResetPath();

        animator.SetBool("isSitting", false);

        // transform.rotation = placeToStand.rotation;

        StartCoroutine(SlowMovement(placeToStand.position));
    }
}
