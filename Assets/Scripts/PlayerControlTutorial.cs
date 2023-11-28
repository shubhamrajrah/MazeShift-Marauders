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


public class PlayerControlTutorial : MonoBehaviour
{
    //Instructions -
    private string[] instructions = {
        "Tutorial 3",
        "Wall destruction mode activated. You can now destroy one wall of your choice by colliding with it",
        "Woah! you did some damage to the maze. Sike!",
        "Collect the mazeshift powerup to change the maze at your command",
        "Press D to shift the maze to upcoming maze, beware, it could be good or bad!",
        "You are now a certified wall destroyer and maze shifter"
    };

    public TextMeshProUGUI mazeShiftPowerUpText;
    public Text InstructionsTimer;
    public Text InstructionsKeys;
    private Boolean isTimer; 
    private Boolean isKey; 
    public Text InstructionFinal;
    public GameObject Timer_Img;

    public Text dialogueTextGhost;
    public Text dialogueTextSpeed;

    public Boolean speedOn = false;
    public GameObject speedImg;
    public GameObject dialogBoxHighlighter;
    public GameObject mazeshiftHighlighter;

    public float startSpeed = 4f;
    public float speed = 4f;
    public string nextLevel;
    public int curLevel;
    public GameObject gameWinPanel;
    public GameObject intermediateGameWinPanel;
    public GameObject timePanel;
    private Rigidbody rb;

    private LevelInfo _levelInfo;

    // Door Descending 
    public GameObject winDoor;
    public float descentSpeed = 1.0f;
    public float descentDistance = 2.0f;
    public int keyNum = 3;
    public GameObject endBlock;
    public Color targetUnfinish = Color.red;
    public Color targetFinish = Color.green;


    //Ghost Power up
    public GameObject[] walls;
    public int availableGhostPowerUps = 0;
    public bool canMove = true;
    public bool WallDestroyerTouched = false;
    public TimerController timerController;

    public Color timerHighlight = Color.yellow;


    public ProgressBarScript progressBarWallDestroy;
    public GameObject trapBlock; 
    public Vector3 respawnPosition; 
    public Image wallDestroyer;
    public int availableMazeShiftPowerUp = 0;
    public Material noWallMaterialDestruct;
    public Material WallMaterial;

    //Level 4 - Tutorial
    public Text dialougeText;

    public GameObject tutorialPanel;

    public bool GameIsWon { get; private set; } = false;

