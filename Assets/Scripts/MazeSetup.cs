using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class MazeSetup : MonoBehaviour
{

    int[,] maze;
    int[,] maze1;
    int[,] maze2;
    int[,] maze_og_level3 = {
        {3,0,3,0,3,1,3,0,3,0,3,0,3,0,3}, //1
        {0,0,0,0,0,0,0,0,1,0,0,0,3,1,3}, //2
        {3,0,3,1,3,1,3,1,3,1,3,1,3,0,3}, //3
        {1,0,0,0,0,0,0,0,0,0,0,0,3,0,3}, //4
        {3,0,3,1,3,0,3,1,3,0,3,1,3,1,3}, //5
        {0,0,1,0,1,0,1,0,1,0,1,0,3,0,3}, //6
        {3,0,3,0,3,0,3,0,3,0,3,0,3,0,3}, //7
        {0,0,1,0,1,0,1,0,1,0,0,1,3,0,3}, //8
        {3,0,3,0,3,1,3,0,3,1,3,0,3,1,3}, //9
        {1,0,0,0,0,0,0,0,0,0,0,0,3,0,3}, //10
        {3,0,3,1,3,1,3,1,3,0,3,1,3,0,3}, //11
        {3,1,3,0,3,1,3,0,3,0,3,1,3,0,3}, //1
        {1,1,0,0,0,0,0,0,1,1,0,1,3,0,3}, //2
        {3,0,3,1,3,1,3,1,3,1,3,0,3,1,3}, //3
        {1,0,0,0,0,1,0,0,0,0,1,0,3,1,3} //4
    };
    int[,] maze_leve1_alt1 = {
        //maze 1 [alternate switching] - source - block_11_1 
        //target - block_7_11
        {3,0,3,1,3,0,3,1,3,0,3}, //1
        {0,1,0,0,0,0,1,1,0,0,1}, //2
        {3,1,3,0,3,0,3,1,3,0,3}, //3
        {0,0,1,0,1,0,1,1,0,1,0}, //4
        {3,0,3,0,3,0,3,1,3,1,3}, //5
        {0,1,0,0,1,1,0,1,0,1,0}, //6
        {3,1,3,0,3,1,3,1,3,1,3}, //7
        {1,0,1,0,0,1,0,0,0,0,1}, //8
        {3,0,3,0,3,1,3,0,3,0,3}, //9
        {0,1,0,0,1,0,1,1,1,0,0}, //10
        {3,1,3,0,3,0,3,1,3,0,3} //11
    };
    int[,] maze_level_alt2 = {
        // //maze2 [alternate switching]
        //maze 1 [alternate switching]
        {3,0,3,1,3,0,3,0,3,0,3}, //1
        {1,1,0,0,0,0,1,1,0,0,1}, //2
        {3,1,3,0,3,0,3,1,3,0,3}, //3
        {0,0,0,1,1,0,1,1,1,1,0}, //4
        {3,0,3,1,3,0,3,1,3,1,3}, //5
        {1,0,0,0,0,1,0,1,0,1,0}, //6
        {3,0,3,0,3,1,3,1,3,1,3}, //7
        {0,0,1,0,1,0,1,0,0,1,1}, //8
        {3,0,3,0,3,0,3,0,3,1,3}, //9
        {0,0,1,1,0,0,1,1,0,0,0}, //10
        {3,0,3,1,3,0,3,1,3,0,3} //11
    };

    int[,] maze_og_level2 = {
        //maze 1 [alternate switching] - source - block_11_1 
        //target - block_7_11
        {3,0,3,1,3,0,3,1,3,0,3}, //1
        {0,1,0,0,0,0,1,1,0,0,1}, //2
        {3,1,3,0,3,0,3,1,3,0,3}, //3
        {0,0,1,0,1,0,1,1,0,1,0}, //4
        {3,0,3,0,3,0,3,1,3,1,3}, //5
        {0,1,0,0,1,1,0,1,0,1,0}, //6
        {3,1,3,0,3,1,3,1,3,1,3}, //7
        {1,0,1,0,0,1,0,0,0,0,1}, //8
        {3,0,3,0,3,1,3,0,3,0,3}, //9
        {0,1,0,0,1,0,1,1,1,0,0}, //10
        {3,1,3,0,3,0,3,1,3,0,3} //11
    };


    // Flag to ensure we initialize the maze only once
    private bool mazeInitialized = false;
    private float switchTime = 5.0f; 
    private float lastSwitch = 0.0f; 
    
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level1")
        {
            maze1 = maze_leve1_alt1;
            maze2 = maze_level_alt2;
        }
        else if (currentScene == "Level2")
        {
            maze = maze_level_alt2; 
        }
        else if (currentScene == "Level3")
        {
            maze = maze_og_level3; 
        }
        else
        {
            maze = maze_leve1_alt1; 
        }
    }
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && !mazeInitialized)
        if (!mazeInitialized && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            
            InitializeMaze();
            mazeInitialized = true; // Ensure we don't re-initialize if space is pressed again
        }
        if (Time.time - lastSwitch > switchTime)
        {
            ToggleMaze();
            lastSwitch = Time.time;
        }

    }
    void ToggleMaze()
    {
        if (maze == maze1)
        {
            maze = maze2;
        }
        else
        {
            maze = maze1;
        }

        InitializeMaze();
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
        GameObject sourceBlock = GameObject.Find("block_11_1");
        if (sourceBlock) sourceBlock.GetComponent<Renderer>().material.color = Color.blue;

        GameObject targetBlock = GameObject.Find("block_7_11");
        if (targetBlock) targetBlock.GetComponent<Renderer>().material.color = Color.green;

        for (int j = 1; j <= maze.GetLength(0); j++)
        {
            for (int i = 1; i <= maze.GetLength(1); i++)
            {
                // Fetch the block based on its name
                GameObject block = GameObject.Find($"block_{j}_{i}");
                // Debug.Log($"Processing block_{j}_{i}"+"Maze VAlue - " + maze[j-1, i-1]);
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