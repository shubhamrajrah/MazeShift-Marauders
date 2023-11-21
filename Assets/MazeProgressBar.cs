using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeProgressBar : MonoBehaviour
{
   public GameObject[] boundaryObjects; 
    private int currentObjectIndex = 0;
    private float timeLeft;
    private float totalTime;
    private bool isActive = false;

    public void StartProgressBarSequence(float duration)
    {
        if (boundaryObjects == null || boundaryObjects.Length == 0)
        {
            Debug.LogError("Boundary objects array is not initialized or empty.");
            return;
        }

        totalTime = duration / boundaryObjects.Length;
        timeLeft = totalTime;
        currentObjectIndex = 0;
        isActive = true;

        foreach (var obj in boundaryObjects)
        {
            obj.SetActive(true);
        }
    }
      void Update()
    {
        if (!isActive)
            return;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            if (currentObjectIndex < boundaryObjects.Length)
            {
                boundaryObjects[currentObjectIndex].SetActive(false); 
                Debug.Log($"Hiding object {currentObjectIndex}");
                currentObjectIndex++;
                timeLeft = totalTime;
            }
            else
            {
                isActive = false;
                Debug.Log("All objects processed. Sequence complete.");
                ResetBoundaryObjects();
            }
        }
    }

    private void ResetBoundaryObjects()
    {
        foreach (var obj in boundaryObjects)
        {
            obj.SetActive(true);
        }
    }

    
}

