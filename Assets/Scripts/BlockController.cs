using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the cube moves
    public float moveHeight = 0.5f;  // The height for the raised wall

    private float baseHeight;     // Starting height for the block
    private float targetHeight;   // The target height to which the block should move
    public int row;
    public int col;

    private void Start()
    {
        baseHeight = transform.position.y;
        targetHeight = baseHeight;
    }

    private void Update()
    {
        // Smoothly move towards the target height using MoveTowards
        var position = transform.position;
        float newY = Mathf.MoveTowards(position.y, targetHeight, moveSpeed * Time.deltaTime);
        position = new Vector3(position.x, newY, position.z);
        transform.position = position;
    }

    // This method sets the target position based on the maze value
    public void AdjustBlock(int value)
    {
        if (gameObject.CompareTag("Wall"))
        {
            // Renderer renderer = gameObject.GetComponent<Renderer>();
            // if (renderer != null)
            // {
            //     renderer.material.color = Color.black;
            // }
            // gameObject.GetComponent<Collider>().isTrigger = true;

            targetHeight = (value == 1) ? baseHeight + moveHeight : baseHeight;
        }
        // You can add further adjustments for other block types if needed.
    }
    public bool IsUp()
    {
        // Assuming that when the wall is "up" its y position is greater than the baseHeight
        return transform.position.y > baseHeight;
    }
    public bool IsAtCoordinates(int targetRow, int targetCol)
    {
        return row == targetRow && col == targetCol;
    }
}
