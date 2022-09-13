using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject victoryMenuUI;
    public GameObject defeatMenuUI;

    private bool canPause = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Constants.EXIT) && canPause)
        {
            if (gameIsPaused)
            {
                Resume();

            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Victory()
    {
        victoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        canPause = false;
    }

    public void Defeat()
    {
        defeatMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        canPause = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
