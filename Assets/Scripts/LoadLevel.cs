using System;
using System.Collections;
using Analytic.DTO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    private bool _isStarEnough;

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
        // set global variable to null
        GlobalVariables.LevelInfo = null;
    }

    public void ReLoadScene()
    {
        Debug.Log("Restart Scene");
        int currentIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIdx);
    }

    public void SendTrack(bool result)
    {
        if (GlobalVariables.LevelTrack == null)
        {
            return;
        }

        LevelTrack track = GlobalVariables.LevelTrack;
        track.EndLevel = GlobalVariables.Level;
        track.IsReachTheEnd = result;
        Debug.Log(JsonUtility.ToJson(track, true));
        track.SendTrack();
        GlobalVariables.LevelTrack = null;
    }

    public void CheckStarNumber(int starRequired)
    {
        int level = GlobalVariables.Level, totalStars = 0;
        for (int i = 1; i <= level; i++)
        {
            totalStars += GlobalVariables.LevelStars[i];
        }
        _isStarEnough = totalStars >= starRequired;
    }

    public void CheckAndLoad(GameObject popUpWindow)
    {
        int level = GlobalVariables.Level;
        if (_isStarEnough)
        {
            LoadScene($"Level{level + 1}");
        }
        else
        {
            // star is not enough, go back to main menu
            popUpWindow.SetActive(true);
            StartCoroutine(LoadMainAfterDelay());
            
        }
        SendResult(true);
    }
    
    private IEnumerator LoadMainAfterDelay()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1.5f); // wait for 3 s
        Debug.Log("调用了");
        LoadScene("Menu");
    }
    
}