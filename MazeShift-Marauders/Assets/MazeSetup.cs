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

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && !mazeInitialized)
        if (!mazeInitialized && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            InitializeMaze();
            mazeInitialized = true; // Ensure we don't re-initialize if space is pressed again
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
                Debug.Log($"Processing block_{j}_{i}"+"Maze VAlue - " + maze[j-1, i-1]);
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