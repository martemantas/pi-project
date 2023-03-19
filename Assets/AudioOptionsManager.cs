using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioOptionsManager : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }
    [SerializeField] private TextMeshProUGUI musicslider;
    public void OnMusicSliderValueChange(float volume)
    {
        musicVolume = volume;
        AudioManager.instance.UpdateMixerVolume();

    }
}
