using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject PauseMain;
    public GameObject PauseSettings;

    public List<AudioSource> allAudioSources;
    public List<bool> allAudioSourcesRunningState;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        PauseSettings.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        FindObjectOfType<AudioManager>().PauseSounds(true);
        AudioManager.paused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        FindObjectOfType<AudioManager>().PauseSounds(false);
        AudioManager.paused = true;
    }

    public void LoadMenu()
    {
        Resume();
        MoneyManager.AddMoney(MoneyManager.gottenCoins);
        Debug.Log(MoneyManager.gottenCoins + "Coins");
        MoneyManager.ResetMoney();
        SceneManager.LoadScene("TitleScreen");
    }
    public void LoadHub()
    {
        Resume();
        MoneyManager.AddMoney(MoneyManager.gottenCoins);
        Debug.Log(MoneyManager.gottenCoins + "Coins");
        MoneyManager.ResetMoney();
        SceneManager.LoadScene("MainHub");
    }

    public void SettingsMenu()
    {
        if (PauseMain.activeSelf)
        {
            PauseMain.SetActive(false);
            PauseSettings.SetActive(true);
        }
        else
        {
            PauseMain.SetActive(true);
            PauseSettings.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    

}
