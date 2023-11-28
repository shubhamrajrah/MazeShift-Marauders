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
    public PlayerTutorial1 _pc;
    public GameObject winCollection; 
    public GameObject trophyImage;

     

    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player") && !recentlyTeleported)
    {
        Vector3 newPosition = linkedPortal.position + exitDirection; // Calculate the new position

        // Check if the newPosition is not overlapping with the maze geometry
        if (!Physics.CheckSphere(newPosition, 0.5f)) // Adjust the radius as needed
        {
            recentlyTeleported = true;
            collision.gameObject.transform.position = newPosition;

            StartCoroutine(ResetTeleportation());
            StartCoroutine(FinishRemarks());
        }
    }
}

    private IEnumerator ResetTeleportation()
    {
        yield return new WaitForSeconds(1f); //1sec wait
        recentlyTeleported = false;
    }

     private IEnumerator FinishRemarks()
    {
        yield return new WaitForSeconds(1f); //1sec wait
        _pc.PortalImage.gameObject.SetActive(false);
        _pc.instructionText.text = "Collect to complete tutorial!";
        trophyImage.gameObject.SetActive(true);
        winCollection.gameObject.SetActive(true);
       // StartCoroutine(DisablePanel());
    }

    // private IEnumerator DisablePanel()
    // {
        
    // }

}
