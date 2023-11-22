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


public class GhostTutorialControls : MonoBehaviour
{
    // Start is called before the first frame update

    public float startSpeed = 2f;
    public float speed = 2f;
    public GameObject[] walls;

    public int availableGhostPowerUps = 0;
    public TextMeshProUGUI ghostPowerUpText;
    private Rigidbody rb;
    public float ghostAbilityDuration = 5f;
    public Boolean ghostOn = false;
    public Boolean ghostPressed = false;
    public Material blue;
    public ProgressBarScript progressBarGhost;
    public Material WallMaterial;
    public string nextLevel;
    public GameObject intermediateGameWinPanel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        //rb.drag = 5f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("Inside if...");
            availableGhostPowerUps++;
            //_levelInfo.GhostCollected++;
            // if(curLevel == 3)
            // {
            //     dialogueTextGhost.text = instruction1[1];
            // }
            
            //ghostPowerUpText.text = availableGhostPowerUps.ToString();
            collision.gameObject.SetActive(false);
            

        }
        if (collision.gameObject.CompareTag("WinCollection"))
        {
            //Debug.Log("Inside Win Col");
            //_levelInfo.CalculateInterval(DateTime.Now);
            collision.gameObject.SetActive(false);
            //IntermediateGameWinPanelDisplay();

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


    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     transform.position += Vector3.left * speed * Time.deltaTime;
        // }

        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     transform.position += Vector3.right * speed * Time.deltaTime;
        // }

        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     transform.position += Vector3.forward * speed * Time.deltaTime;
        // }

        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     transform.position += Vector3.back * speed * Time.deltaTime;
        // }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Alternatively, you can use specific keys
        // float horizontalInput = Input.GetKey(KeyCode.RightArrow) ? 1f : (Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f);
        // float verticalInput = Input.GetKey(KeyCode.UpArrow) ? 1f : (Input.GetKey(KeyCode.DownArrow) ? -1f : 0f);

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Set the velocity of the Rigidbody based on input
        rb.velocity = movement * speed;

        if (Input.GetKeyDown(KeyCode.G) && availableGhostPowerUps > 0) // Check for 'G' press and if power-ups are available
            {
                availableGhostPowerUps--;
                UseGhostPowerUp();
            }
    }
}
