using UnityEngine;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{
        public CameraMover cameraMover; // Assign this in the inspector
    public Text instructionText;

    public float speed = 5.0f; // Speed of the player
    private Rigidbody _rb; // Reference to the Rigidbody component

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Get the Rigidbody component at the start
    }

    public void InitializeForMaze2()
{
    // Reset the player's state as needed for the second maze
    // For example:
    _rb.velocity = Vector3.zero;
    _rb.angularVelocity = Vector3.zero;
    _rb.isKinematic = false;
    speed = 5.0f; // Reset speed or set it to a new value
    instructionText.text = "Find your way out!";
    // Any other initialization code specific to maze 2
}


    void FixedUpdate() // Use FixedUpdate for physics-related updates
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Get horizontal input (left/right arrow keys)
        float moveVertical = Input.GetAxis("Vertical"); // Get vertical input (up/down arrow keys)

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); // Create movement vector

        // Move the player using Rigidbody to ensure proper physics interaction
        _rb.MovePosition(_rb.position + movement * speed * Time.fixedDeltaTime);
        //instructionText.gameObject.SetActive(false);


    if (cameraMover.GetCurrentPosIndex() == 1) // If currentPosIndex is 1 (second index since it's zero-based)
    {
        // Do something, for example:
       // instructionText.text = "Camera is at the second position";
    }
    else
    {
        instructionText.text = "Use F to view the Future Maze";
    }

    }
}
