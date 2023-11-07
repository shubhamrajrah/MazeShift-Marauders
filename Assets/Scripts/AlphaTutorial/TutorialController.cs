using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    // public GameObject dialoguePanel;
    public Text dialogueText;
    public Text instructionCounterText;
    public GameObject timerObject;
    public GameObject scoreObject;
    public GameObject winTileObject;

    private string[] messages = {
        ""
    };

    private int currentMessageIndex = 0;

    private void Start()
    {
        
        this.gameObject.SetActive(true);
        StartCoroutine(DisplayMessages());
    }

    private IEnumerator DisplayMessages()
    {
        
        while (currentMessageIndex < messages.Length)
        {
            dialogueText.text = messages[currentMessageIndex];
            instructionCounterText.text = (currentMessageIndex + 1) + "/" + messages.Length; // Update the progress
            currentMessageIndex++;

            if (currentMessageIndex == 2)
            {
                // Change the color of the "WinTile" GameObject to green
                Renderer renderer = winTileObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.green;
                }
                
            }
            else if (currentMessageIndex == 3)
            {
                timerObject.SetActive(true);
            }
            else if (currentMessageIndex == 4)
            {
                scoreObject.SetActive(true);
            }
            yield return new WaitForSeconds(3);
        }
        this.gameObject.SetActive(false);
    }
}