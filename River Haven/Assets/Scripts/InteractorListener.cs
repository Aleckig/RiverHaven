using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InteractorListener : MonoBehaviour
{
    [SerializeField] private GameObject ButtonTip;
    [SerializeField] private TMP_Text ButtonTipText;
    private KeyCode ActionKeyCode = KeyCode.E;
    private UnityEvent Actions;
    private bool collided = false;

    // Update is called once per frame
    void Update()
    {
        if (!collided) return;

        if (Input.GetKeyDown(ActionKeyCode))
        {
            Actions?.Invoke();
            Debug.Log("Interacted!");
        }
    }
    public void StartInteraction(KeyCode ActionKeyCode, UnityEvent _Actions)
    {
        Actions = _Actions;
        this.ActionKeyCode = ActionKeyCode;

        collided = true;
        DisplayButtonTip(collided);
    }

    public void EndInteraction()
    {
        ActionKeyCode = KeyCode.E;
        collided = false;
        DisplayButtonTip(collided);
    }

    private void DisplayButtonTip(bool displayValue)
    {
        ButtonTipText.text = "Press " + ActionKeyCode;
        ButtonTip.SetActive(displayValue);
    }
}
