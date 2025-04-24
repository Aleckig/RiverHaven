using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class CarBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip honkingSound;
    [SerializeField] private float honkVolume = 0.5f;
    [SerializeField] private List<GameObject> npcList = new List<GameObject>();
    [SerializeField] private List<GameObject> carVariants = new List<GameObject>();
    [SerializeField] private List<Transform> followPoints = new List<Transform>();
    [SerializeField] private Transform npcSpawnPoint;
    //
    private NavMeshAgent navMeshAgent;
    //
    private int stopPointID = 0;
    public int SetStopPointID { set => stopPointID = ((value >= 1) && (value <= 5)) ? value : 0; }
    //
    private bool pauseMovement = false; // For general purpose of stop, ak player or other object
    private bool npcOut = false;  // For checking where is npc went to the shop or not

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        // Set the car to a random variant
        int randomIndex = Random.Range(0, carVariants.Count);
        for (int i = 0; i < carVariants.Count; i++)
        {
            if (i == randomIndex)
            {
                carVariants[i].SetActive(true);
            }
            else
            {
                carVariants[i].SetActive(false);
            }
        }
        FollowPathWithTp();
    }
    public void PauseMovement()
    {
        pauseMovement = true;
        navMeshAgent.ResetPath();
        navMeshAgent.velocity /= 2;
    }
    public void ContinueMovement() => pauseMovement = npcOut; // Continue movement only if the NPC isn't out of the car
    public void NPCGetInCar()
    {
        stopPointID = 0;
        npcOut = false;
        ContinueMovement();
    }

    public void PlayHonkSound()
    {
        if (audioSource != null && honkingSound != null && !npcOut)
        {
            audioSource.PlayOneShot(honkingSound, honkVolume);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StopPoint"))
        {
            int.TryParse(other.gameObject.name, out int parsedStopPointID);
            if (parsedStopPointID == stopPointID)
            {
                PauseMovement();
                if (!npcOut)  // If the NPC is already out, do not spawn another one
                {
                    GameObject npc = npcList[Random.Range(0, npcList.Count)];
                    npc.GetComponent<NPCFromCar>().StartMovement(parsedStopPointID, npcSpawnPoint, this.gameObject);
                }
                npcOut = true;
            }
        }

        // Check if the waypoint is a stop point and if the ID matches

    }

    private void FollowPathWithTp()
    {
        StartCoroutine(MoveTo(followPoints));
    }

    private IEnumerator MoveTo(List<Transform> waypoints, bool tpStatus = true)
    {
        Transform objectToMove = this.gameObject.transform;

        if (tpStatus)
        {
            navMeshAgent.enabled = false;
            objectToMove.position = waypoints[0].position;
            navMeshAgent.enabled = true;
        }
        else yield return new WaitForSeconds(.5f);

        foreach (Transform waypointObj in waypoints)
        {
            if (waypointObj == null)
                yield break;
            // Start moving
            while (true)
            {
                //Pause movement
                while (pauseMovement)
                {
                    yield return new WaitForSeconds(1f);
                }

                // Start animation for spining wheels
                // SpinWheelsAnimation();
                // Move towards the current waypoint
                navMeshAgent.SetDestination(waypointObj.position);

                // Check if the NPC has reached the waypoint
                Vector3 npcPos = new(transform.position.x, 0, transform.position.z);
                Vector3 waypointPos = new(waypointObj.position.x, 0, waypointObj.position.z);
                float distance = (npcPos - waypointPos).magnitude;

                if (distance <= .3f)
                    break;

                yield return new WaitForFixedUpdate();
                // Leave the routine and return here in the next frame
            }
        }

        navMeshAgent.ResetPath();
        gameObject.SetActive(false);
        yield break;
    }
}
