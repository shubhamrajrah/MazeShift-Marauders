using System;
using Analytic.DTO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MazeSetUpScripts
{
    public class MazeSetUpLevel6 : MonoBehaviour
    {
        int[,] _mazeOgLevel6 =
        {
            { 3, 0, 3, 0, 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3, 1, 3, 1, 3, 0 }, //1
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 3, 1, 1, 0, 1, 3, 1, 3 }, //2
            { 3, 0, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 0, 3, 0, 3, 1, 3, 0 }, //3
            { 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 1, 1, 3, 1, 3 }, //4
            { 3, 0, 3, 1, 3, 0, 3, 1, 3, 0, 3, 1, 3, 1, 3, 0, 3, 1, 3, 0 }, //5
            { 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 3, 0, 0, 1, 1, 3, 1, 3 }, //6
            { 3, 0, 3, 0, 3, 0, 3, 0, 3, 0, 3, 0, 0, 1, 3, 0, 3, 1, 3, 0 }, //7
            { 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 3, 0, 1, 3, 1, 3, 1, 3 }, //8
            { 3, 0, 3, 0, 3, 1, 3, 1, 3, 1, 3, 3, 0, 3, 1, 3, 0, 3, 0, 3 }, //9
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 1, 3, 0 }, //10
            { 3, 0, 3, 1, 3, 1, 3, 1, 3, 0, 3, 1, 3, 0, 3, 3, 3, 1, 3, 0 }, //11
            { 3, 1, 3, 0, 3, 1, 1, 3, 1, 3, 0, 3, 1, 0, 0, 0, 3, 1, 3, 1 }, //12
            { 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 3, 0, 3, 3, 3, 1, 3, 0 }, //13
            { 3, 0, 3, 1, 3, 1, 3, 1, 3, 1, 3, 0, 3, 1, 1, 0, 3, 1, 3, 0 }, //14
            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 3, 3, 3, 1, 3, 0 }, //15
            { 3, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 3, 0, 1, 1, 3, 1, 3, 0 }, //16
            { 3, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 3, 0, 1, 1, 1, 3, 1, 3 }, //17
            { 3, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 3, 0, 1, 1, 1, 3, 1, 3 }, //18
            { 3, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 3, 0, 1, 1, 3, 1, 3, 0 }, //19
            { 3, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 3, 0, 1, 1, 1, 3, 1, 3 }, //20
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 1, 3, 1, 3 } //21
            
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

        [SerializeField] private float switchTime = 5.0f; //
        private float _lastSwitch = 0.0f; //
        private LevelInfo _levelInfo;

        void Start()
        {
            _maze = _mazeOgLevel6;
            // mazeChangeTimer = mazeChangeInterval; // initialize maze change timer
            GeneratePreviewMaze(); // generate future maze
            _playerObjectRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
            _pc = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
            _playerSpeed = _pc.speed;
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
        }

        void SetMazeToPreview()
        {
            _maze = _previewMaze; // set current maze as future maze
            DisplayMaze(_maze);
        }

        void GeneratePreviewMaze()
        {
            _previewMaze = (int[,])_maze.Clone();
            for (int i = 1; i < _previewMaze.GetLength(0); i++)
            {
                for (int j = 1; j < _previewMaze.GetLength(1); j++)
                {
                    _previewMaze[i, j] = Random.Range(0, 2);
                }
            }
        }

    }
}
