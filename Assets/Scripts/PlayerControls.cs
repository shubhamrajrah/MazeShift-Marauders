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
    public Boolean speedOn = false;


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
    private string _keyTextFormat = "Stars: {0}/{1}";
    private int _keyGet = 0;
    private bool _isDescending = false;

    //Ghost Power up
    public GameObject[] walls;
    public int availableGhostPowerUps = 0;
    private int availableSpeedPowerUps = 0;
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
    public Image wallDestroyer;

    //Level 4 - Tutorial
    
    // MazeShiftPowerUp
    public int availableMazeShiftPowerUp = 0;
    public TextMeshProUGUI mazeShiftPowerUpText;

    public bool GameIsWon { get; private set; } = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.drag = 0f;
        
        respawnPosition = transform.position;
        plusFiveSecondsText.gameObject.SetActive(false);
        if (curLevel > 2)
        {
            progressBarGhost.gameObject.SetActive(false);
            progressBarSpeed.gameObject.SetActive(false);
        }
        if (curLevel == 4 || curLevel == 5 || curLevel == 6)
        {
            progressBarWallDestroy.gameObject.SetActive(false);
            wallDestroyer.gameObject.SetActive(false);
        }
        // set time scale to 1 in case time scale was mistakenly set to 0
        Time.timeScale = 1;
        // initialize level info
        GlobalVariables.LevelInfo ??= new LevelInfo(curLevel, DateTime.Now);
        // initialize level track
        GlobalVariables.LevelTrack ??= new LevelTrack(curLevel);
        // track level
        GlobalVariables.Level = curLevel;
        _levelInfo = GlobalVariables.LevelInfo;
        endBlock.GetComponent<Renderer>().material.color = targetUnfinish;
        keyText.text = string.Format(_keyTextFormat, _keyGet, keyNum);
    }

    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Alternatively, you can use specific keys
        // float horizontalInput = Input.GetKey(KeyCode.RightArrow) ? 1f : (Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f);
        // float verticalInput = Input.GetKey(KeyCode.UpArrow) ? 1f : (Input.GetKey(KeyCode.DownArrow) ? -1f : 0f);

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Set the velocity of the Rigidbody based on input
        rb.velocity = movement * speed;

        if (Input.GetKeyDown(KeyCode.S) && availableSpeedPowerUps > 0) // Check for 'G' press and if power-ups are available
        {
            speedOn = true;
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
        if (collision.gameObject.CompareTag("Key"))
        {
            _keyGet++;
            collision.gameObject.SetActive(false);
            _levelInfo.KeyCollected++;
            keyText.text = string.Format(_keyTextFormat, _keyGet, keyNum);
            // New code to activate the gold star in game screen
            Transform starsInGamePlayTransform = GameObject.Find("StarsInGamePlay").transform;
            Transform goldStarsTransform = starsInGamePlayTransform.Find("GoldStarsInGamePlay");

            if (goldStarsTransform != null)
            {
                string goldStarName = "GoldStarGP" + _keyGet;
                Transform goldStarChild = goldStarsTransform.Find(goldStarName);
                if (goldStarChild != null)
                {
                    goldStarChild.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("GoldStar child with name " + goldStarName + " not found in starsInGamePlay!");
                }
            }
            else
            {
                Debug.LogWarning("goldStarsInGamePlay not found in starsInGamePlay!");
            }

            if (_keyGet == keyNum && !_isDescending)
            {
                endBlock.GetComponent<Renderer>().material.color = targetFinish;
                StartCoroutine(DoorDescend());
            }
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
            GameIsWon = true; 
            _levelInfo.CalculateInterval(DateTime.Now);
            // update star number
            GlobalVariables.LevelStars[curLevel] = Math.Max(GlobalVariables.LevelStars[curLevel], _keyGet);
            collision.gameObject.SetActive(false);
            if (nextLevel == "Menu")
            {
                GameWinPanelDisplay();
            }
            else
            {
                GameObject starsingameplay = GameObject.Find("StarsInGamePlay");
                starsingameplay.SetActive(false);
                IntermediateGameWinPanelDisplay();
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
            wallDestroyer.gameObject.SetActive(true);
            _levelInfo.DestructionCollected++;
        } else if (collision.gameObject.CompareTag(("ShiftPower")))
        {
            collision.gameObject.SetActive(false);
            availableMazeShiftPowerUp++;
            _levelInfo.MazeShiftCollected++;
            mazeShiftPowerUpText.text = availableMazeShiftPowerUp.ToString();
        }
        if (collision.gameObject.CompareTag("Wall") && WallDestroyerTouched)
        {
            Vector3 wallPosition = collision.gameObject.transform.position;
            if (wallPosition.y >= 0.5f)
            {
                Debug.Log("destroying high wall---"+wallPosition);
                Vector3 newPosition = new Vector3(wallPosition.x, 0f, wallPosition.z);
                Debug.Log("destroying high new position wall---"+newPosition);
                collision.gameObject.transform.position = newPosition;
                collision.gameObject.SetActive(false);
                WallDestroyerTouched = false;
                 wallDestroyer.gameObject.SetActive(false);
                progressBarWallDestroy.gameObject.SetActive(false);
                
            }
            
        }

        if (collision.gameObject == trapBlock) 
        {
            transform.position = respawnPosition; 
        }
    }
    
    void UseSpeedPowerUp()
    {
        speed = 3f;
        progressBarSpeed.StartProgress(5f);
        StartCoroutine(TurnOffSpeedPowerUp(5f));
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
    void IntermediateGameWinPanelDisplay()
    {
        Time.timeScale = 0;
        intermediateGameWinPanel.SetActive(true);

        Transform goldStarsTransform = intermediateGameWinPanel.transform.Find("Stars/GoldStars");

        foreach (Transform child in goldStarsTransform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 1; i <= _keyGet; i++)
        {
            string goldStarName = "GoldStar" + i;
            Transform goldStarChild = goldStarsTransform.Find(goldStarName);
            if (goldStarChild != null)
            {
                goldStarChild.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("GoldStar child with name " + goldStarName + " not found!");
            }
        }
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
    
}