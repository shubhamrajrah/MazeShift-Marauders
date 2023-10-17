using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class TimerController : MonoBehaviour
{

    public float countdownTime = 60f;  // Set your countdown time in seconds here
    private float timer;

    public TextMeshProUGUI timerText;  // Reference to your TextMeshPro text object
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        timer = countdownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            timer = 0;
            UpdateTimerText();
            GameOver();
            
        }
    }
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);

        Time.timeScale = 0;

        StartCoroutine(WaitAndLoadMenu());
    }

    IEnumerator WaitAndLoadMenu()
    {
        yield return new WaitForSecondsRealtime(3); 
        Time.timeScale = 1; 
        SceneManager.LoadScene("Menu"); 
    }
}
