using UnityEngine;
using UnityEngine.UI;

public class TreePlanting : MonoBehaviour
{
    public GameObject questMarker;
    public GameObject dirt;
    public GameObject smallTree;
    public GameObject finalTree;
    public ParticleSystem dirtParticleEffect;
    public ParticleSystem plantingParticleEffect;
    public ParticleSystem waterParticleEffect;
    public ParticleSystem completedParticleEffect;
    public Image ProgressBar;
    public GameObject progressBarContainer;

    private bool playerInArea = false;
    private bool shovelInArea = false;
    private bool plantInArea = false;
    private bool bucketInArea = false;

    private int actionCount = 0;
    private const int requiredActions = 5;

    private enum PlantingStage
    {
        NotStarted,
        Digging,
        Planting,
        Watering,
        Completed
    }

    private PlantingStage currentStage = PlantingStage.NotStarted;

    void Start()
    {
        dirt.SetActive(false);
        smallTree.SetActive(false);
        finalTree.SetActive(false);
        SetProgressBarVisibility(false);
        Debug.Log("TreePlanting: Start method called. Initial stage: " + currentStage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformAction();
        }
    }

    public void UpdateProgressBar(float progress)
    {
        ProgressBar.fillAmount = progress;
    }

    private void SetProgressBarVisibility(bool isVisible)
    {
        progressBarContainer.gameObject.SetActive(isVisible);
    }

    private void PerformAction()
    {
        if (!playerInArea) return;

        Debug.Log("TreePlanting: PerformAction called. Current stage: " + currentStage);
        switch (currentStage)
        {
            case PlantingStage.NotStarted:
                if (shovelInArea) StartDigging();
                else Debug.Log("TreePlanting: Cannot start digging. No shovel in area.");
                break;
            case PlantingStage.Digging:
                if (shovelInArea) ContinueDigging();
                else Debug.Log("TreePlanting: Cannot continue digging. No shovel in area.");
                break;
            case PlantingStage.Planting:
                if (plantInArea) ContinuePlanting();
                else Debug.Log("TreePlanting: Cannot plant. No plant in area.");
                break;
            case PlantingStage.Watering:
                if (bucketInArea) ContinueWatering();
                else Debug.Log("TreePlanting: Cannot water. No bucket in area.");
                break;
            case PlantingStage.Completed:
                Debug.Log("TreePlanting: Tree is already fully grown.");
                SetProgressBarVisibility(false);
                break;
        }
    }

    private void StartDigging()
    {
        Debug.Log("TreePlanting: Starting to dig.");
        currentStage = PlantingStage.Digging;
        actionCount = 0;
        SetProgressBarVisibility(true);
        UpdateProgressBar(0f);
        ContinueDigging();
    }

    private void ContinueDigging()
    {
        actionCount++;
        Debug.Log("TreePlanting: Digging. Action count: " + actionCount);
        if (dirtParticleEffect != null)
        {
            dirtParticleEffect.transform.position = questMarker.transform.position;
            dirtParticleEffect.Play();
        }

        UpdateProgressBar((float)actionCount / requiredActions);

        if (actionCount >= requiredActions)
        {
            questMarker.SetActive(false);
            dirt.SetActive(true);
            actionCount = 0;
            currentStage = PlantingStage.Planting;
            SetProgressBarVisibility(false);
            Debug.Log("TreePlanting: Digging completed. Moving to Planting stage.");
        }
    }

    private void ContinuePlanting()
    {
        if (actionCount == 0)
        {
            SetProgressBarVisibility(true);
            UpdateProgressBar(0f);
        }

        actionCount++;
        Debug.Log("TreePlanting: Planting. Action count: " + actionCount);
        if (plantingParticleEffect != null)
        {
            plantingParticleEffect.transform.position = dirt.transform.position;
            plantingParticleEffect.Play();
        }

        UpdateProgressBar((float)actionCount / requiredActions);

        if (actionCount >= requiredActions)
        {
            dirt.SetActive(false);
            smallTree.SetActive(true);
            actionCount = 0;
            currentStage = PlantingStage.Watering;
            SetProgressBarVisibility(false);
            Debug.Log("TreePlanting: Planting completed. Moving to Watering stage.");
        }
    }

    private void ContinueWatering()
    {
        if (actionCount == 0)
        {
            SetProgressBarVisibility(true);
            UpdateProgressBar(0f);
        }

        actionCount++;
        Debug.Log("TreePlanting: Watering. Action count: " + actionCount);
        if (waterParticleEffect != null)
        {
            waterParticleEffect.transform.position = smallTree.transform.position;
            waterParticleEffect.Play();
        }

        UpdateProgressBar((float)actionCount / requiredActions);

        if (actionCount >= requiredActions)
        {
            smallTree.SetActive(false);
            finalTree.SetActive(true);
            actionCount = 0;
            currentStage = PlantingStage.Completed;
            SetProgressBarVisibility(false);
            Debug.Log("TreePlanting: Watering completed. Tree fully grown.");

            if (completedParticleEffect != null)
            {
                completedParticleEffect.transform.position = finalTree.transform.position;
                completedParticleEffect.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
            Debug.Log("TreePlanting: Player entered tree planting area.");
        }
        else if (other.CompareTag("Shovel"))
        {
            shovelInArea = true;
            Debug.Log("TreePlanting: Shovel entered tree planting area.");
        }
        else if (other.CompareTag("Plant"))
        {
            plantInArea = true;
            Debug.Log("TreePlanting: Plant entered tree planting area.");
        }
        else if (other.CompareTag("Bucket"))
        {
            bucketInArea = true;
            Debug.Log("TreePlanting: Bucket entered tree planting area.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = false;
            Debug.Log("TreePlanting: Player exited tree planting area.");
        }
        else if (other.CompareTag("Shovel"))
        {
            shovelInArea = false;
            Debug.Log("TreePlanting: Shovel exited tree planting area.");
        }
        else if (other.CompareTag("Plant"))
        {
            plantInArea = false;
            Debug.Log("TreePlanting: Plant exited tree planting area.");
        }
        else if (other.CompareTag("Bucket"))
        {
            bucketInArea = false;
            Debug.Log("TreePlanting: Bucket exited tree planting area.");
        }
    }
}