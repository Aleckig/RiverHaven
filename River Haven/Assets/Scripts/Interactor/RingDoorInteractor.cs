using UnityEngine;

public class RingDoorInteractor : MonoBehaviour
{
    [SerializeField] private NPCAtHome NPCAtHome;
    [SerializeField] private GameObject InteractUIBlock;

    private void OnTriggerEnter(Collider other)
    {
        InteractUIBlock.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        InteractUIBlock.SetActive(false);
    }
}
