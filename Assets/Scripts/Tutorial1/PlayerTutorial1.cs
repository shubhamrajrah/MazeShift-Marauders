using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTutorial1 : MonoBehaviour
{
    public float speed = 5.0f;
    // public GameObject arrowUI;
    public Text instructionText;
    public GameObject keyUI;
    public Text timerText;
    // public GameObject FreezeBounceText;
    public GameObject powerUpUI; // Assign the power-up UI GameObject in the inspector

    private float timer = 60.0f;
    private bool hasKey = false;
    private bool canMove = true;

    // for Key Counter animation
    // public GameObject keyCountUI; // Assign the KeyCount UI GameObject in the inspector
    // public GameObject borderImage; // Assign the border GameObject in the inspector

    // for Global Timer Animation
    public GameObject globalTimerUI; // Assign the GlobalTimer UI GameObject in the inspector
    public GameObject globalTimerBorder; // Assign the GlobalTimer border GameObject in the inspector
    private bool hasFlashedGlobalTimerBorder = false;
    public Transform newCameraPosition; // Assign this in the inspector with the new position and rotation for the camera

    public bool futureFlag = false;

    private bool hasDisplayedInstruction = false;
public GameObject instructionPanel; 

public GameObject portal1;
public GameObject portal2;
public GameObject FreezeImage;
public GameObject KeyImage;
public GameObject ArrowImage;
public GameObject fLetterImage;
private Rigidbody rb;
public GameObject PortalImage;
public GameObject playerForMaze1; // Assign the player GameObject for maze1 in the inspector
public GameObject playerForMaze2; // Assign the player GameObject for maze2 in the inspector
public Transform startOfMaze2;
public GameObject freeze;

private GameObject keyObject;

    public GameObject winCollection; 

    public GameObject IntermediateWinPanel;


//  private LevelInfo _levelInfo;
//  public int curLevel;

    void Start()
    {   
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.drag = 0f;
        // canMove = false;
        instructionText.text = "This is a shifting maze game";
        globalTimerUI.SetActive(false);
        globalTimerBorder.SetActive(false);
        StartCoroutine(DisplayInstructionAfterDelay1());
        portal1.gameObject.SetActive(false);
        portal2.gameObject.SetActive(false);
        freeze.gameObject.SetActive(false);
        keyObject = GameObject.FindWithTag("Key"); // Find the Key GameObject by its tag
    }

   

    void Update()
    {
        if (canMove)
        {
            
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            // transform.Translate(movement * speed * Time.deltaTime);
            rb.velocity = movement * speed;
            if ((moveHorizontal != 0 || moveVertical != 0) && !hasDisplayedInstruction)
            {
                StartCoroutine(DisplayInstructionAfterDelay());
                            hasDisplayedInstruction = true;

            }

            
        }

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Round(timer).ToString();
    }

    private IEnumerator DisplayInstructionAfterDelay1()
    {       
        yield return new WaitForSeconds(1); // Wait for 2 seconds
        ArrowImage.gameObject.SetActive(true);  
        instructionText.text = "Use arrow keys to move";
        yield return new WaitForSeconds(1);
        canMove = true;
        // instructionText.text = "Collect the key"; // Update text after 2 seconds
        // arrowUI.SetActive(false); // Deactivate arrowUI after showing the instruction
    }


private IEnumerator DisplayInstructionAfterDelay()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        ArrowImage.gameObject.SetActive(false);
        KeyImage.gameObject.SetActive(true);
        instructionText.text = "Collect the star"; // Update text after 2 seconds
        // arrowUI.SetActive(false); // Deactivate arrowUI after showing the instruction
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            CollectKey();
            Destroy(other.gameObject); // Assuming the key is the object to be destroyed
            keyObject = null; 
        }
        if (other.CompareTag("FreezeCollection"))
        {
            CollectPowerUp();
            Destroy(other.gameObject); // Assuming the power-up is the object to be destroyed
        }

        if (other.CompareTag("WinCollection"))
        {
           IntermediateWinPanel.SetActive(true); 
           Destroy(other.gameObject);
            instructionPanel.SetActive(false);

        }


    }

    private void CollectKey()
    {
        hasKey = true;
        // canMove = false;
        instructionText.text = "Stars will unlock the levels";

        // keyCountUI.SetActive(true);
        // StartCoroutine(FlashBorderForSeconds(2, borderImage));
        StartCoroutine(ProceedAfterKeyCollection());
    }

    private IEnumerator ProceedAfterKeyCollection()
{
    instructionText.text = "Stars will unlock the levels";
    yield return new WaitForSeconds(2); // Duration for displaying the first instruction

    // Now show the global timer UI and border
    globalTimerUI.SetActive(true);
    StartCoroutine(FlashBorderForSeconds(2, globalTimerBorder));
    yield return new WaitForSeconds(2); // Wait for the global timer border to flash

    canMove = true; // Allow the player to move again
    freeze.gameObject.SetActive(true);
    KeyImage.gameObject.SetActive(false);
    FreezeImage.gameObject.SetActive(true);
    instructionText.text = "Collect to get 5 seconds more";
}


private void CollectPowerUp()
{
    FreezeImage.gameObject.SetActive(false);
    timer += 5.0f; // Add 5 seconds to the timer
    fLetterImage.gameObject.SetActive(true);
    instructionText.text = "Press F to view the Future Maze"; 

    futureFlag = true;

    // Deactivate the timer UI and border here if needed
    globalTimerUI.SetActive(false);
    globalTimerBorder.SetActive(false);
}


private IEnumerator WaitAndAddPortal(float waitTime)
{
    yield return new WaitForSeconds(4);
    PortalImage.gameObject.SetActive(true);
        fLetterImage.gameObject.SetActive(false);

    instructionText.text = "Use Portal to Teleport"; 
portal1.gameObject.SetActive(true);
portal2.gameObject.SetActive(true);

   
}


    private IEnumerator ProceedAfterPowerUpCollection()
    {
        yield return new WaitForSeconds(2); // Wait for the power-up text to display
        // FreezeBounceText.SetActive(false);
        canMove = false; // Stop the player from moving
        timerText.text = "Let's go ahead"; // Change the timer text to display the message
        yield return new WaitForSeconds(3);
        MoveCameraToNextLocation(); // Call the method to move the camera

        yield return new WaitForSeconds(3);
    TransitionToNextMaze();
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

    private void TransitionToNextMaze()
{
    // Disable player for maze 1
    playerForMaze1.SetActive(false);

    // Enable player for maze 2 and position it at the start
    playerForMaze2.SetActive(true);
    playerForMaze2.transform.position = startOfMaze2.position;
    playerForMaze2.transform.rotation = startOfMaze2.rotation;

    // Assuming you have a method to initialize or reset the player's state for maze 2
    playerForMaze2.GetComponent<Player2>().InitializeForMaze2();
}
}
