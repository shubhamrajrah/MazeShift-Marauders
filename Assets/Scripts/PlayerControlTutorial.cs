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
    private string[] instruction1 = {
        "Collect Ghost Power up to walk through walls",
        "Press G to walk through wall"
    };

    private string[] instruction2 = {
        "Collect Speed Power up to increase speed",
        "Press S to increase Speed"
    };

    private string[] instructionTimer = {
        "Collect button to buy more time"
    };

    private string[] instructionKeys = {
        "Collect Keys to unlock the Door"
    };

    public Text InstructionsTimer;
     public Text InstructionsKeys;
     private Boolean isTimer; 
     private Boolean isKey; 
     public Text InstructionFinal;
     public GameObject Timer_Img;
     public GameObject Key_Img;

    public Text dialogueTextGhost;
    public Text dialogueTextSpeed;

    public Boolean speedOn = false;
    public GameObject speedImg;


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
    public TextMeshProUGUI keyText;
    // private string _keyTextFormat = "Stars: {0}/{1}";
    private int _keyGet = 0;

    //Ghost Power up
    public GameObject[] walls;
    public int availableGhostPowerUps = 0;
    public TextMeshProUGUI ghostPowerUpText;
    public TextMeshProUGUI speedPowerUpText;
    public TextMeshProUGUI plusFiveSecondsText;
    public bool canMove = true;
    public bool WallDestroyerTouched = false;
    public TimerController timerController;

    public Color timerHighlight = Color.yellow;

    public ProgressBarScript progressBarGhost;
    public ProgressBarScript progressBarSpeed;
    public ProgressBarScript progressBarWallDestroy;
    public GameObject trapBlock; 
    public Vector3 respawnPosition; 

    //Level 4 - Tutorial
    public Text dialougeText;
    private string[] instructions = {
        "Grab to activate destruction mode",
        "Run into a wall to destroy it permenantly"
    };
    public GameObject tutorialPanel;

    public bool GameIsWon { get; private set; } = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.drag = 0f;
      
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
        // keyText.text = string.Format(_keyTextFormat, _keyGet, keyNum);
    }
    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Set the velocity of the Rigidbody based on input
        rb.velocity = movement * speed;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WinCollection"))
        {
            Debug.Log("Inside Win Col");
            GameIsWon = true; 
            _levelInfo.CalculateInterval(DateTime.Now);
            // update star number
            GlobalVariables.LevelStars[curLevel] = Math.Max(GlobalVariables.LevelStars[curLevel], _keyGet);
            collision.gameObject.SetActive(false);
            if (nextLevel == "Menu")
            {
                GameWinPanelDisplay();
            }
            
        }
        else if (collision.gameObject.CompareTag("Tile") || collision.gameObject.CompareTag("Wall"))
        {
            PortalTeleporter.recentlyTeleported = false;  
        }
        else if (collision.gameObject.CompareTag("WallDestroyer"))
        {
            WallDestroyerTouched = true;
            collision.gameObject.SetActive(false);
            // dialougeText.text = instructions[1];
            _levelInfo.DestructionCollected++;
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
                progressBarWallDestroy.gameObject.SetActive(false);
                
                if (curLevel == 4){
                    tutorialPanel.SetActive(false);
                }
                
            }
            
        }

        if (collision.gameObject == trapBlock) 
        {
            transform.position = respawnPosition; 
        }
    }



    void GameWinPanelDisplay()
    {
        Time.timeScale = 0;
        gameWinPanel.SetActive(true);
    }
 
}

    

    
