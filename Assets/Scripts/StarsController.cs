using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StarsController : MonoBehaviour
{
    [SerializeField] private int unlockLevel5 = 12;
    [SerializeField] private int unlockLevel6 = 18;
    public List<GameObject> Level1Star;
    public List<GameObject> Level2Star;
    public List<GameObject> Level3Star;
    public List<GameObject> Level4Star;
    public List<GameObject> Level5Star;
    public List<GameObject> Level6Star;
    public GameObject LockCanvas5;
    public GameObject LockCanvas6;
    public GameObject StarBar5;
    public GameObject StarBar6;
    public TextMeshProUGUI StarNum;
    
    void Start()
    {
        int[] LevelStars = GlobalVariables.LevelStars;
        int TotalStars = 0;
        List<List<GameObject>> StarBar = new List<List<GameObject>>(){ Level1Star, Level2Star, Level3Star, Level4Star, Level5Star, Level6Star };
        for (int i = 1; i <= 4; i++)
        {
            TotalStars += LevelStars[i];
            ShowStars(i, LevelStars[i], StarBar[i - 1]);
        }

        if (TotalStars >= unlockLevel5)
        {
            TotalStars += LevelStars[5];
            LockCanvas5.SetActive(false);
            StarBar5.SetActive(true);
            ShowStars(5, LevelStars[5], StarBar[4]);
        }

        if (TotalStars >= unlockLevel6)
        {
            TotalStars += LevelStars[6];
            LockCanvas6.SetActive(false);
            StarBar6.SetActive(true);
            ShowStars(6, LevelStars[6], StarBar[5]);
        }

        StarNum.text = TotalStars.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ShowStars(int level, int starNumber, List<GameObject> starBar)
    {
        for (int i = 0; i < starNumber; i++)
        {
            Debug.Log(level + " " + i);
            starBar[i].SetActive(true);
        }
    }
}