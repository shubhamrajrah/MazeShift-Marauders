using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{

    public float countdownTime = 60f;  // Set your countdown time in seconds here
    private float timer;

    public TextMeshProUGUI timerText;  // Reference to your TextMeshPro text object
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
            // Timer has reached 0, handle timer completion logic here
        }
    }
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
