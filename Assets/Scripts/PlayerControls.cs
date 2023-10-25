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

	//Ghost Power up
	public GameObject[] walls;
	private int availablePowerUps = 0;
	public TextMeshProUGUI powerUpText;
	private Rigidbody playerobjectrb;

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

		if (GlobalVariables.LevelInfo == null)
		{
			// called when enter a new level
			GlobalVariables.LevelInfo = new LevelInfo(GlobalVariables.Level, DateTime.Now);
		}
		_levelInfo = GlobalVariables.LevelInfo;
		lastBlockPosition = transform.position;
		playerobjectrb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
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
			GameWinPanelDisplay();
			playerobjectrb.velocity = Vector3.zero;
            playerobjectrb.angularVelocity = Vector3.zero;
            Time.timeScale = 0f;
			//WinText.text = "You Win!!";
		}

		if (collision.gameObject.CompareTag("Coin"))
		{

			AddScore();
			_levelInfo.CoinCollected++;
			Destroy(collision.gameObject);

		}

		if (collision.gameObject.CompareTag("Ghost"))
		{
			Debug.Log("Inside if...");
			AddScore();
			_levelInfo.CoinCollected++;
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
	void RandomizeCoinPositions()
	{
		GameObject[] coinSpawnPoints = GameObject.FindGameObjectsWithTag("CoinSpawnPoint");

		if (coinSpawnPoints.Length >= 3)
		{
			List<GameObject> spawnPointsList = new List<GameObject>(coinSpawnPoints);
			System.Random rng = new System.Random();

			for (int i = 0; i < 2; i++)
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

	void GameWinPanelDisplay()
	{
		gameWinPanel.SetActive(true);
	}
}