    public Text instructionForTutorial;
    public Image imageForTutorial;
    public int WallsDestroyed = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.drag = 0f;
        instructionForTutorial.text = instructions[0];
        // Sprite newSprite = Resources.Load<Sprite>("wallD");
        // imageForTutorial.sprite = newSprite;
        // set time scale to 1 in case time scale was mistakenly set to 0
        Time.timeScale = 1;
        // initialize level info
        GlobalVariables.LevelInfo ??= new LevelInfo(curLevel, DateTime.Now);
        // initialize level track
        GlobalVariables.LevelTrack ??= new LevelTrack(curLevel);
        // track level
        GlobalVariables.Level = curLevel;
        _levelInfo = GlobalVariables.LevelInfo;
        // endBlock.GetComponent<Renderer>().material.color = targetUnfinish;
    }
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Set the velocity of the Rigidbody based on input
        rb.velocity = movement * speed;
        if(Input.GetKeyDown(KeyCode.D) && availableMazeShiftPowerUp > 0)
        {
            StartCoroutine(mazeshiftHighlightInfo(2f));
            availableMazeShiftPowerUp--;
            mazeShiftPowerUpText.text = availableMazeShiftPowerUp.ToString();
            walls = GameObject.FindGameObjectsWithTag("WallShift");
            foreach (GameObject wall in walls)
            {
                // Move each wall down by 'descentDistance'
                wall.transform.position -= new Vector3(0, descentDistance, 0);
            }
        }
        if(!WallDestroyerTouched){
            List<GameObject> walls = new List<GameObject>();
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj.CompareTag("WallShift") || obj.CompareTag("Wall"))
                {
                    walls.Add(obj);
                }
            }
            foreach (GameObject wall in walls)
            {
                Renderer renderer = wall.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = WallMaterial;
                }
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WinCollection"))
        {
            Debug.Log("Inside Win Col");
            GameIsWon = true; 
            _levelInfo.CalculateInterval(DateTime.Now);
            // update star number
            collision.gameObject.SetActive(false);
            IntermediateGameWinPanelDisplay();  
        }
        else if (collision.gameObject.CompareTag("WallDestroyer"))
        {
            GameObject image2 = GameObject.Find("Image2");
            if(image2!=null)
            {
                image2.SetActive(false);
            }
            WallDestroyerTouched = true;
            // progressBarWallDestroy.gameObject.SetActive(true);
            // progressBarWallDestroy.StartProgress(3f);
            WallsDestroyed++;

            if(WallsDestroyed < 3){
                Sprite newSprite = Resources.Load<Sprite>("wallD");
                imageForTutorial.sprite = newSprite;
                instructionForTutorial.text = instructions[1];
                StartCoroutine(HighlightInfo(1f));
            }else{
                instructionForTutorial.text = instructions[2];
                StartCoroutine(HighlightInfo(1f));
            }
            
            
            collision.gameObject.SetActive(false);
            _levelInfo.DestructionCollected++;
            List<GameObject> walls = new List<GameObject>();
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj.CompareTag("WallShift") || obj.CompareTag("Wall"))
                {
                    walls.Add(obj);
                }
            }
            foreach (GameObject wall in walls)
            {
                Renderer renderer = wall.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = noWallMaterialDestruct;
                }
            }
            // StartCoroutine(TurnOffWallDestructionMode(3f, walls));
        }else if (collision.gameObject.CompareTag(("ShiftPower")))
        {   
            Sprite newSprite = Resources.Load<Sprite>("maze_shift");
            imageForTutorial.sprite = newSprite;
            instructionForTutorial.text = instructions[4];
            StartCoroutine(mazeshiftHighlightInfo(2f));
            collision.gameObject.SetActive(false);
            availableMazeShiftPowerUp++;
            _levelInfo.MazeShiftCollected++;
            mazeShiftPowerUpText.text = availableMazeShiftPowerUp.ToString();
        }

        
        if (collision.gameObject.CompareTag("Wall") && WallDestroyerTouched)
        {
            Vector3 wallPosition = collision.gameObject.transform.position;
            Debug.Log("trying to dstroy the wall---"+wallPosition);
            if (wallPosition.y >= 0.5f)
            {
                Debug.Log("destroying high wall---"+wallPosition);
                Vector3 newPosition = new Vector3(wallPosition.x, 0f, wallPosition.z);
                Debug.Log("destroying high new position wall---"+newPosition);
                collision.gameObject.transform.position = newPosition;
                collision.gameObject.SetActive(false);
                WallDestroyerTouched = false;
                List<GameObject> walls = new List<GameObject>();
                foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
                {
                    if (obj.CompareTag("WallShift") || obj.CompareTag("Wall"))
                    {
                        walls.Add(obj);
                    }
                }
                foreach (GameObject wall in walls)
                {
                    Renderer renderer = wall.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = WallMaterial;
                    }
                }
            
                // progressBarWallDestroy.gameObject.SetActive(false);
            }
            
        }

    }
    IEnumerator TurnOffWallDestructionMode(float delay, List<GameObject> walls)
    {
            yield return new WaitForSeconds(delay);
            WallDestroyerTouched = false;
            foreach (GameObject wall in walls)
            {
                wall.GetComponent<Renderer>().material = WallMaterial;
            }
    }

    IEnumerator HighlightInfo(float delay)
    {
        dialogBoxHighlighter.SetActive(true);
        yield return new WaitForSeconds(delay);
        dialogBoxHighlighter.SetActive(false);
    }

    IEnumerator mazeshiftHighlightInfo(float delay)
    {
        mazeshiftHighlighter.SetActive(true);
        yield return new WaitForSeconds(delay);
        mazeshiftHighlighter.SetActive(false);
    }

    void GameWinPanelDisplay()
    {
        Time.timeScale = 0;
        gameWinPanel.SetActive(true);
    }
    void IntermediateGameWinPanelDisplay()
    {
        Time.timeScale = 0;
        intermediateGameWinPanel.SetActive(true);
    }
 
}

    

    
