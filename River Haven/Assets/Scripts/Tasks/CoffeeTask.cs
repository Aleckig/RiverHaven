using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeTask : MonoBehaviour
{
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private List<Cable> leftCables = new List<Cable>();
    [SerializeField] private List<Cable> rightCables = new List<Cable>();
    [SerializeField] private Camera mainCamera; // Reference to main camera

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        ShuffleCables();
    }

    private void ShuffleCables()
    {
        List<Color> availableColors = new List<Color>(colors);
        List<int> availableLeftIndices = new List<int>();
        List<int> availableRightIndices = new List<int>();

        for (int i = 0; i < leftCables.Count; i++)
        {
            availableLeftIndices.Add(i);
            leftCables[i].Initialize(true, mainCamera);
        }

        for (int i = 0; i < rightCables.Count; i++)
        {
            availableRightIndices.Add(i);
            rightCables[i].Initialize(false, mainCamera);
        }

        while (availableColors.Count > 0 && availableLeftIndices.Count > 0 && availableRightIndices.Count > 0)
        {
            int colorIndex = Random.Range(0, availableColors.Count);
            int leftIndex = Random.Range(0, availableLeftIndices.Count);
            int rightIndex = Random.Range(0, availableRightIndices.Count);

            Color selectedColor = availableColors[colorIndex];
            leftCables[availableLeftIndices[leftIndex]].SetColor(selectedColor);
            rightCables[availableRightIndices[rightIndex]].SetColor(selectedColor);

            availableColors.RemoveAt(colorIndex);
            availableLeftIndices.RemoveAt(leftIndex);
            availableRightIndices.RemoveAt(rightIndex);
        }
    }
}