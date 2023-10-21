using UnityEngine;
using System.Collections.Generic;

public class MazeSetup : MonoBehaviour
{
    int[,] maze = {
        // {4,4,4,4,4,4,4},
        // {4,3,1,3,0,3,4},
        // {4,0,0,0,1,1,4},
        // {4,3,1,3,0,3,4},
        // {4,1,1,0,1,0,4},
        // {4,3,0,3,1,3,4},
        // {4,4,4,4,4,4,4}

        // {3,1,3,0,3},
        // {0,0,0,1,1},
        // {3,1,3,0,3},
        // {1,1,0,1,0},
        // {3,0,3,1,3}

        {3,1,3,0,3,1,3,0,3,0,3},
        {0,0,0,0,0,0,0,0,1,0,0},
        {3,0,3,1,3,1,3,1,3,1,3},
        {1,0,0,0,0,0,0,0,0,0,0},
        {3,0,3,1,3,0,3,1,3,0,3},
        {0,0,1,0,1,0,1,0,1,0,1},
        {3,0,3,0,3,0,3,0,3,0,3},
        {0,0,1,0,1,0,1,0,1,0,0},
        {3,0,3,0,3,1,3,0,3,1,3},
        {1,0,0,0,0,0,0,0,0,0,0},
        {3,0,3,1,3,1,3,1,3,0,3}
    };

    // Flag to ensure we initialize the maze only once
    private bool mazeInitialized = false;
    int[,] previewMaze;
    float mazeChangeInterval = 20f;  // change every 20 seconds interval
    float mazeChangeTimer;
    bool isPreviewing = false;
    //private Rigidbody playerobjectrb;

    private PlayerControls pc;


    void Start()
    {
        mazeChangeTimer = mazeChangeInterval;  // initialize maze change timer
        GeneratePreviewMaze();  // generate future maze
        //playerobjectrb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
    }
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && !mazeInitialized)
        if (!mazeInitialized && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            InitializeMaze();
            mazeInitialized = true; // Ensure we don't re-initialize if space is pressed again
        }
        if (Input.GetKey(KeyCode.P))
        {
            PreviewNextMaze();
            //playerobjectrb.velocity = Vector3.zero;
            pc.speed=0;
        }
        else if (isPreviewing)
        {
            RevertToCurrentMaze();
            pc.speed = 1.5f;
        }

        mazeChangeTimer -= Time.deltaTime;
        if (mazeChangeTimer <= 0)
        {
            mazeChangeTimer = mazeChangeInterval;
            SetMazeToPreview();
            GeneratePreviewMaze();
        }
    }
    void PreviewNextMaze()
    {
        isPreviewing = true;
        DisplayMaze(previewMaze);
    }

    void RevertToCurrentMaze()
    {
        isPreviewing = false;
        DisplayMaze(maze);
    }

    void DisplayMaze(int[,] mazeToDisplay)
    {
       
        for (int j = 1; j <= 11; j++)
        {
            for (int i = 1; i <= 11; i++)
            {
                GameObject block = GameObject.Find($"block_{j}_{i}");
                if (block)
                {
                    blockController controller = block.GetComponent<blockController>();
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
        maze = previewMaze;  // set current maze as future maze
        InitializeMaze();
    }

    void GeneratePreviewMaze()
    {
        previewMaze = (int[,])maze.Clone(); 

        
        for (int i = 1; i < previewMaze.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < previewMaze.GetLength(1) - 1; j++)
            {
                Vector3 blockPosition = new Vector3(i, 0, j);
                previewMaze[i, j] = Random.Range(0, 2);
            }
        }
    }

    public void ResetMaze()
    {
        ShuffleMaze();
        InitializeMaze();
    }

    void ShuffleMaze()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 playerBlockPosition = new Vector3(Mathf.Floor(player.transform.position.x), 0, Mathf.Floor(player.transform.position.z));

        GameObject[] coins = GameObject.FindGameObjectsWithTag("CoinSpawnPoint");
        List<Vector3> coinPositions = new List<Vector3>();
        foreach (GameObject coin in coins)
        {
            coinPositions.Add(new Vector3(Mathf.Floor(coin.transform.position.x), 0, Mathf.Floor(coin.transform.position.z)));
        }

        // Shuffle the inner blocks
        for (int i = 1; i < maze.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < maze.GetLength(1) - 1; j++)
            {
                Vector3 blockPosition = new Vector3(i, 0, j);
                if (blockPosition != playerBlockPosition && !coinPositions.Contains(blockPosition))
                {
                    maze[i, j] = Random.Range(0, 2);
                }
            }
        }




    }


    void InitializeMaze()
    {
        GameObject sourceBlock = GameObject.Find("block_11_7");
        if (sourceBlock) sourceBlock.GetComponent<Renderer>().material.color = Color.blue;

        GameObject targetBlock = GameObject.Find("block_1_5");
        if (targetBlock) targetBlock.GetComponent<Renderer>().material.color = Color.green;

        for (int j = 1; j <= 11; j++)
        {
            for (int i = 1; i <= 11; i++)
            {
                // Fetch the block based on its name
                GameObject block = GameObject.Find($"block_{j}_{i}");
                //Debug.Log($"Processing block_{j}_{i}"+"Maze VAlue - " + maze[j-1, i-1]);
                if (block)
                {
                    blockController controller = block.GetComponent<blockController>();
                    if (controller)
                    {

                        controller.AdjustBlock(maze[j-1, i-1]);
                    }
                }
            }
        }


    }
}