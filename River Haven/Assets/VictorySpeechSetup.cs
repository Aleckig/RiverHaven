using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictorySpeechSetup : MonoBehaviour
{
    [SerializeField] private Transform[] characterPositions;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Transform[] characterPositionsAtPlantingSite;

    public void TeleportCharacters()
    {
        StartCoroutine(TeleportCharactersInvoke());
    }

    public void TeleportCharactersToPlantingSite()
    {
        characterPositions = characterPositionsAtPlantingSite;
        StartCoroutine(TeleportCharactersInvoke());
    }

    private IEnumerator TeleportCharactersInvoke()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < characters.Length; i++)
        {
            // Get the NPC GameObject
            GameObject npc = characters[i];

            // Get the target Transform position
            Transform targetPosition = characterPositions[i];

            // Teleport the NPC to the target position
            if (npc != null && targetPosition != null)
            {
                UnityEngine.AI.NavMeshAgent agent = npc.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (agent != null)
                {
                    // Disable the NavMeshAgent if it exists
                    agent.enabled = false;
                }
                npc.gameObject.SetActive(false);
                npc.transform.position = targetPosition.position;
                npc.transform.rotation = targetPosition.rotation;
                npc.gameObject.SetActive(true);
                NPCAnimator npcAnimator = npc.GetComponent<NPCAnimator>();
                if (npcAnimator != null)
                {
                    npcAnimator.StopWalking();
                }
                npc.GetComponentInChildren<Animator>().SetBool("isWalking", false);
                npc.GetComponentInChildren<Animator>().SetBool("isSitting", false);
                npc.GetComponentInChildren<Animator>().Play("Breathing", 0, Random.Range(0f,1f));
            }
        }
    }
}
