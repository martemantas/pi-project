using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof (Slider))]
public class SoundSlider : MonoBehaviour
{
    Slider slider
    {
        get { return GetComponent<Slider>(); }
    }
   
    [SerializeField]
    private string VolumeName;
    [SerializeField]
    private Text volumeLabel;

    void Start()
    {
        ResetSliderValue();

        slider.onValueChanged.AddListener(delegate
        {
            UpdateValueOnChange(slider.value);
        });
    }

    public void UpdateValueOnChange(float value)
    {
        if (volumeLabel != null)
            volumeLabel.text = Mathf.Round(value * 100.0f).ToString() + "%";

        if (Settings.profile)
        {
            Settings.profile.SetAudioLevels(VolumeName, value);
        }
    }

    public void ResetSliderValue()
    {
        if (Settings.profile)
        {
            float volume = Settings.profile.GetAudioLevels(VolumeName);

            UpdateValueOnChange(volume);
            slider.value = volume;
        }
    }
}
