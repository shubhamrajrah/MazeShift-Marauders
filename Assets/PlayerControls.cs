using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    public float speed = 1.5f;
	public Text WinText;
	public int score;
	public GameObject coinPrefab;
	bool coinsSpawned = false;
	public TextMeshProUGUI scoreText;

	void Start()
    {
        WinText.enabled = false;
		score = 0;
		//Random Coin Genration
		if (!coinsSpawned)
		{
			RandomizeCoinPositions();
			coinsSpawned = true;
		}
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
		Debug.Log("Score"+score);

	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Win tIle");
		
		if (collision.gameObject.tag == "WinTile")
		{
			Debug.Log("Win tIle");
			WinText.enabled = true;
		}
		if (collision.gameObject.tag == "Coin")
		{

			AddScore();
			Destroy(collision.gameObject);

		}
	}

	//Code to generate Coins at Random Locations
	void RandomizeCoinPositions()
	{
		GameObject[] coinSpawnPoints = GameObject.FindGameObjectsWithTag("CoinSpawnPoint");

		if (coinSpawnPoints.Length >= 3)
		{
			List<GameObject> spawnPointsList = new List<GameObject>(coinSpawnPoints);
			System.Random rng = new System.Random();

			for (int i = 0; i < 8; i++)
			{
				int randomIndex = rng.Next(spawnPointsList.Count);
				Debug.Log("spawnPointsList.Count" + spawnPointsList.Count);
				GameObject spawnPoint = spawnPointsList[randomIndex];

				Instantiate(coinPrefab, spawnPoint.transform.position, Quaternion.identity);
				spawnPointsList.RemoveAt(randomIndex);
			}
		}
		else
		{
			Debug.LogWarning("Not enough coin spawn points in the scene.");
		}
	}

	//Code to Add Player Score
	void AddScore()
	{
		score++;
		Debug.Log("Current Score == " + score);
		scoreText.text = score.ToString();
	}
}
