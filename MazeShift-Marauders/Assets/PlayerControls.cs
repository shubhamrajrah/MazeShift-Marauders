using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    public float speed = 1.5f;
	public Text WinText;
    void Start()
    {
        WinText.enabled = false;
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
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Win tIle");
		
		if (collision.gameObject.tag == "WinTile")
		{
			Debug.Log("Win tIle");
			WinText.enabled = true;
		}
	}
}
