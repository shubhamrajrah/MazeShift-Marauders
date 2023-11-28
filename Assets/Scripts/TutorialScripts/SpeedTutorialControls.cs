using System;
using System.Collections;
using System.Collections.Generic;
using Analytic;
using Analytic.DTO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SpeedTutorialControls : MonoBehaviour
{
    // Start is called before the first frame update
    public float startSpeed = 2f;
    public float speed = 2f;
    private int availableSpeedPowerUps = 0; 
    public TextMeshProUGUI speedPowerUpText;
    public ProgressBarScript progressBarSpeed;

    public Boolean speedOn = false;
    public GameObject speedImg;
    private Rigidbody rb;
    public float yThreshold = -0.2f;

    public GameObject[] walls;

    public int availableGhostPowerUps = 0;
    public TextMeshProUGUI ghostPowerUpText;
    public float ghostAbilityDuration = 5f;
    public Boolean ghostOn = false;
    public Boolean ghostPressed = false;
    public Material blue;
    public ProgressBarScript progressBarGhost;
    public Material WallMaterial;
    public string nextLevel;
    public GameObject intermediateGameWinPanel;
    public Image GhostPrompt;
    public TextMeshProUGUI GhostPromptText;
    private int ghostflag = 0;
    private float scalevalue = 1;
    private int scaleflag=1;
    private Vector3 initialScale;


    public Image SpeedPromptImage;
    public TextMeshProUGUI SpeedPromptText;
    private int speedflag = 0;
    private float scalevalue_speed = 1;
    private int scaleflag_speed=1;
    private Vector3 initialScale_speed;



    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        walls = GameObject.FindGameObjectsWithTag("Wall");
        //rb.drag = 5f;
        GhostPrompt.enabled = false;
        GhostPromptText.enabled = false;
        SpeedPromptImage.enabled = false;
        SpeedPromptText.enabled = false;
        initialScale = GhostPrompt.rectTransform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Alternatively, you can use specific keys
        // float horizontalInput = Input.GetKey(KeyCode.RightArrow) ? 1f : (Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f);
        // float verticalInput = Input.GetKey(KeyCode.UpArrow) ? 1f : (Input.GetKey(KeyCode.DownArrow) ? -1f : 0f);

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement.Normalize();
        // Calculate movement direction
        float yVelocity = rb.velocity.y;

        // Set the modified x and z components to the movement vector
        Vector3 modifiedMovement = new Vector3(movement.x * speed, yVelocity, movement.z * speed);

        // Set the complete modified velocity to the Rigidbody
        rb.velocity = modifiedMovement;

        if (Input.GetKeyDown(KeyCode.S) && availableSpeedPowerUps > 0) // Check for 'G' press and if power-ups are available
        {
            speedOn = true;
            UseSpeedPowerUp();
            availableSpeedPowerUps--; // decrement the power-up count
            //_levelInfo.SpeedUsed++;
            speedPowerUpText.text = availableSpeedPowerUps.ToString();
            // if(curLevel == 3)
            // {
            //     dialogueTextSpeed.gameObject.SetActive(false);
            //     speedImg.gameObject.SetActive(false);
            // }
            SpeedPromptImage.enabled = false;
            SpeedPromptText.enabled = false;
           
        }
        // Check if the player's y position is below the threshold
        if (transform.position.y < yThreshold)
        {
            // Restart the level
            RestartLevel();
        }
        if(GhostPrompt.enabled)
        {
            if(scaleflag==1){
            GhostPrompt.rectTransform.localScale += new Vector3(scalevalue*Time.deltaTime,scalevalue*Time.deltaTime,scalevalue*Time.deltaTime);
        }
            else{
                GhostPrompt.rectTransform.localScale -= new Vector3(scalevalue*Time.deltaTime,scalevalue*Time.deltaTime,scalevalue*Time.deltaTime);
            }
            //scalevalue+=0.01f*scaleflag;
            Vector3 currentScale = GhostPrompt.rectTransform.localScale;
            if(currentScale.x >= initialScale.x * 1.3f)
            {
                scaleflag = -1;
            }
            else if (currentScale.x <= initialScale.x)
            {
                // Reset to the original scale
                GhostPrompt.rectTransform.localScale = initialScale;

                // Reverse the scaling direction
                scaleflag = 1;
            }
        }
        if(SpeedPromptImage.enabled)
        {
            if(scaleflag_speed==1){
            SpeedPromptImage.rectTransform.localScale += new Vector3(scalevalue_speed*Time.deltaTime,scalevalue_speed*Time.deltaTime,scalevalue_speed*Time.deltaTime);
        }
            else{
            SpeedPromptImage.rectTransform.localScale -= new Vector3(scalevalue_speed*Time.deltaTime,scalevalue_speed*Time.deltaTime,scalevalue_speed*Time.deltaTime);
            }
            //scalevalue+=0.01f*scaleflag;
            Vector3 currentScale_speed = SpeedPromptImage.rectTransform.localScale;
            if(currentScale_speed.x >= initialScale.x * 1.3f)
            {
                scaleflag_speed = -1;
            }
            else if (currentScale_speed.x <= initialScale.x)
            {
                // Reset to the original scale
                SpeedPromptImage.rectTransform.localScale = initialScale;

                // Reverse the scaling direction
                scaleflag_speed = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.G) && availableGhostPowerUps > 0) // Check for 'G' press and if power-ups are available
            {
                availableGhostPowerUps--;
                UseGhostPowerUp();
                GhostPrompt.enabled = false;
                GhostPromptText.enabled = false;
                ghostPowerUpText.text = availableGhostPowerUps.ToString();

            }
    }
    void RestartLevel()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Speed"))
        {
            Debug.Log("Inside if...");
            availableSpeedPowerUps++;
            //_levelInfo.SpeedCollected++;
            speedPowerUpText.text = availableSpeedPowerUps.ToString();
            collision.gameObject.SetActive(false);
            if(speedflag ==0){
            SpeedPromptImage.enabled = true;
            SpeedPromptText.enabled =true;
        }
            speedflag = 1;
        }
        if (collision.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("Inside if...");
            availableGhostPowerUps++;
            //_levelInfo.GhostCollected++;
            // if(curLevel == 3)
            // {
            //     dialogueTextGhost.text = instruction1[1];
            // }
            
            ghostPowerUpText.text = availableGhostPowerUps.ToString();
            collision.gameObject.SetActive(false);
            if(ghostflag ==0){
            GhostPrompt.enabled = true;
            GhostPromptText.enabled =true;
        }
            ghostflag = 1;

        }
        if (collision.gameObject.CompareTag("WinCollection"))
        {
            //Debug.Log("Inside Win Col");
            //_levelInfo.CalculateInterval(DateTime.Now);
            collision.gameObject.SetActive(false);
            //IntermediateGameWinPanelDisplay();
            intermediateGameWinPanel.SetActive(true);
            Time.timeScale = 0;

            // TODO: Load Next Level
        }
    }
    void ChangeColorToBlue(GameObject wallGameObject)
    {
    Renderer renderer = wallGameObject.GetComponent<Renderer>();
    if (renderer != null)
    {
       // renderer.material.color = new Color(0.68f, 0.85f, 0.9f, 1.0f);
        renderer.material = blue;

    }
    }
     void UseGhostPowerUp()
    {
        GhostPowerUp();
        progressBarGhost.StartProgress(5f);

    }

    void GhostPowerUp()
    {
        Debug.Log("Inside GhostPowerUp");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        Debug.Log("Fine got walls");
        foreach (GameObject wall in walls)
        {
            Debug.Log("Inside FOR");
            wall.GetComponent<Collider>().isTrigger = true;
            ChangeColorToBlue(wall);
        }

        StartCoroutine(TurnOffGhostPowerUp(5f));
    }

    IEnumerator TurnOffGhostPowerUp(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Now, set isTrigger back to false for all walls
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<Collider>().isTrigger = false;
            wall.GetComponent<Renderer>().material = WallMaterial;
        }
    }
    void UseSpeedPowerUp()
    {
        speed = 4f;
        progressBarSpeed.StartProgress(5f);
        StartCoroutine(TurnOffSpeedPowerUp(5f));
    }
    IEnumerator TurnOffSpeedPowerUp(float delay)
    {
        yield return new WaitForSeconds(delay);

        speed = startSpeed;
        
    }
}
