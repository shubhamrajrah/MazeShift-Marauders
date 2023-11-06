using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform linkedPortal;
    public Vector3 exitDirection; // Add this line to your script

    public static bool recentlyTeleported = false;  // Add this

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !recentlyTeleported)
        {
            recentlyTeleported = true;
            // collision.gameObject.transform.position = new Vector3(linkedPortal.position.x, collision.gameObject.transform.position.y,linkedPortal.position.z-0.5f);
            collision.gameObject.transform.position = new Vector3(linkedPortal.position.x + exitDirection.x, collision.gameObject.transform.position.y, linkedPortal.position.z + exitDirection.z);

            // StartCoroutine(ResetTeleportation());
        }
    }
    // private IEnumerator ResetTeleportation()
    // {
    //     yield return new WaitForSeconds(1f); //1sec wait
    //     recentlyTeleported = false;
    // }

}
