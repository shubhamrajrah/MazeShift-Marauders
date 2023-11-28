using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScriptTutorial : MonoBehaviour
{
    public Transform linkedPortal;
    public Vector3 exitDirection; // Add this line to your script

    public GameObject portalhint;
    public GameObject futurehint;
    public GameObject tutorialpanel;
    public static bool recentlyTeleported = false;  // Add this
    private PlayerTutorial1 _pc;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !recentlyTeleported)
        {
            recentlyTeleported = true;
            // collision.gameObject.transform.position = new Vector3(linkedPortal.position.x, collision.gameObject.transform.position.y,linkedPortal.position.z-0.5f);
            collision.gameObject.transform.position = new Vector3(linkedPortal.position.x + exitDirection.x, collision.gameObject.transform.position.y, linkedPortal.position.z + exitDirection.z);
           
            StartCoroutine(ResetTeleportation());
            StartCoroutine(FinishRemarks());
            //GlobalVariables.LevelInfo.Transported++;
        }
        // portalhint = GameObject.FindWithTag("PortalHint");
        // futurehint = GameObject.FindWithTag("FutureHint");
        // tutorialpanel = GameObject.FindWithTag("TutorialPanel");
        // if(portalhint){
        //     portalhint.SetActive(false);
        //     if(!futurehint){
        //         tutorialpanel.SetActive(false);
        //     }
        // }
    }
    private IEnumerator ResetTeleportation()
    {
        yield return new WaitForSeconds(1f); //1sec wait
        recentlyTeleported = false;
    }

     private IEnumerator FinishRemarks()
    {
        yield return new WaitForSeconds(2f); //1sec wait
        _pc.instructionText.text = "Onward to Glory!!";
       // StartCoroutine(DisablePanel());
    }

    // private IEnumerator DisablePanel()
    // {
        
    // }

}
