using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeProgressBarLevel2 : MonoBehaviour
{
   public Image fillImage; 
   public Color startColor = Color.green; 
    public Color endColor = Color.red; 
    private float timeLeft;
    private float totalTime;
    private bool isActive = false; 

    public void StartProgressBarSequence(float duration)
    {
        totalTime = duration; 
        timeLeft = totalTime;
        isActive = true;

        fillImage.fillAmount = 1;
        fillImage.color = startColor; 
    }

    void Update()
    {
        if (!isActive)
            return;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            fillImage.fillAmount = timeLeft / totalTime;

            
            fillImage.color = Color.Lerp(endColor, startColor, fillImage.fillAmount);
        }
        else
        {
            isActive = false;
        }
    }
}
