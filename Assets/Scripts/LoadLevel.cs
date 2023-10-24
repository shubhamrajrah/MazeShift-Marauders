using System;
using Analytic.DTO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public void LoadNextLevel(int levelNum)
    {
        GlobalVariables.Level = levelNum;
    }

    public void LoadScene(string sceneName)
    {
        // Load the scene with the specified name
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int idx)
    {
        SceneManager.LoadScene(idx);
    }

    public void SendResult(bool result)
    {
        if (GlobalVariables.LevelInfo == null)
        {
            return;
        }
        LevelInfo levelInfo = GlobalVariables.LevelInfo;
        levelInfo.IsSuccess = result;
        Debug.Log(JsonUtility.ToJson(levelInfo, true));
        if (!levelInfo.IsSuccess)
        {
            levelInfo.CalculateInterval(DateTime.Now);
        }
        // send data to firebase
        levelInfo.SendData();
        // set global variable to nullddddddd
        GlobalVariables.LevelInfo = null;
    }
    
    public void ReLoadScene()
    {
        Debug.Log("Restart Scene");
        int currentIdx = SceneManager.GetActiveScene().buildIndex;
        LevelInfo levelInfo = GlobalVariables.LevelInfo;
        if (levelInfo != null)
        {
            levelInfo.DeadTimesUp++;
            levelInfo.CoinCollected = 0;
        }
        SceneManager.LoadScene(currentIdx);
    }
}