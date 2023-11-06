using Analytic.DTO;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

namespace MazeSetUpScripts
{
    public class MazeSetUpLevel3 : MonoBehaviour
    {
        //Ghost
        public PlayerControls playercontrols;
        public ProgressBarScript progressBarGhost;
        private List<(int, int)> _redWalls = new List<(int, int)>();
        public GameObject[] walls;
        public Material noWallMaterial;
        public Material WallMaterial;


        int[,] _mazeOgLevel3 =
        {
            { 3, 0, 3, 0, 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3 }, //1
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 3, 1, 1 }, //2
            { 3, 0, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 0, 3 }, //3
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0 }, //4
            { 3, 0, 3, 1, 3, 0, 3, 1, 3, 0, 3, 1, 3, 1, 3 }, //5
            { 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 3, 0, 0 }, //6
            { 3, 0, 3, 0, 3, 0, 3, 0, 3, 0, 3, 0, 3, 1, 3 }, //7
            { 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 3, 0, 1 }, //8
            { 3, 0, 3, 0, 3, 1, 3, 0, 3, 1, 3, 0, 3, 1, 3 }, //9
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0 }, //10
            { 3, 0, 3, 1, 3, 1, 3, 1, 3, 0, 3, 1, 3, 0, 3 }, //11
            { 3, 1, 3, 0, 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 0 }, //12
            { 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 3, 0, 3 }, //13
            { 3, 0, 3, 1, 3, 1, 3, 1, 3, 1, 3, 0, 3, 1, 1 }, //14
            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 3, 1, 3 } //15
        };

        int[,] _maze;

        // Flag to ensure we initialize the maze only once
        private bool _mazeInitialized = false;
        int[,] _previewMaze;
        bool _isPreviewing;
        private Rigidbody _playerObjectRb;

        private PlayerControls _pc;
        private float _playerSpeed;
        public GameObject dimmingPanel;

        [SerializeField] private float switchTime = 10.0f; //
        private float _lastSwitch = 0.0f; //
        private LevelInfo _levelInfo;

        void Start()
        {
            _maze = _mazeOgLevel3;
            // mazeChangeTimer = mazeChangeInterval; // initialize maze change timer
            GeneratePreviewMaze(); // generate future maze
            _playerObjectRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
            _pc = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
            _playerSpeed = _pc.speed;
        }
        void GhostAbilty(int[,] _maze)
        {
            List<(int, int)> wallCoordinates = new List<(int, int)>();
            Debug.Log("maze lenght cols " + _maze.GetLength(0));
            Debug.Log("maze lenght row " + _maze.GetLength(1));
            playercontrols.availableGhostPowerUps--;
            playercontrols.ghostPowerUpText.text = playercontrols.availableGhostPowerUps.ToString();
            for (int i = 1; i <= _maze.GetLength(0); i++)
            {
                for (int j = 1; j <= _maze.GetLength(1); j++)
                {
                    if (_maze[i - 1, j - 1] == 1)
                    {
                        Debug.Log("current block one- " + $"block_{i}_{j}");
                        GameObject wallGameObject = GameObject.Find($"block_{i}_{j}");
                        if (wallGameObject != null && wallGameObject.CompareTag("Wall"))
                        {
                            Renderer renderer = wallGameObject.GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material = WallMaterial;
                            }
                            Debug.Log("Inside if" + i + " " + j);

                            Debug.Log(" YO  Inside if" + i + " " + j);
                            wallCoordinates.Add((i, j));
                            wallGameObject.GetComponent<Collider>().isTrigger = true;

                        }
                    }
                }
            }
            _redWalls.Clear();
            // Select 4 unique wall blocks randomly if there are at least 4 walls
            if (wallCoordinates.Count >= 4)
            {
                Debug.Log("total walls- " + wallCoordinates.Count);
                List<(int, int)> selectedWalls = new List<(int, int)>();
                for (int i = 0; i < wallCoordinates.Count / 2; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, wallCoordinates.Count);
                    (int, int) randomWall = wallCoordinates[randomIndex];
                    wallCoordinates.RemoveAt(randomIndex);
                    _redWalls.Add(randomWall);
                }
                ChangeColorToRed(_redWalls);

            }
            walls = GameObject.FindGameObjectsWithTag("Wall");
            StartCoroutine(TurnOffGhostPowerUp(5f));
        }
        void ChangeColorToRed(List<(int, int)> coordinates)
        {
            foreach (var (row, col) in coordinates)
            {
                Debug.Log("Inside for - red" + row + " " + col);
                GameObject block = GetBlockGameObjectAt(row, col);
                if (block != null)
                {
                    Renderer renderer = block.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        //renderer.material.color = Color.red;
                        renderer.material = noWallMaterial;
                    }
                    block.GetComponent<Collider>().isTrigger = false;
                }
            }
        }

        void ChangeColorToBlack(List<(int, int)> coordinates)
        {
            foreach (var (row, col) in coordinates)
            {
                Debug.Log("Inside for - black" + row + " " + col);
                GameObject block = GetBlockGameObjectAt(row, col);
                if (block != null)
                {
                    Renderer renderer = block.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = WallMaterial;
                    }
                }
            }
        }
        GameObject GetBlockGameObjectAt(int row, int col)
        {
            string blockName = $"block_{row}_{col}";
            Debug.Log("BLOCK++" + blockName);
            GameObject block = GameObject.Find(blockName);
            return block;
        }
        IEnumerator TurnOffGhostPowerUp(float delay)
        {
            yield return new WaitForSeconds(delay);
            // Now, set isTrigger back to false for all walls
            foreach (GameObject wall in walls)
            {
                wall.GetComponent<Collider>().isTrigger = false;
                wall.GetComponent<Renderer>().material = WallMaterial;
            }
        }

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space) && !mazeInitialized)
            if (!_mazeInitialized && (Input.GetKey(KeyCode.UpArrow) ||
                                      Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                                      Input.GetKey(KeyCode.RightArrow)))
            {
                DisplayMaze(_maze);
                _mazeInitialized = true; // Ensure we don't re-initialize if space is pressed again
            }
            // Initalize the maze

            if (Input.GetKey(KeyCode.P))
            {
                PreviewNextMaze();
                _playerObjectRb.velocity = Vector3.zero;
                _playerObjectRb.angularVelocity = Vector3.zero;
                _playerObjectRb.isKinematic = true;
                _pc.speed = 0;
                dimmingPanel.SetActive(true);
                GlobalVariables.LevelInfo.FutureSightUsedTime += Time.deltaTime;
            }
            else if (_isPreviewing)
            {
                RevertToCurrentMaze();
                _pc.speed = _playerSpeed;
                _playerObjectRb.isKinematic = false;
                dimmingPanel.SetActive(false);
            }

            if (Time.time - _lastSwitch > switchTime)
            {
                _lastSwitch = Time.time;
                SetMazeToPreview();
                _previewMaze = null;
                GeneratePreviewMaze();
            }
            if (Input.GetKeyDown(KeyCode.G) && playercontrols.availableGhostPowerUps > 0) // Check for 'G' press and if power-ups are available
            {
                Debug.Log("Inside iff");
                GhostAbilty(_maze);
                progressBarGhost.StartProgress(5f);
                // DeactivateGhostPowerUp();
                //GhostPowerUp();
            }
        }

        void PreviewNextMaze()
        {
            _isPreviewing = true;
            DisplayMaze(_previewMaze);
        }

        void RevertToCurrentMaze()
        {
            _isPreviewing = false;
            DisplayMaze(_maze);
        }

        void DisplayMaze(int[,] mazeToDisplay)
        {
            for (int j = 1; j <= mazeToDisplay.GetLength(0); j++)
            {
                for (int i = 1; i <= mazeToDisplay.GetLength(1); i++)
                {
                    // Fetch the block based on its name
                    GameObject block = GameObject.Find($"block_{j}_{i}");
                    // Debug.Log($"Processing block_{j}_{i}"+"Maze VAlue - " + maze[j-1, i-1]);
                    if (block)
                    {
                        BlockController controller = block.GetComponent<BlockController>();
                        if (controller)
                        {
                            controller.AdjustBlock(mazeToDisplay[j - 1, i - 1]);
                        }
                    }
                }
            }
            int trapX = 3;
            int trapY = 11;
            GameObject trapBlock = GameObject.Find($"block_{trapX}_{trapY}");
            if (trapBlock)
            {

                trapBlock.GetComponent<Renderer>().material.color = new Color(0.6f, 0.3f, 0.0f, 1.0f);


                _pc.trapBlock = trapBlock;
            }
        }

        void SetMazeToPreview()
        {
            _maze = _previewMaze; // set current maze as future maze
            DisplayMaze(_maze);
        }

        void GeneratePreviewMaze()
        {
            _previewMaze = (int[,])_maze.Clone();
            for (int i = 1; i < _previewMaze.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < _previewMaze.GetLength(1) - 1; j++)
                {
                    _previewMaze[i, j] = UnityEngine.Random.Range(0, 2);
                }
            }
        }

    }

    
}
