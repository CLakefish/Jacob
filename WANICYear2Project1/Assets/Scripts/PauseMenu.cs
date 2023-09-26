/*
 * zak baydass
 * 10/28/2022
 * this script is what the Menu system uses to enable the pause menu and settings menu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public static bool SettingsisOpen = false;
    public static bool canPause = false;
    private bool isPaused = false;
    public GameObject PauseUI;

    private void Awake()
    {
        canPause = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            onPause();
        }
    }
    public void onPause() //uses new input system to pause game by setting value to true or false. 
    {
        if (canPause == true && SettingsisOpen == false)
        {
            if (GameisPaused == true && canPause == true)
            {
                Resume();
            }
            else if (GameisPaused == false && canPause == true)
            {
                Pause();
            }
        }
        Debug.Log("pressed Pause");
    }
    public void Resume()
    {
        PauseUI.SetActive(false);
        GameisPaused = false;
        isPaused = false;
        Time.timeScale = 1;
    }
    void Pause()
    {
        PauseUI.SetActive(true);
        GameisPaused = true;
        Time.timeScale = 0;
        isPaused = true;
    }
    public void OpenSettings()
    {
        SettingsisOpen = true;

    }
    public void CloseSettings()
    {
        SettingsisOpen = false;

    }
    public void StartGame()
    {

    }
}
