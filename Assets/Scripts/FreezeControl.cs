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
        Debug.Log(other.gameObject.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("freeze the time");
            gameObject.SetActive(false);
            timerController.FreezeTimer(freezeTime);
        }
    }
}
