using System;
using System.Collections;
using System.Collections.Generic;
using Analytic;
using Analytic.DTO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class TutorialScript : MonoBehaviour
{

    public Text dialogueText;
    private string[] instructions = {
         "Use Arrow Keys to move Player"};
    
    void Start()
    {
        dialogueText.text = instructions[0];
    }

    void Update()
    {
        
    }
}
