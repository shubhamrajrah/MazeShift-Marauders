using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    bool isPaused = false;

    public GameObject PausePanel;

    public GameObject PowerUpText;

    private GameObject textMeshProButton;
    // Start is called before the first frame update
    void Start()
    {
        PausePanel.SetActive(false);
        textMeshProButton = GameObject.FindWithTag("PauseMenu");
        //objectRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
    // if (Input.GetKeyDown(KeyCode.Escape))
    // {
    //     Paused();
    // }

    }

    public void Paused()
    {
        Debug.Log("Pause Menu Clicked");
        isPaused = !isPaused;
        PausePanel.SetActive(isPaused);
        if(isPaused){
        Time.timeScale = 0f;
        textMeshProButton.SetActive(false);
        if(PowerUpText){
        PowerUpText.SetActive(false);
    }
    }
    else {
        Time.timeScale = 1f;
        textMeshProButton.SetActive(true);
        if(PowerUpText){
        PowerUpText.SetActive(true);
    }
    }
    //objectRenderer.enabled = !isPaused;

    }
}
