using System;
using System.Collections;
using System.Collections.Generic;
using Analytic.DTO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class TimerController : MonoBehaviour
{

    public float countdownTime = 60f;  // Set your countdown time in seconds here
    private float _timer;
    private float _freezeTime = 0f;
    private TimerState _state;

    public TextMeshProUGUI timerText;  // Reference to your TextMeshPro text object
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    public GameObject tutorialpanel;

    void Start()
    {
        _timer = countdownTime;
        _state = TimerState.Normal;
        tutorialpanel = GameObject.FindWithTag("TutorialPanel");
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case TimerState.Freeze:
                _freezeTime -= Time.deltaTime;
                if (_freezeTime <= 0)
                {
                    UnfreezeTimer();
                }
                break;
            case TimerState.Normal:
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;
                }
                else
                {
                    _timer = 0;
                    _state = TimerState.TimeIsUp;
                }
                UpdateTimerText();
                break;
            case TimerState.TimeIsUp:
                GameOver();
                break;
            case TimerState.Hold:
                // hold the time and do nothing
                break;
            default:
                Debug.Log("unknown state");
                break;
        }
        // if (_timer > 0)
        // {
        //     _timer -= Time.deltaTime;
        //     UpdateTimerText();
        // }
        // else
        // {
        //     // LevelInfo levelInfo = GlobalVariables.LevelInfo;
        //     // if (levelInfo != null)
        //     // {
        //     //     levelInfo.IsSuccess = false;
        //     //     levelInfo.CalculateIntervalAndSend(DateTime.Now);
        //     //     GlobalVariables.LevelInfo = null;
        //     // }
        //     _timer = 0;
        //     UpdateTimerText();
        //     GameOver();
        //     
        // }
        UpdateTimerText();
    }
    public void UnfreezeTimer()
    {
        if (_state == TimerState.Freeze)
        {
            _state = TimerState.Normal;
            
            _timer -= Time.deltaTime;
        }
    }
    public void AddTime(float timeToAdd)
    {
        _timer += timeToAdd;
        UpdateTimerText(); 
    }
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(_timer / 60f);
        int seconds = Mathf.FloorToInt(_timer % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        if (tutorialpanel)
        {
            tutorialpanel.SetActive(false);
        }

    }

    public void FreezeTimer(float time)
    {
        _state = TimerState.Freeze;
        _freezeTime += time;
    }

    private enum TimerState
    {
        Normal, Freeze, TimeIsUp, Hold
    }
}
