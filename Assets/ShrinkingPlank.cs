using UnityEngine;

public class ShrinkingPlank : MonoBehaviour
{
    public float shrinkSpeed = 2f; // Adjust the speed as needed
    public float restoreThreshold = 0.01f; // Adjust the threshold as needed

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Shrink the plank
        transform.localScale -= new Vector3(0f,0f,shrinkSpeed * Time.deltaTime);

        // Check if the plank has shrunk to the threshold
        if (transform.localScale.z <= restoreThreshold)
        {
            // Restore the initial scale
            transform.localScale = initialScale;
        }
    }
}