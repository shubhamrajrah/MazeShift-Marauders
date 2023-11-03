using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform linkedPortal;
    public static bool recentlyTeleported = false;  // Add this

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !recentlyTeleported)
        {
            recentlyTeleported = true;
            // collision.gameObject.transform.position = linkedPortal.position - linkedPortal.forward * 1f;
            collision.gameObject.transform.position = new Vector3(linkedPortal.position.x + 1f, collision.gameObject.transform.position.y,linkedPortal.position.z);

        }
    }

}
