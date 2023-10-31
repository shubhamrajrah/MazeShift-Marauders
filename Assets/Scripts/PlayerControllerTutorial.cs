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


public class PlayerControllerTutorial : MonoBehaviour
{
    public float startSpeed = 1.5f;
    public float speed = 1.5f;
    // public string nextLevel;
    // public int curLevel;
    // public GameObject gameWinPanel;
     public GameObject timePanel;


    // private LevelInfo _levelInfo;
    // private LoadLevel _loadLevel;

    // Door Descending 
    //public GameObject winDoor;
    // public float descentSpeed = 1.0f;
    // public float descentDistance = 2.0f;
    //  private bool _isDescending = false;

    // //Ghost Power up
     public GameObject[] walls;
    // private int availableGhostPowerUps = 0;
    // private int availableSpeedPowerUps = 0;
    // public TextMeshProUGUI ghostPowerUpText;
    // public TextMeshProUGUI speedPowerUpText;
     public TextMeshProUGUI plusFiveSecondsText;
    // public bool canMove = true;
     public TimerController timerController;

     public Color timerHighlight = Color.yellow;

    // public ProgressBarScript progressBarGhost;
    // public ProgressBarScript progressBarSpeed;
    public GameObject ghost;
    // public TextMeshProUGUI instruction1;
    // public TextMeshProUGUI instruction2;
    // public TextMeshProUGUI instruction3;
    // public TextMeshProUGUI instruction4;
    // public TextMeshProUGUI instruction5;

    private string[] instructions = {
        "Use Arrow Keys to move Player",
        "Collect Speed Power up to increase speed",
        "Press S to increase Speed",
        "Collect Ghost Power up to walk through walls",
        "Press G to walk through wall",
        "Press P to view the Future Maze",
        "Now you can view the Future Maze anytime"

    };
      public Text dialogueText;

    
    void Start()
    {
        dialogueText.text = instructions[0];

        ghost.gameObject.SetActive(false);
        
        //plusFiveSecondsText.gameObject.SetActive(false);
        // if (curLevel == 3 || curLevel == 4)
        // {
        //     progressBarGhost.gameObject.SetActive(false);
        //     progressBarSpeed.gameObject.SetActive(false);


        // }
        // set time scale to 1 in case time scale was mistakenly set to 0
        // Time.timeScale = 1;
        // if (GlobalVariables.LevelInfo == null)
        // {
        //     // called when enter a new level
        //     GlobalVariables.LevelInfo = new LevelInfo(curLevel, DateTime.Now);
        // }

        // _loadLevel = gameObject.AddComponent<LoadLevel>();
        // _levelInfo = GlobalVariables.LevelInfo;
    }

    void Update()
    {

        // if (!canMove) return;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
            
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
            
        }

        
        // //Ghost Power up
        if (Input.GetKeyDown(KeyCode.G)) // Check for 'G' press and if power-ups are available
        {
            UseGhostPowerUp();
        }
        if (Input.GetKeyDown(KeyCode.S)) // Check for 'G' press and if power-ups are available
        {
            UseSpeedPowerUp();
        }
        if (Input.GetKeyDown(KeyCode.P)) // Check for 'G' press and if power-ups are available
        {
            dialogueText.text = instructions[6];
        }
    }
    public void HandleFreezeEffect(int freezeTime)
    {
        StartCoroutine(FreezePlayerRoutine(freezeTime));
    }

    private IEnumerator FreezePlayerRoutine(int freezeTime)
    {
        // canMove = false; 

        Image panelImage = timePanel.GetComponent<Image>();
        plusFiveSecondsText.gameObject.SetActive(true);
        Time.timeScale = 0;
        timePanel.SetActive(true);
        panelImage.color = timerHighlight;
        yield return new WaitForSecondsRealtime(0.5f);
        timePanel.SetActive(false);
        Time.timeScale = 1;
        plusFiveSecondsText.gameObject.SetActive(false);

        
    }


    void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("WinTile") && !_isDescending)
        // {
        //     StartCoroutine(DoorDescend());
        // }
        if (collision.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("Inside if...");
           
        //    instruction4.gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            // instruction5.gameObject.SetActive(true);
            dialogueText.text = instructions[4];

        }
         if (collision.gameObject.CompareTag("Speed"))
        {
            Debug.Log("Inside if...");
            // instruction2.gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            // instruction3.gameObject.SetActive(true);
            dialogueText.text = instructions[2];
        }
        // else if (collision.gameObject.CompareTag("WinCollection"))
        // {
        //     Debug.Log("Inside Win Col");
        //     _levelInfo.CalculateInterval(DateTime.Now);
        //     collision.gameObject.SetActive(false);
        //     if (nextLevel == "Menu")
        //     {
        //         // when this the last level, show game win panel to the player 
        //         GameWinPanelDisplay();
        //     }
        //     else
        //     {
        //         _loadLevel.LoadScene(nextLevel);
        //         _loadLevel.SendResult(true);
        //     }

        //     // GameWinPanelDisplay();
        // }

        // if (collision.gameObject.CompareTag("FreezeCollection"))
        // {
        //     HandleFreezeEffect(5);
        // }
        if (collision.gameObject.CompareTag("ArrowTile"))
        {
            dialogueText.text = instructions[1];
            // instruction1.gameObject.SetActive(false);    
            // instruction2.gameObject.SetActive(true);  
            collision.gameObject.tag = "Untagged";               
        }
    }


    void UseGhostPowerUp()
    {
        GhostPowerUp();

    }
    void UseSpeedPowerUp()
    {
        speed = 3f;
        StartCoroutine(TurnOffSpeedPowerUp(5f));
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
        }

                    // instruction5.gameObject.SetActive(false);
        dialogueText.text = instructions[5];
    }
    IEnumerator TurnOffSpeedPowerUp(float delay)
    {
        yield return new WaitForSeconds(delay);

        speed = startSpeed;
        // instruction3.gameObject.SetActive(false);
        DisplayGhostPowerup();
        dialogueText.text = instructions[3];
    }

    void DisplayGhostPowerup()
    {
            // instruction4.gameObject.SetActive(true);
            ghost.gameObject.SetActive(true);

    }

    // void GameWinPanelDisplay()
    // {
    //     Time.timeScale = 0;
    //     gameWinPanel.SetActive(true);
    // }

    // private IEnumerator DoorDescend()
    // {
    //     _isDescending = true;
    //     Transform doorTransform = winDoor.transform;
    //     Vector3 initialWallPosition = doorTransform.position;
    //     Vector3 targetPosition = initialWallPosition - Vector3.up * descentDistance;
    //     while (Vector3.Distance(doorTransform.position, targetPosition) > 0.01f)
    //     {
    //         doorTransform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
    //         yield return null;
    //     }
    //     _isDescending = false;
    // }
}