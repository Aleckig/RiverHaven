using UnityEngine;
using UnityEngine.UI;

public class TreePlanting : MonoBehaviour
{
    [SerializeField] private GameObject questMarker;
    [SerializeField] private GameObject dirt;
    [SerializeField] private GameObject smallTree;
    [SerializeField] private GameObject finalTree;
    
    [SerializeField] private ParticleSystem dirtParticleEffect;
    [SerializeField] private ParticleSystem plantingParticleEffect;
    [SerializeField] private ParticleSystem waterParticleEffect;
    [SerializeField] private ParticleSystem completedParticleEffect;
    
    [SerializeField] private Image ProgressBar;
    [SerializeField] private GameObject progressBarContainer;

    private bool playerInArea = false;
    private bool shovelInArea = false;
    private bool plantInArea = false;
    private bool bucketInArea = false;

    // Action counter and constant to determine when an action is complete
    private int actionCount = 0;
    private const int requiredActions = 10;

    // Enum to define the different stages of the tree planting process
    private enum PlantingStage
    {
        NotStarted,
        Digging,
        Planting,
        Watering,
        Completed
    }

    // Current stage of the planting process, starting at NotStarted
    private PlantingStage currentStage = PlantingStage.NotStarted;

    void Start()
    {
        // Initialize by hiding dirt, small tree, final tree, and progress bar
        dirt.SetActive(false);
        smallTree.SetActive(false);
        finalTree.SetActive(false);
        SetProgressBarVisibility(false);
    }

    void Update()
    {
        // Check if the player presses the action key ("E") and perform an action if they do
        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformAction();
        }
    }

    // Updates the progress bar fill based on the current progress
    public void UpdateProgressBar(float progress)
    {
        ProgressBar.fillAmount = progress;
    }

    // Sets the visibility of the progress bar container
    private void SetProgressBarVisibility(bool isVisible)
    {
        progressBarContainer.gameObject.SetActive(isVisible);
    }

    // Handles the logic for performing an action based on the current stage of the process
    private void PerformAction()
    {
        // Only allow actions if the player is in the area
        if (!playerInArea) return;

        // Perform different actions based on the current stage
        switch (currentStage)
        {
            case PlantingStage.NotStarted:
                if (shovelInArea) StartDigging(); // Start digging if shovel is in the area
                break;
            case PlantingStage.Digging:
                if (shovelInArea) ContinueDigging(); // Continue digging if shovel is in the area
                break;
            case PlantingStage.Planting:
                if (plantInArea) ContinuePlanting(); // Continue planting if plant is in the area
                break;
            case PlantingStage.Watering:
                if (bucketInArea) ContinueWatering(); // Continue watering if bucket is in the area
                break;
            case PlantingStage.Completed:
                SetProgressBarVisibility(false); // Hide progress bar if the process is completed
                break;
        }
    }

    // Initiates the digging process
    private void StartDigging()
    {
        currentStage = PlantingStage.Digging; // Set the stage to Digging
        actionCount = 0; // Reset action counter
        SetProgressBarVisibility(true); // Show progress bar
        UpdateProgressBar(0f); // Initialize progress bar to 0
        ContinueDigging(); // Start the digging action
    }

    // Handles the digging process and progress
    private void ContinueDigging()
    {
        actionCount++; // Increment action count

        // Play the dirt particle effect at the quest marker position
        if (dirtParticleEffect != null)
        {
            dirtParticleEffect.transform.position = questMarker.transform.position;
            dirtParticleEffect.Play();
        }

        // Update the progress bar based on the action count
        UpdateProgressBar((float)actionCount / requiredActions);

        // Check if required actions are completed
        if (actionCount >= requiredActions)
        {
            questMarker.SetActive(false); // Hide the quest marker
            dirt.SetActive(true); // Show the dirt object
            actionCount = 0; // Reset action counter
            currentStage = PlantingStage.Planting; // Move to the next stage: Planting
            SetProgressBarVisibility(false); // Hide progress bar
        }
    }

    // Handles the planting process and progress
    private void ContinuePlanting()
    {
        // Show and reset the progress bar when starting the planting stage
        if (actionCount == 0)
        {
            SetProgressBarVisibility(true);
            UpdateProgressBar(0f);
        }

        actionCount++; // Increment action count

        // Play the planting particle effect at the dirt position
        if (plantingParticleEffect != null)
        {
            plantingParticleEffect.transform.position = dirt.transform.position;
            plantingParticleEffect.Play();
        }

        // Update the progress bar based on the action count
        UpdateProgressBar((float)actionCount / requiredActions);

        // Check if required actions are completed
        if (actionCount >= requiredActions)
        {
            dirt.SetActive(false); // Hide the dirt object
            smallTree.SetActive(true); // Show the small tree object
            actionCount = 0; // Reset action counter
            currentStage = PlantingStage.Watering; // Move to the next stage: Watering
            SetProgressBarVisibility(false); // Hide progress bar
        }
    }

    // Handles the watering process and progress
    private void ContinueWatering()
    {
        // Show and reset the progress bar when starting the watering stage
        if (actionCount == 0)
        {
            SetProgressBarVisibility(true);
            UpdateProgressBar(0f);
        }

        actionCount++; // Increment action count

        // Play the water particle effect at the small tree position
        if (waterParticleEffect != null)
        {
            waterParticleEffect.transform.position = smallTree.transform.position;
            waterParticleEffect.Play();
        }

        // Update the progress bar based on the action count
        UpdateProgressBar((float)actionCount / requiredActions);

        // Check if required actions are completed
        if (actionCount >= requiredActions)
        {
            smallTree.SetActive(false); // Hide the small tree object
            finalTree.SetActive(true); // Show the final tree object
            actionCount = 0; // Reset action counter
            currentStage = PlantingStage.Completed; // Mark the process as completed
            SetProgressBarVisibility(false); // Hide progress bar

            // Play the completed particle effect at the final tree position
            if (completedParticleEffect != null)
            {
                completedParticleEffect.transform.position = finalTree.transform.position;
                completedParticleEffect.Play();
            }
        }
    }

    // Detects when a player or tool enters the trigger area
    void OnTriggerEnter(Collider other)
    {
        // Check the tag of the object that entered the area and set the corresponding boolean
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
        }
        else if (other.CompareTag("Shovel"))
        {
            shovelInArea = true;
        }
        else if (other.CompareTag("Plant"))
        {
            plantInArea = true;
        }
        else if (other.CompareTag("Bucket"))
        {
            bucketInArea = true;
        }
    }

    // Detects when a player or tool exits the trigger area
    void OnTriggerExit(Collider other)
    {
        // Check the tag of the object that exited the area and reset the corresponding boolean
        if (other.CompareTag("Player"))
        {
            playerInArea = false;
        }
        else if (other.CompareTag("Shovel"))
        {
            shovelInArea = false;
        }
        else if (other.CompareTag("Plant"))
        {
            plantInArea = false;
        }
        else if (other.CompareTag("Bucket"))
        {
            bucketInArea = false;
        }
    }
}
