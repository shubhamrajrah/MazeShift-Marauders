using System.Collections.Generic;
using UnityEngine;

namespace MazeSetUpScripts
{
    public class MazeSetupLevel1 : MonoBehaviour
    {
        List<int[,]> _maze = new List<int[,]>();
        int _curIdx;
         [SerializeField] private MazeProgressBar progressBar; // Reference to the MazeProgressBar
        [SerializeField] private float progressBarDuration = 5.0f; // Duration for the progress bar sequence


        int[,] _mazeLeve1Alt1 =
        {
            //maze 1 [alternate switching] - source - block_11_1 
            //target - block_7_11
            { 3, 0, 3, 1, 3, 0, 3, 1, 3, 0, 3 }, //1
            { 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1 }, //2
            { 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3 }, //3
            { 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0 }, //4
            { 3, 0, 3, 0, 3, 0, 3, 1, 3, 1, 3 }, //5
            { 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0 }, //6
            { 3, 1, 3, 0, 3, 1, 3, 1, 3, 1, 3 }, //7
            { 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1 }, //8
            { 3, 0, 3, 0, 3, 1, 3, 0, 3, 0, 3 }, //9
            { 0, 1, 0, 0, 1, 0, 1, 1, 1, 0, 0 }, //10
            { 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3 } //11
        };

        int[,] _mazeLevelAlt2 =
        {
            // //maze2 [alternate switching]
            //maze 1 [alternate switching]
            { 3, 0, 3, 1, 3, 0, 3, 0, 3, 0, 3 }, //1
            { 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1 }, //2
            { 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3 }, //3
            { 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0 }, //4
            { 3, 0, 3, 1, 3, 0, 3, 1, 3, 1, 3 }, //5
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0 }, //6
            { 3, 0, 3, 0, 3, 1, 3, 1, 3, 1, 3 }, //7
            { 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1 }, //8
            { 3, 0, 3, 0, 3, 0, 3, 0, 3, 1, 3 }, //9
            { 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0 }, //10
            { 3, 0, 3, 1, 3, 0, 3, 1, 3, 0, 3 } //11
        };
        
        

        [SerializeField] private float switchTime = 5.0f;

        // Flag to ensure we initialize the maze only once
        private bool _mazeInitialized = false;

        private float _lastSwitch = 0.0f;

        void Start()
        {
            _maze.Add(_mazeLeve1Alt1);
            _maze.Add(_mazeLevelAlt2);
            progressBar.StartProgressBarSequence(progressBarDuration);

        }

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space) && !mazeInitialized)
            if (!_mazeInitialized && (Input.GetKey(KeyCode.UpArrow) ||
                                      Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                                      Input.GetKey(KeyCode.RightArrow)))
            {
                _curIdx = 0;
                InitializeMaze();
                _mazeInitialized = true; // Ensure we don't re-initialize if space is pressed again
            }

            if (Time.time - _lastSwitch > switchTime)
            {
                ToggleMaze();
                _lastSwitch = Time.time;
                progressBar.StartProgressBarSequence(progressBarDuration);
            }
        }

        void ToggleMaze()
        {
            _curIdx = (_curIdx + 1) % _maze.Count;
            InitializeMaze();
        }

        void InitializeMaze()
        {
            for (int j = 1; j <= _maze[_curIdx].GetLength(0); j++)
            {
                for (int i = 1; i <= _maze[_curIdx].GetLength(1); i++)
                {
                    // Fetch the block based on its name
                    GameObject block = GameObject.Find($"block_{j}_{i}");
                    // Debug.Log($"Processing block_{j}_{i}"+"Maze VAlue - " + maze[j-1, i-1]);
                    if (block)
                    {
                        BlockController controller = block.GetComponent<BlockController>();
                        if (controller)
                        {
                            controller.AdjustBlock(_maze[_curIdx][j - 1, i - 1]);
                        }
                    }
                }
            }
        }
    }
}