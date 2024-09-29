using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Interactor : MonoBehaviour
{
    [SerializeField] private InteractorListener interactorListener;
    [SerializeField] private string PlayerTag = "Player";
    [SerializeField] private KeyCode ActionKeyCode = KeyCode.E;
    [SerializeField] private UnityEvent Actions;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PlayerTag)
        {
            interactorListener.StartInteraction(ActionKeyCode, Actions);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == PlayerTag)
        {
            interactorListener.EndInteraction();
        }
    }
}
