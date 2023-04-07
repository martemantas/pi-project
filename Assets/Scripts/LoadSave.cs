using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSave : MonoBehaviour
{
    private Slider volumeSlider = null;
    //[SerializeField] private Text volumeTextUI = null;

    private void Start()
    {
        //float volumeValue = volumeSlider.value;
        //LoadValues();
    }

    /*public void VolumeSlider(float volume)
    {
        volumeTextUI.text = volume.ToString("0.0");
    }*/

    public void SaveVolumeButton()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("GameVolume", volumeValue);
        LoadValues();
    }

    void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("GameVolume");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
    
}
