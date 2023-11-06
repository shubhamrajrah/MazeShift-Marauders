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


public class PlayerControls : MonoBehaviour
{
    public float startSpeed = 1.5f;
    public float speed = 1.5f;
    public string nextLevel;
    public int curLevel;
    public GameObject gameWinPanel;
    public GameObject timePanel;


    private LevelInfo _levelInfo;
    private LoadLevel _loadLevel;

    // Door Descending 
    public GameObject winDoor;
    public float descentSpeed = 1.0f;
    public float descentDistance = 2.0f;
    private bool _isDescending = false;

    //Ghost Power up
    private int wallsPassedThrough = 0;
    private bool isPowerUpActive;
    public GameObject[] walls;
    public int availableGhostPowerUps = 0;
    private int availableSpeedPowerUps = 0;
    public TextMeshProUGUI ghostPowerUpText;
    public TextMeshProUGUI speedPowerUpText;
    public TextMeshProUGUI plusFiveSecondsText;
    public bool canMove = true;
    public TimerController timerController;

    //Current maze

    public Color timerHighlight = Color.yellow;

    public ProgressBarScript progressBarGhost;
    public ProgressBarScript progressBarSpeed;


    void Start()
    {
        plusFiveSecondsText.gameObject.SetActive(false);
        if (curLevel > 2)
        {
            progressBarGhost.gameObject.SetActive(false);
            progressBarSpeed.gameObject.SetActive(false);
        }
        // set time scale to 1 in case time scale was mistakenly set to 0
        Time.timeScale = 1;
        if (GlobalVariables.LevelInfo == null)
        {
            // called when enter a new level
            GlobalVariables.LevelInfo = new LevelInfo(curLevel, DateTime.Now);
        }

        if (GlobalVariables.LevelTrack == null)
        {
            GlobalVariables.LevelTrack = new LevelTrack(curLevel);
        }
        GlobalVariables.Level = curLevel;
        _loadLevel = gameObject.AddComponent<LoadLevel>();
        _levelInfo = GlobalVariables.LevelInfo;
    }

    void Update()
    {
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

        //Ghost Power up
        //if (Input.GetKeyDown(KeyCode.G) && availableGhostPowerUps > 0) // Check for 'G' press and if power-ups are available
        //{
        //    UseGhostPowerUp();
        //    availableGhostPowerUps--;// decrement the power-up count
        //    _levelInfo.GhostUsed++;
        //    ghostPowerUpText.text = availableGhostPowerUps.ToString();
        //}
        if (Input.GetKeyDown(KeyCode.S) && availableSpeedPowerUps > 0) // Check for 'G' press and if power-ups are available
        {
            UseSpeedPowerUp();
            availableSpeedPowerUps--; // decrement the power-up count
            _levelInfo.SpeedUsed++;
            speedPowerUpText.text = availableSpeedPowerUps.ToString();
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

        // canMove = true; 
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WinTile") && !_isDescending)
        {
            StartCoroutine(DoorDescend());
        }
        else if (collision.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("Inside if...");
            availableGhostPowerUps++;
            _levelInfo.GhostCollected++;
            ghostPowerUpText.text = availableGhostPowerUps.ToString();
            collision.gameObject.SetActive(false);

        }
        else if (collision.gameObject.CompareTag("Speed"))
        {
            Debug.Log("Inside if...");
            availableSpeedPowerUps++;
            _levelInfo.SpeedCollected++;
            speedPowerUpText.text = availableSpeedPowerUps.ToString();
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("WinCollection"))
        {
            Debug.Log("Inside Win Col");
            _levelInfo.CalculateInterval(DateTime.Now);
            collision.gameObject.SetActive(false);
            if (nextLevel == "Menu")
            {
                // when this the last level, show game win panel to the player 
                GameWinPanelDisplay();
            }
            else
            {
                _loadLevel.LoadScene(nextLevel);
                _loadLevel.SendResult(true);
            }
        }
    }

    void UseGhostPowerUp()
    {
        //GhostPowerUp();
        
        walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wall in walls)
        {
            BlockController blockController = wall.gameObject.GetComponent<BlockController>();
            if (blockController != null && blockController.IsUp() == true)
            {
                wall.GetComponent<Collider>().isTrigger = true;

            }
            
        }

        progressBarGhost.StartProgress(5f);

    }
    
    public void DeactivateGhostPowerUp()
    {
        foreach (GameObject wall in walls)
        {
            if (wall != null)
            {
                wall.GetComponent<Collider>().isTrigger = false;
            }
        }
    }
    //private bool IsWallUp(BlockController blockController)
    //{
    //    // You'll need to determine what "up" means for your game
    //    // This is a simple check assuming the wall is "up" if it's above a certain height
    //    return blockController.transform.position.y > blockController.baseHeight;
    //} 
    void UseSpeedPowerUp()
    {
        speed = 3f;
        progressBarSpeed.StartProgress(5f);
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
    }
    IEnumerator TurnOffSpeedPowerUp(float delay)
    {
        yield return new WaitForSeconds(delay);

        speed = startSpeed;
    }

    void GameWinPanelDisplay()
    {
        Time.timeScale = 0;
        gameWinPanel.SetActive(true);
    }

    private IEnumerator DoorDescend()
    {
        _isDescending = true;
        Transform doorTransform = winDoor.transform;
        Vector3 initialWallPosition = doorTransform.position;
        Vector3 targetPosition = initialWallPosition - Vector3.up * descentDistance;
        while (Vector3.Distance(doorTransform.position, targetPosition) > 0.01f)
        {
            doorTransform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
            yield return null;
        }
        _isDescending = false;
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Wall") && isPowerUpActive)
    //    {
    //        PassedThroughWall();
    //    }
    //}
}