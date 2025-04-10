using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class ActivityTracker : MonoBehaviour
{
    [SerializeField] private GameObject arrowRotation;
    [SerializeField] private GameObject playerObject;
    private bool messageShown = false;
    private int activitiesCompleted = 0;
    [SerializeField] private ScreenFade screenFade;
    private bool hasTeleported = false;
    [SerializeField] private GameObject layla;
    [SerializeField] private GameObject laylaPosition;

    //void Update()
    //{
    //    var activitiesCompletedVar = DialogueLua.GetVariable("ActivitiesCompleted");
    //    int activitiesCompleted = activitiesCompletedVar.asInt;
    //    if (activitiesCompleted >= 6 && messageShown == false)
    //    {
    //        DialogueManager.ShowAlert("Congratulations! You have completed all the activity board tasks. Go talk to Marcus at the NGO building.");
    //        if (arrowRotation != null) {
    //            arrowRotation.GetComponent<ArrowRotation>().MakeNewTarget4();
    //            arrowRotation.gameObject.SetActive(true);
    //        }
    //        messageShown = true;
    //    }
    //    if (playerObject != null && playerObject.GetComponent<IndoorTracker>().isInNGO && activitiesCompleted >= 6)
    //    {
    //        arrowRotation.gameObject.SetActive(false);
    //        this.gameObject.SetActive(false);
    //    }
    //}

    void Start()
    {
        InvokeRepeating("TriggerAction", 0f, 5f);
    }

    void TriggerAction()
    {
        var activitiesCompletedVar = DialogueLua.GetVariable("ActivitiesCompleted");
        activitiesCompleted = activitiesCompletedVar.asInt;
        if (activitiesCompleted >= 3 && hasTeleported == false && screenFade != null)
        {
            DialogueManager.ShowAlert("Layla has urgent news. Go talk to her.");
            screenFade.TriggerFadeAndTeleportToNGO();
            UnityEngine.AI.NavMeshAgent agent = layla.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                // Disable the NavMeshAgent if it exists
                agent.enabled = false;
            }
            layla.gameObject.SetActive(false);
            layla.gameObject.transform.position = laylaPosition.gameObject.transform.position;
            layla.gameObject.SetActive(true);
            hasTeleported = true;
        }
        if (activitiesCompleted >= 6 && messageShown == false)
        {
            DialogueManager.ShowAlert("Congratulations! You have completed all the activity board tasks. Go talk to Marcus at the NGO building.");
            if (arrowRotation != null)
            {
                arrowRotation.gameObject.SetActive(true);
                arrowRotation.GetComponent<ArrowRotation>().MakeNewTarget5();
            }
            messageShown = true;
        }
        if (playerObject != null && activitiesCompleted >= 6 && playerObject.GetComponent<IndoorTracker>().isInNGO)
        {
            arrowRotation.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        // Stop invoking when the object is disabled to free up resources
        CancelInvoke("TriggerAction");
    }
}
