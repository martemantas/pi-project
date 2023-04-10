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
    private GameObject controlsPanel;

    // Use t$$anonymous$$s for initialization
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();

        menuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");

        mainMenuPanel = GameObject.Find("MainMenu");
        optionsPanel = GameObject.Find("SettingsMenu");
        controlsPanel = GameObject.Find("ControlsMenu");

        switchToMenu(menuID);
    }

    public void switchToMenu(int menuID)
    {
        foreach (GameObject panel in menuPanels)
        {
            panel.gameObject.SetActive(false);
        }

        switch (menuID)
        {
            case 0:
                if(particleSystem != null)
                    particleSystem.Play();
                mainMenuPanel.gameObject.SetActive(true);
                break;
            case 1:
                if(particleSystem!= null)
                    particleSystem.Play();
                optionsPanel.gameObject.SetActive(true);
                break;
            case 2:
                if(particleSystem != null)
                    particleSystem.Play();
                controlsPanel.gameObject.SetActive(true);
                break;
            case 3:
                if (particleSystem != null)
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
