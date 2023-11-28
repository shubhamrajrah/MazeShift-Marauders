using UnityEngine;
using System.Collections;


namespace MazeSetUpScripts
{
   public class Maze2Setup : MonoBehaviour
   {
       int[,] _mazeOgLevel2 =
       {
           //maze 1 [alternate switching] - source - block_11_1
           //target - block_7_11
           { 3, 0, 3, 1, 3, 0, 3, 1, 3}, //1
           { 0, 1, 0, 0, 0, 0, 1, 1, 0}, //2
           { 3, 1, 3, 0, 3, 0, 3, 1, 3 }, //3
           { 0, 0, 1, 0, 1, 0, 1, 1, 0 }, //4
           { 3, 0, 3, 0, 3, 0, 3, 1, 3}, //5
           { 0, 1, 0, 0, 1, 1, 0, 1, 0 }, //6
           { 3, 1, 3, 0, 3, 1, 3, 1, 3}, //7
           { 1, 0, 1, 0, 0, 1, 0, 0, 0}, //8
           { 3, 0, 3, 0, 3, 1, 3, 0, 3 }, //9
           { 0, 1, 0, 0, 1, 0, 1, 1, 1 }, //10
           { 3, 1, 3, 0, 3, 0, 3, 1, 3 } //11
       };
      
       int[,] _maze;
       // Flag to ensure we initialize the maze only once
       private bool _mazeInitialized = false;
       int[,] _previewMaze;
       bool _isPreviewing;
       private Rigidbody _playerObjectRb;


       private PlayerTutorial1 _pc;
       private float _playerSpeed;
      
       [SerializeField]
       private float switchTime = 5.0f; //
               public GameObject dimmingPanel;


       private float _lastSwitch = 0.0f; //


private bool _firstFKeyPressed = false;
       private bool _fKeyReleased = false;    


       void Start()
       {
           _maze = _mazeOgLevel2;
           // mazeChangeTimer = mazeChangeInterval; // initialize maze change timer
           GeneratePreviewMaze(); // generate future maze
           _playerObjectRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
           _pc = GameObject.FindWithTag("Player").GetComponent<PlayerTutorial1>();
           _playerSpeed = _pc.speed;
           //targetBlock.GetComponent<Renderer>().material.color = Color.green;
            _pc.portal1.gameObject.SetActive(false);
         _pc.portal2.gameObject.SetActive(false);
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
          
           if (Input.GetKey(KeyCode.F)  && _pc.futureFlag )
           {
               PreviewNextMaze();
               _playerObjectRb.velocity = Vector3.zero;
               _playerObjectRb.angularVelocity = Vector3.zero;
               _playerObjectRb.isKinematic = true;
               _pc.speed = 0;
               dimmingPanel.SetActive(true);
               if (!_firstFKeyPressed)
               {
                   _firstFKeyPressed = true; // Indicate that 'F' has been pressed for the first time
                   StartCoroutine(FirstTimeTeleportFeature()); // Start the teleport feature with delay
               }
               // GlobalVariables.LevelInfo.FutureSightUsedTime += Time.deltaTime;
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


       private IEnumerator FirstTimeTeleportFeature()
       {
           yield return new WaitForSeconds(1);
           _pc.fLetterImage.gameObject.SetActive(false);
           _pc.PortalImage.gameObject.SetActive(true);
           _pc.instructionText.text = "Use Portal to Teleport";
           _pc.portal1.gameObject.SetActive(true);
           _pc.portal2.gameObject.SetActive(true);
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
           Debug.Log("INSIDE DISPLAYYYY");
           for (int j = 1; j <= mazeToDisplay.GetLength(0); j++)
           {
               for (int i = 1; i <= mazeToDisplay.GetLength(1); i++)
               {
                   // Fetch the block based on its name
                   GameObject block = GameObject.Find($"block_{j}_{i}");
                   //Debug.Log("block-----"+i+"_"+j);
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
   for (int i = 0; i < _previewMaze.GetLength(0); i++) // Start at 0 to include all rows
   {
       for (int j = 0; j < _previewMaze.GetLength(1); j++) // Start at 0 to include all columns
       {
           // Ensure that the edges of the maze are not randomized if that's required
           if (i > 0 && j > 0 && i < _previewMaze.GetLength(0) - 1 && j < _previewMaze.GetLength(1) - 1)
           {
               _previewMaze[i, j] = Random.Range(0, 4); // Assuming 0-3 are valid block states
           }
       }
   }
}


   }
}
