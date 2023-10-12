using UnityEngine;

public class blockController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the cube moves
    public float moveHeight = 0.5f;  // The height for the raised wall

    private float baseHeight;     // Starting height for the block
    private float targetHeight;   // The target height to which the block should move

    private void Start()
    {
        baseHeight = transform.position.y;
        targetHeight = baseHeight;
    }

    private void Update()
    {
        // Smoothly move towards the target height using MoveTowards
        float newY = Mathf.MoveTowards(transform.position.y, targetHeight, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // This method sets the target position based on the maze value
    public void AdjustBlock(int value)
    {
        if (gameObject.CompareTag("Wall"))
        {
            targetHeight = (value == 1) ? baseHeight + moveHeight : baseHeight;
        }
        // You can add further adjustments for other block types if needed.
    }
}
