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

     public GameObject timePanel;
    public GameObject gameWinPanel;


    public GameObject[] walls;

     public TextMeshProUGUI plusFiveSecondsText;
     public bool canMove = true;
     public TimerController timerController;

     public Color timerHighlight = Color.yellow;

 
    public GameObject ghost;
    public GameObject freeze;

    public TextMeshProUGUI timer;
    public TextMeshProUGUI timer1;
    private string[] instructions = {
        "Use Arrow Keys to move Player",
        "Collect Speed Power up to increase speed",
        "Press S to increase Speed",
        "Collect Ghost Power up to walk through walls",
        "Press G to walk through wall",
        "Press F to view the Future Maze",
        "Now you can view the Future Maze anytime",
        "Collect button to buy more time",
        "Now reach the win Tile in time"

    };
      public Text dialogueText;
    [SerializeField] private GameObject targetBlock;
    public TextMeshProUGUI futureText;
    public GameObject TutorialPanel;

    void Start()
    {
        timer.gameObject.SetActive(false);
        timer1.gameObject.SetActive(false);
        dialogueText.text = instructions[0];
        freeze.gameObject.SetActive(false);
        futureText.gameObject.SetActive(true);
        ghost.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        // futureText.gameObject.SetActive(true);
        if (!canMove) return;
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
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            UseGhostPowerUp();
        }
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            UseSpeedPowerUp();

            
        }
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            //StartCoroutine(WaitForFunction());
        }
    }
    IEnumerator WaitForFunction()
    {
        yield return new WaitForSeconds(1);
        timer.gameObject.SetActive(true);
        timer1.gameObject.SetActive(true);
        freeze.gameObject.SetActive(true);
        dialogueText.text = instructions[7];
        StartCoroutine(WaitForFunctionWinTile());
    }
    IEnumerator WaitForFunctionWinTile()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("HELLO!!");
        dialogueText.text = instructions[8];
        targetBlock.GetComponent<Renderer>().material.color = Color.green;
        targetBlock.gameObject.tag = "WinTile";
    }
    public void HandleFreezeEffect(int freezeTime)
    {
        StartCoroutine(FreezePlayerRoutine(freezeTime));
    }

    private IEnumerator FreezePlayerRoutine(int freezeTime)
    {
        canMove = false; 

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
        if (collision.gameObject.CompareTag("WinTile"))
        {
            GameWinPanelDisplay();
        }
        if (collision.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("Inside if...");
           
            collision.gameObject.SetActive(false);
            dialogueText.text = instructions[4];

        }
         if (collision.gameObject.CompareTag("Speed"))
        {
            Debug.Log("Inside if...");
            collision.gameObject.SetActive(false);
            dialogueText.text = instructions[2];
        }
       

        if (collision.gameObject.CompareTag("FreezeCollection"))
        {
            collision.gameObject.SetActive(false);
            HandleFreezeEffect(2);
            //StartCoroutine(WaitForFunctionAfterTimer());

        }
        if (collision.gameObject.CompareTag("ArrowTile"))
        {
            dialogueText.text = instructions[1];
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
        StartCoroutine(WaitForFunction());
    }
    IEnumerator TurnOffSpeedPowerUp(float delay)
    {
        yield return new WaitForSeconds(delay);

        speed = startSpeed;
        DisplayGhostPowerup();
        dialogueText.text = instructions[3];
    }

    void DisplayGhostPowerup()
    {
        ghost.gameObject.SetActive(true);
    }

    void GameWinPanelDisplay()
    {
        Time.timeScale = 0;
        TutorialPanel.gameObject.SetActive(false);
        gameWinPanel.SetActive(true);
        dialogueText.gameObject.SetActive(false);
        futureText.gameObject.SetActive(false);

        timer.gameObject.SetActive(false);
    }
}