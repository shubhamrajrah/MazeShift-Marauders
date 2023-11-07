using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    // An array to store all child ghost prefabs
    private GameObject[] ghostPrefabs;

    private void Start()
    {
        // Populate the array with all child objects (ghost prefabs)
        ghostPrefabs = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            ghostPrefabs[i] = transform.GetChild(i).gameObject;
        }

        // Deactivate all ghost prefabs
        foreach (GameObject ghost in ghostPrefabs)
        {
            ghost.SetActive(false);
        }

        // Randomly activate two of them
        ActivateRandomGhosts(1);
    }

    private void ActivateRandomGhosts(int numberOfGhostsToActivate)
    {
        for (int i = 0; i < numberOfGhostsToActivate; i++)
        {
            GameObject randomGhost;
            do
            {
                randomGhost = ghostPrefabs[Random.Range(0, ghostPrefabs.Length)];
            } while (randomGhost.activeSelf); // Ensure we don't select an already activated ghost

            randomGhost.SetActive(true);
        }
    }
}