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

	public GameObject speedPowerPrefab;
	bool coinsSpawned = false;
	public TextMeshProUGUI scoreText;
	public MazeSetup mazeSetup;

	private Vector3 lastBlockPosition;
	private int boundariesCrossed = 0;
	private LevelInfo _levelInfo;
	public GameObject gameWinPanel;

	//Ghost Power up
	public GameObject[] walls;
	private int availableGhostPowerUps = 0;
	private int availableSpeedPowerUps = 0;
	public TextMeshProUGUI ghostPowerUpText;
	public TextMeshProUGUI speedPowerUpText;

	List<GameObject> spawnPointsList;

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
		Debug.Log(JsonUtility.ToJson(_levelInfo, true));
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

		

		//Ghost Power up
		if (Input.GetKeyDown(KeyCode.G) && availableGhostPowerUps > 0) // Check for 'G' press and if power-ups are available
		{
			UseGhostPowerUp();
			availableGhostPowerUps--; // decrement the power-up count
			ghostPowerUpText.text = "Ghost AP: " + availableGhostPowerUps.ToString();
		}
		if (Input.GetKeyDown(KeyCode.S) && availableSpeedPowerUps > 0) // Check for 'G' press and if power-ups are available
		{
			UseSpeedPowerUp();
			availableSpeedPowerUps--; // decrement the power-up count
			speedPowerUpText.text = "Speed AP: " + availableSpeedPowerUps.ToString();
		}

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "WinTile")
		{
			Debug.Log("Win tIle");
			_levelInfo.CalculateInterval(DateTime.Now);
			gameWinPanelDisplay();
			//WinText.text = "You Win!!";
		}

		if (collision.gameObject.tag == "Coin")
		{

			AddScore();
			_levelInfo.CoinCollected++;
			Destroy(collision.gameObject);

		}

		if (collision.gameObject.tag == "Ghost")
		{
			Debug.Log("Inside if...");

			AddScore();
			_levelInfo.CoinCollected++;
			availableGhostPowerUps++;
			ghostPowerUpText.text = "Ghost AP: " + availableGhostPowerUps.ToString();
			Destroy(collision.gameObject);


		}

		if (collision.gameObject.tag == "SpeedPowerUp")
		{
			Debug.Log("Inside SpeedPowerUp...");
			Debug.Log("available SpeedPowerUps..."+availableSpeedPowerUps);
			AddScore();
			_levelInfo.CoinCollected++;
			availableSpeedPowerUps++;
			Debug.Log("available SpeedPowerUps after inc..."+availableSpeedPowerUps);
			speedPowerUpText.text = "Speed AP: " + availableSpeedPowerUps.ToString();
			Destroy(collision.gameObject);
		}
	}

	void UseGhostPowerUp()
	{
		ghostPowerUp();
	}
	

	void ghostPowerUp()
	{
		Debug.Log("Inside PowerUp");
		walls = GameObject.FindGameObjectsWithTag("Wall");
		Debug.Log("Fine got walls");
		foreach (GameObject wall in walls)
		{
			// Debug.Log("Inside FOR");
			wall.GetComponent<Collider>().isTrigger = true;

		}
		StartCoroutine(TurnOffGhostPowerUp(5f));
	}

	IEnumerator TurnOffGhostPowerUp(float delay)
	{
		yield return new WaitForSeconds(delay);
		// Now, set isTrigger back to false for all walls
		foreach (GameObject wall in walls)
		{
			wall.GetComponent<Collider>().isTrigger = false;
		}
	}
	void UseSpeedPowerUp()
	{
		speedPowerUp();
	}
	void speedPowerUp()
	{
		Debug.Log("Inside speed PowerUp");
		speed = 3f;
		StartCoroutine(TurnOffSpeedPowerUp(5f));
	}

	IEnumerator TurnOffSpeedPowerUp(float delay)
	{
		yield return new WaitForSeconds(delay);
		speed = 1.5f;

	}

	//Code to generate Coins at Random Locations
	void RandomizeCoinPositions()
	{
		GameObject[] coinSpawnPoints = GameObject.FindGameObjectsWithTag("CoinSpawnPoint");
		GameObject[] speedSpawnPoints = GameObject.FindGameObjectsWithTag("SpeedPowerUp");

		if (coinSpawnPoints.Length >= 3)
		{
			spawnPointsList = new List<GameObject>(coinSpawnPoints);
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


		if (speedSpawnPoints.Length >= 3)
		{
			List<GameObject> speedSpawnPointsList = new List<GameObject>(speedSpawnPoints);
			System.Random rng = new System.Random();

			for (int i = 0; i < 2; i++)
			{
				int randomIndex = rng.Next(speedSpawnPointsList.Count);
				Debug.Log("speedSpawnPointsList.Count" + speedSpawnPointsList.Count);
				GameObject speedSpawnPoint = speedSpawnPointsList[randomIndex];
				Debug.Log("spawn point" + speedSpawnPoint);
				Instantiate(speedPowerPrefab, speedSpawnPoint.transform.position, Quaternion.identity);
				speedSpawnPointsList.RemoveAt(randomIndex);
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
		//
		// StartCoroutine(WaitAndLoadMenu());
	}

	IEnumerator WaitAndLoadMenu()
	{
		yield return new WaitForSecondsRealtime(5);
		Time.timeScale = 1;
		SceneManager.LoadScene("Menu");
	}
}
