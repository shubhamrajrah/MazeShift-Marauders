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


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;

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
           
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Speed"))
        {
            Debug.Log("Inside if...");
            availableSpeedPowerUps++;
            //_levelInfo.SpeedCollected++;
            //speedPowerUpText.text = availableSpeedPowerUps.ToString();
            collision.gameObject.SetActive(false);

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
