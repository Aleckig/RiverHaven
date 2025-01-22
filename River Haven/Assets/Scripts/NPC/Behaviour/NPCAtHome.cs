using UnityEngine;

public class NPCAtHome : MonoBehaviour
{
    [SerializeField] private bool isBusy = false;
    [SerializeField] private float feedbackDelay = 1f; //Delay between knock and reaction 
    [SerializeField] private Transform doorDestination;
    //
    private NPCAnimator animator;

    public void KnockAtDoor()
    {

    }
}
