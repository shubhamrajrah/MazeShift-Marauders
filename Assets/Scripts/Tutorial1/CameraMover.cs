using System.Collections;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform[] cameraPositions; // Assign in inspector
    private int currentPosIndex = 0;

    void Update()
    {
        // Check for a key press and if the currentPosIndex is within bounds
        if (Input.GetKeyDown(KeyCode.Space) && currentPosIndex < cameraPositions.Length - 1)
        {
            currentPosIndex++;
            StartCoroutine(MoveToPosition(cameraPositions[currentPosIndex]));
        }
    }

    private IEnumerator MoveToPosition(Transform target)
    {
        float timeToMove = 1.0f; // Time in seconds to complete the move
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originalPosition, target.position, (elapsedTime / timeToMove));
            transform.rotation = Quaternion.Slerp(originalRotation, target.rotation, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    public void MoveToNextPosition()
{
    if (currentPosIndex < cameraPositions.Length - 1)
    {
        currentPosIndex++;
        StartCoroutine(MoveToPosition(cameraPositions[currentPosIndex]));
    }
}
public int GetCurrentPosIndex()
    {
        return currentPosIndex;
    }
}
