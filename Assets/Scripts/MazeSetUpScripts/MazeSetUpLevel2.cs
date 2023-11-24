using UnityEngine;

namespace MazeSetUpScripts
{
    public class MazeSetUpLevel2 : MonoBehaviour
    {
        // int[,] _mazeOgLevel2 =
        // {
        //     //maze 1 [alternate switching] - source - block_11_1 
        //     //target - block_7_11
        //     { 3, 0, 3, 1, 3, 0, 3, 1, 3, 0, 3, 0, 3 }, //1
        //     { 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0 }, //2
        //     { 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3, 0, 3 }, //3
        //     { 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0 }, //4
        //     { 3, 0, 3, 0, 3, 0, 3, 1, 3, 1, 3, 1, 3 }, //5
        //     { 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0 }, //6
        //     { 3, 1, 3, 0, 3, 1, 3, 1, 3, 1, 3, 1, 3 }, //7
        //     { 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0 }, //8
        //     { 3, 0, 3, 0, 3, 1, 3, 0, 3, 0, 3, 1, 3 }, //9
        //     { 0, 1, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0 }, //10
        //     { 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3, 1, 3 }, //11
        //     { 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3, 0, 0 }, //12
        //     { 3, 1, 3, 0, 3, 0, 3, 1, 3, 0, 3, 0, 3 } //13
        // };

        int[][,] mazesLevel2 = MazeSetupUtils.mazes_level2;
        private int mazeshiftmode = MazeSetupUtils.mazeshiftmode;
        private int index;
        public Material skull;
        
        int[,] _maze;
        // Flag to ensure we initialize the maze only once
        private bool _mazeInitialized = false;
        int[,] _previewMaze;
        bool _isPreviewing;
        private Rigidbody _playerObjectRb;

        private PlayerControls _pc;
        private float _playerSpeed;
        public GameObject dimmingPanel;
        public GameObject portalhint;
        public GameObject futurehint;
        public GameObject tutorialpanel;
        
        [SerializeField]
        private float switchTime = 5.0f; //
        private float _lastSwitch = 0.0f; //

        //Audio before Maze Change
        [SerializeField]
        private AudioSource tickingSoundSource; 

        [SerializeField]
        private AudioClip tickingSoundClip; 

        void Start()
        {
            _maze = mazesLevel2[0];
            // mazeChangeTimer = mazeChangeInterval; // initialize maze change timer
            GeneratePreviewMaze(); // generate future maze
            _playerObjectRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
            _pc = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
            _playerSpeed = _pc.speed;

            tickingSoundSource.clip = tickingSoundClip;

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
            
            if (Input.GetKey(KeyCode.F))
            {
                PreviewNextMaze();
                _playerObjectRb.velocity = Vector3.zero;
                _playerObjectRb.angularVelocity = Vector3.zero;
                _playerObjectRb.isKinematic = true;
                _pc.speed = 0;
                dimmingPanel.SetActive(true);

                portalhint = GameObject.FindWithTag("PortalHint");
                futurehint = GameObject.FindWithTag("FutureHint");
                tutorialpanel = GameObject.FindWithTag("TutorialPanel");
                if(futurehint){
                    futurehint.SetActive(false);
                    if(!portalhint){
                        tutorialpanel.SetActive(false);
                    }
                }
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
            if (Time.time - _lastSwitch > switchTime - 2.4f && !tickingSoundSource.isPlaying)
            {
                tickingSoundSource.loop = true;
                tickingSoundSource.Play();
            }
            else if (Time.time - _lastSwitch <= switchTime - 2.4f && tickingSoundSource.isPlaying)
            {
                tickingSoundSource.Stop();
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

            int trapX = 9;
            int trapY = 11;
            GameObject trapBlock = GameObject.Find($"block_{trapX}_{trapY}");
            if (trapBlock)
            {
                
                //trapBlock.GetComponent<Renderer>().material.color = new Color(0.6f, 0.3f, 0.0f, 1.0f); 
                trapBlock.GetComponent<Renderer>().material= skull;
               
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
            // _previewMaze = (int[,])_maze.Clone();
            // for (int i = 1; i < _previewMaze.GetLength(0); i++)
            // {
            //     for (int j = 1; j < _previewMaze.GetLength(1); j++)
            //     {
            //         _previewMaze[i, j] = Random.Range(0, 2);
            //     }
            // }

            if(mazeshiftmode==0){
            _previewMaze = mazesLevel2[UnityEngine.Random.Range(0, 10)];
            }
            else
            {
                index=(index+1)%10;
                _previewMaze = mazesLevel2[index];
            }
            

        }
    }
}
