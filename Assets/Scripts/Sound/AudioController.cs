using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{

    [SerializeField]
    private Profiles m_profiles;

    [SerializeField]
    private List<SoundSlider> m_VolumeSliders = new List<SoundSlider>();

    void Awake()
    {
        if (m_profiles != null)
            m_profiles.SetProfile(m_profiles);
        
    }

    
    void Start()
    {
        if (Settings.profile && Settings.profile.audioMixer != null)
            Settings.profile.GetAudioLevels();
    }

    public void ApplyChanges()
    {
        if (Settings.profile && Settings.profile.audioMixer != null)
            Settings.profile.SaveAudioLevels();
       
    }

    public void CancelChanges()
    {
        if(Settings.profile && Settings.profile.audioMixer != null)
            Settings.profile.GetAudioLevels();

        for(int i=0; i<m_VolumeSliders.Count; i++)
        {
            m_VolumeSliders[i].ResetSliderValue();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SoundSlider[] sliders = FindObjectsOfType<SoundSlider>();
        m_VolumeSliders.Clear();
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i].gameObject.tag == "Sound")
                m_VolumeSliders.Add(sliders[i]);
        }
        FindObjectOfType<DropDownFind>().SetValue();
        FindObjectOfType<ButonFindSound>().SetValue();
    }
 

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
}
