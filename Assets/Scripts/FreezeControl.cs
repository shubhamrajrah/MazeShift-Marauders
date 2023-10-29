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
        }
    }
    // private IEnumerator FreezePlayerRoutine(PlayerControls player)
    // {
        
    //     timerController.FreezeTimer(freezeTime);
    //     player.plusFiveSecondsText.gameObject.SetActive(true);
    //     yield return new WaitForSeconds(1);
    //     player.plusFiveSecondsText.gameObject.SetActive(false);

       
    //     player.freezeText.gameObject.SetActive(true);
    //     player.canMove = false;
    //     yield return new WaitForSeconds(2);

      
    //     player.freezeText.gameObject.SetActive(false);
    //     player.canMove = true;
    // }
}
