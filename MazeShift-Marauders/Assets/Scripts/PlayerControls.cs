using Analytics;
using Analytics.DTO;
using UnityEngine;


public class PlayerControls : MonoBehaviour
{
    public float speed = 1.5f;

    void Start()
    {
        // use this way to record data
        // HttpSender.RecordData("Player", new Player("yd", 63));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
}