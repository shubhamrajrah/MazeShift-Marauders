using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour
{
    public Image fillerImage;
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    private bool isProgressing = false;

    private void HideProgressBar()
    {
        this.gameObject.SetActive(false);
    }

    public void StartProgress(float timeInSeconds)
    {
        if (!isProgressing)
        {
            this.gameObject.SetActive(true);
            fillerImage.fillAmount = 1;
            StartCoroutine(ProgressRoutine(timeInSeconds));
        }
        
    }

    private IEnumerator ProgressRoutine(float timeInSeconds)
    {
        isProgressing = true;
        float elapsed = 0;
        float startValue = fillerImage.fillAmount;

        while (elapsed < timeInSeconds)
        {
            float progress = elapsed / timeInSeconds;
            fillerImage.fillAmount = Mathf.Lerp(startValue, 0, progress);
            fillerImage.color = Color.Lerp(startColor, endColor, progress);

            elapsed += Time.deltaTime;
            yield return null;
        }

        fillerImage.fillAmount = 0;
        isProgressing = false;
        HideProgressBar();
    }


}