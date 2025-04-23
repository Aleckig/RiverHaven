using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObjectToDestination : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private bool isNPC;
    [SerializeField] private GameObject npc;

    public void TeleportToTarget()
    {
        if (npc != null && destination != null)
        {
            UnityEngine.AI.NavMeshAgent agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                // Disable the NavMeshAgent if it exists
                agent.enabled = false;
            }
            npc.gameObject.SetActive(false);
            this.transform.position = destination.position;
            this.transform.rotation = destination.rotation;
            npc.gameObject.SetActive(true);
            NPCAnimator npcAnimator = npc.GetComponent<NPCAnimator>();
            if (npcAnimator != null)
            {
                npcAnimator.StopWalking();
            }
            npc.GetComponentInChildren<Animator>().SetBool("isWalking", false);
            npc.GetComponentInChildren<Animator>().SetBool("isSitting", false);
            npc.GetComponentInChildren<Animator>().Play("Breathing", 0, Random.Range(0f, 1f));
        }

        //this.transform.position = destination.position;
        //this.transform.rotation = destination.rotation;
    }
}
