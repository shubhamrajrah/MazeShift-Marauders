using System;
using System.Collections;
using System.Collections.Generic;
using Analytic;
using Analytic.DTO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerControls : MonoBehaviour
{
    public float speed = 1.5f;
	//public TextMeshProUGUI WinText;
	public int score;
	public GameObject coinPrefab;
	bool coinsSpawned = false;
	public TextMeshProUGUI scoreText;
	public MazeSetup mazeSetup;

	private Vector3 lastBlockPosition;
	private int boundariesCrossed = 0;
	private LevelInfo _levelInfo;
	public GameObject gameWinPanel;


	void Start()
    {
        //WinText.enabled = false;
		score = 0;
		//Random Coin Genration
		if (!coinsSpawned)
		{
			RandomizeCoinPositions();
			coinsSpawned = true;
		}

		GlobalVariables.LevelInfo = new LevelInfo(GlobalVariables.Level, DateTime.Now);
		_levelInfo = GlobalVariables.LevelInfo;
		lastBlockPosition = transform.position;
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
		//Debug.Log("Score"+score);

		Vector3 currentBlockPosition = new Vector3(Mathf.Floor(transform.position.x), 0, Mathf.Floor(transform.position.z));

		if (currentBlockPosition != lastBlockPosition)
		{
			boundariesCrossed++;
			lastBlockPosition = currentBlockPosition;
		}

		

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "WinTile")
		{
			Debug.Log("Win tIle");
			_levelInfo.IsSuccess = true;
			_levelInfo.CalculateIntervalAndSend(DateTime.Now);
			gameWinPanelDisplay();
			//WinText.text = "You Win!!";
		}
		if (collision.gameObject.tag == "Coin")
		{

			AddScore();
			_levelInfo.CoinCollected++;
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
		scoreText.text = "Coins: " + score.ToString();
	}

	void gameWinPanelDisplay()
	{
		gameWinPanel.SetActive(true);

		Time.timeScale = 0;

		StartCoroutine(WaitAndLoadMenu());
	}

	IEnumerator WaitAndLoadMenu()
	{
		yield return new WaitForSecondsRealtime(5);
		Time.timeScale = 1;
		SceneManager.LoadScene("Menu");
	}
}