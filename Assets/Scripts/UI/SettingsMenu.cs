using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    private ParticleSystem particleSystem;

    public int menuID = 0;
    public GameObject[] menuPanels;
    private GameObject mainMenuPanel;
    private GameObject optionsPanel;

    // Use t$$anonymous$$s for initialization
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();

        menuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");

        mainMenuPanel = GameObject.Find("MainMenu");
        optionsPanel = GameObject.Find("SettingsMenu");

        switchToMenu(menuID);
    }

    public void switchToMenu(int menuID)
    {
        foreach (GameObject panel in menuPanels)
        {
            panel.gameObject.SetActive(false);
            Debug.Log(panel.name);
        }

        switch (menuID)
        {
            case 0:
                particleSystem.Play();
                mainMenuPanel.gameObject.SetActive(true);
                break;
            case 1:
                particleSystem.Play();
                optionsPanel.gameObject.SetActive(true);
                break;
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);
    }

    public void SetQuality(int Index)
    {
        QualitySettings.SetQualityLevel(Index);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
