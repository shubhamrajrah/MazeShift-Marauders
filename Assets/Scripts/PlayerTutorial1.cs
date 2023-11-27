using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTutorial1 : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject arrowUI;
    public Text instructionText;
    public GameObject keyUI;
    public Text timerText;
    // public GameObject FreezeBounceText;
    public GameObject powerUpUI; // Assign the power-up UI GameObject in the inspector

    private float timer = 60.0f;
    private bool hasKey = false;
    private bool canMove = true;

    // for Key Counter animation
    public GameObject keyCountUI; // Assign the KeyCount UI GameObject in the inspector
    public GameObject borderImage; // Assign the border GameObject in the inspector

    // for Global Timer Animation
    public GameObject globalTimerUI; // Assign the GlobalTimer UI GameObject in the inspector
    public GameObject globalTimerBorder; // Assign the GlobalTimer border GameObject in the inspector
    private bool hasFlashedGlobalTimerBorder = false;
    public Transform newCameraPosition; // Assign this in the inspector with the new position and rotation for the camera


    void Start()
    {
        instructionText.text = "Use arrow keys to move";
        keyCountUI.SetActive(false);
        globalTimerUI.SetActive(false);
        globalTimerBorder.SetActive(false);
    }

    void Update()
    {
        if (canMove)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if ((moveHorizontal != 0 || moveVertical != 0) && arrowUI.activeSelf)
            {
                arrowUI.SetActive(false);
                instructionText.text = "Collect the key"; // Update text to instruct player to collect the key
            }

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            transform.Translate(movement * speed * Time.deltaTime);
        }

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Round(timer).ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            CollectKey();
            Destroy(other.gameObject); // Assuming the key is the object to be destroyed
        }
        else if (other.CompareTag("FreezeCollection"))
        {
            CollectPowerUp();
            Destroy(other.gameObject); // Assuming the power-up is the object to be destroyed
        }
    }

    private void CollectKey()
    {
        hasKey = true;
        canMove = false;
        instructionText.text = "Key Collected! Keys will unlock the door";
        keyCountUI.SetActive(true);
        StartCoroutine(FlashBorderForSeconds(2, borderImage));
        StartCoroutine(ProceedAfterKeyCollection());
    }

    private IEnumerator ProceedAfterKeyCollection()
    {
        yield return new WaitForSeconds(2); // Wait for the border to flash
        instructionText.text = "Reach the destination on time";
        globalTimerUI.SetActive(true);
        StartCoroutine(FlashBorderForSeconds(2, globalTimerBorder));
        yield return new WaitForSeconds(2); // Wait for the global timer border to flash
        canMove = true; // Allow the player to move again
        instructionText.text = "Out of time? Get 5 seconds more";
    }

  private void CollectPowerUp()
{
    timer += 5.0f; // Add 5 seconds to the timer
    instructionText.text = "Let's go!"; // Change instruction text to "Let's go!"
    StartCoroutine(WaitAndMoveCamera(2)); // Wait for 2 seconds and then move the camera
}

private IEnumerator WaitAndMoveCamera(float waitTime)
{
    yield return new WaitForSeconds(waitTime);
    CameraMover cameraMover = Camera.main.GetComponent<CameraMover>(); // Assuming CameraMover is attached to the main camera
    if (cameraMover != null)
    {
        cameraMover.MoveToNextPosition(); // Call method to move camera to next position
    }
}


    private IEnumerator ProceedAfterPowerUpCollection()
    {
        yield return new WaitForSeconds(2); // Wait for the power-up text to display
        // FreezeBounceText.SetActive(false);
        canMove = false; // Stop the player from moving
        timerText.text = "Let's go ahead"; // Change the timer text to display the message
        yield return new WaitForSeconds(3);
        MoveCameraToNextLocation(); // Call the method to move the camera
    }

  private void MoveCameraToNextLocation()
{
    StartCoroutine(MoveCamera(Camera.main.transform, newCameraPosition.position, newCameraPosition.rotation, 3f));
}
private IEnumerator MoveCamera(Transform cameraTransform, Vector3 newPosition, Quaternion newRotation, float timeToMove)
{
    Vector3 startPosition = cameraTransform.position;
    Quaternion startRotation = cameraTransform.rotation;
    float elapsedTime = 0f;

    while (elapsedTime < timeToMove)
    {
        cameraTransform.position = Vector3.Lerp(startPosition, newPosition, (elapsedTime / timeToMove));
        cameraTransform.rotation = Quaternion.Lerp(startRotation, newRotation, (elapsedTime / timeToMove));
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    cameraTransform.position = newPosition; // Ensure the final position is set accurately
    cameraTransform.rotation = newRotation; // Ensure the final rotation is set accurately
}



    private IEnumerator FlashBorderForSeconds(float seconds, GameObject borderObject)
    {
        borderObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        borderObject.SetActive(false);
    }
}

