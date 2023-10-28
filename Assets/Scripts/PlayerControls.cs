using System;
using System.Collections;
using System.Collections.Generic;
using Analytic;
using Analytic.DTO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class PlayerControls : MonoBehaviour
{
    public float speed = 1.5f;
	//public TextMeshProUGUI WinText;
	// public GameObject coinPrefab;
	// bool coinsSpawned = false;
	// public MazeSetup mazeSetup;
	public TextMeshProUGUI scoreText;
	public int score;
	public string nextLevel;
	public int curLevel;
	public GameObject gameWinPanel;
    
	private LevelInfo _levelInfo;
	private LoadLevel _loadLevel;
	

	//Ghost Power up
	public GameObject[] walls;
	private int availablePowerUps = 0;
	public TextMeshProUGUI powerUpText;


	void Start()
    {
        //WinText.enabled = false;
		score = 0;
		// //Random Coin Genration
		// if (!coinsSpawned)
		// {
		// 	RandomizeCoinPositions();
		// 	coinsSpawned = true;
		// }
		
		
		// set time scale to 1 in case time scale was mistakenly set to 0
		Time.timeScale = 1;
		if (GlobalVariables.LevelInfo == null)
		{
			// called when enter a new level
			GlobalVariables.LevelInfo = new LevelInfo(curLevel, DateTime.Now);
		}

		_loadLevel = gameObject.AddComponent<LoadLevel>();
		_levelInfo = GlobalVariables.LevelInfo;
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

		//Ghost Power up
		if (Input.GetKeyDown(KeyCode.G) && availablePowerUps > 0) // Check for 'G' press and if power-ups are available
		{
			UsePowerUp();
			availablePowerUps--; // decrement the power-up count
			powerUpText.text = "Ghost Power up: " + availablePowerUps.ToString();
		}

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("WinTile"))
		{
			Debug.Log("Win tIle");
			_levelInfo.CalculateInterval(DateTime.Now);
			if (nextLevel == "Menu")
			{
				// when this the last level, show game win panel to the player 
				GameWinPanelDisplay();
			}
			else
			{
				_loadLevel.LoadScene(nextLevel);
				_loadLevel.SendResult(true);
			}
			
			// GameWinPanelDisplay();
		}

		if (collision.gameObject.CompareTag("Ghost"))
		{
			Debug.Log("Inside if...");
			AddScore();
			availablePowerUps++;
			powerUpText.text = "Ghost Power up: " + availablePowerUps.ToString();
			collision.gameObject.SetActive(false);
		}
	}

	void UsePowerUp()
	{
		PowerUp();
	}

	void PowerUp()
	{
		Debug.Log("Inside PowerUp");
		walls = GameObject.FindGameObjectsWithTag("Wall");
		Debug.Log("Fine got walls");
		foreach (GameObject wall in walls)
		{
			Debug.Log("Inside FOR");
			wall.GetComponent<Collider>().isTrigger = true;

		}
		StartCoroutine(TurnOffPowerUp(5f));
	}

	IEnumerator TurnOffPowerUp(float delay)
	{
		yield return new WaitForSeconds(delay);

		// Now, set isTrigger back to false for all walls
		foreach (GameObject wall in walls)
		{
			wall.GetComponent<Collider>().isTrigger = false;
		}
	}

	//Code to generate Coins at Random Locations
	// void RandomizeCoinPositions()
	// {
	// 	GameObject[] coinSpawnPoints = GameObject.FindGameObjectsWithTag("CoinSpawnPoint");
	//
	// 	if (coinSpawnPoints.Length >= 3)
	// 	{
	// 		List<GameObject> spawnPointsList = new List<GameObject>(coinSpawnPoints);
	// 		System.Random rng = new System.Random();
	//
	// 		for (int i = 0; i < 2; i++)
	// 		{
	// 			int randomIndex = rng.Next(spawnPointsList.Count);
	// 			Debug.Log("spawnPointsList.Count" + spawnPointsList.Count);
	// 			GameObject spawnPoint = spawnPointsList[randomIndex];
	//
	// 			Instantiate(coinPrefab, spawnPoint.transform.position, Quaternion.identity);
	// 			spawnPointsList.RemoveAt(randomIndex);
	// 		}
	// 	}
	// 	else
	// 	{
	// 		Debug.LogWarning("Not enough coin spawn points in the scene.");
	// 	}
	// }

	//Code to Add Player Score
	void AddScore()
	{
		score++;
		Debug.Log("Current Score == " + score);
		scoreText.text = "Coins: " + score.ToString();
	}

	void GameWinPanelDisplay()
	{
		Time.timeScale = 0;
		gameWinPanel.SetActive(true);
	}
}
