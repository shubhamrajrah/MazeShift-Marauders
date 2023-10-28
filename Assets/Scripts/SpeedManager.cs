using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{

    private GameObject[] speedPrefabs;

    private void Start()
    {
        // Populate the array with all child objects (speed prefabs)
        speedPrefabs = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            speedPrefabs[i] = transform.GetChild(i).gameObject;
        }

        // Deactivate all speed prefabs
        foreach (GameObject speed in speedPrefabs)
        {
            speed.SetActive(false);
        }

        // Randomly activate two of them
        ActivateRandomSpeeds(2);
    }

    private void ActivateRandomSpeeds(int numberOfspeedsToActivate)
    {
        for (int i = 0; i < numberOfspeedsToActivate; i++)
        {
            GameObject randomSpeed;
            do
            {
                randomSpeed = speedPrefabs[Random.Range(0, speedPrefabs.Length)];
            } while (randomSpeed.activeSelf); // Ensure we don't select an already activated speed

            randomSpeed.SetActive(true);
        }
    }
}
