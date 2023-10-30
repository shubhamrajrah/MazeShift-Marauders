using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeControl : MonoBehaviour
{
    [SerializeField] 
    private TimerController timerController;
    [SerializeField] 
    private int freezeTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControls playerControls = other.GetComponent<PlayerControls>();

            if (playerControls != null)
            {
                timerController.AddTime(freezeTime);
                gameObject.SetActive(false);
                playerControls.HandleFreezeEffect(freezeTime);
            }

            if (GlobalVariables.LevelInfo != null)
            {
                // add freeze time to level info
                GlobalVariables.LevelInfo.FreezeTime += freezeTime;
            }
        }
    }
}
