using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup SoundVolume;

    public Sound[] sounds;
    public static AudioManager instance;
    [SerializeField] public List<AudioSource> allAudioSources;
    [SerializeField] public List<bool> allAudioSourcesRunningState;
    [SerializeField] public static bool paused;
    public ParticleSystem Rain, RainDrops;
    public float volume { private get;  set; }
    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (GetSound("BackGround_Music1").isPlaying)
            {
                Stop("BackGround_Music1");
                Play("BackGround_Music2");
                Play("Rain_Sound");
                Rain.Play();
                RainDrops.Play();
            }
            else
            {
                GetComponent<AudioManager>().Stop("BackGround_Music2");
                GetComponent<AudioManager>().Stop("Rain_Sound");
                GetComponent<AudioManager>().Play("BackGround_Music1");
                Rain.Stop();
                RainDrops.Stop();
            }
            if (Rain == null || RainDrops == null)
            {
                //GameObject.FindWithTag("GameOverScreen")
                //GameObject.FindWithTag("GameOverScreen")
            }

        }
    }
    // Start is called before the first frame update
    void Awake() // deletes second AudioManager if it appears
    {
        if (instance == null) 
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) // loads every audiocomponent with chosen parameters
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            switch (s.audioType)
            {
                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = SoundVolume;
                    break;
                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = SoundVolume;
                    break;
            }
        }


    }
    public void ChangeSound(float volume)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume *= volume;
        }
    }
    public void PauseSounds(bool paused)
    {
        allAudioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
        foreach (AudioSource a in allAudioSources)
        {
            allAudioSourcesRunningState.Add(a.isPlaying);
        }

        if (paused)
        {
            for (int i = 0; i < allAudioSources.Count; i++)
            {
                if (allAudioSourcesRunningState[i])
                {
                    allAudioSources[i].UnPause();
                }

            }

        }
        else
        {
            allAudioSourcesRunningState = new List<bool>();

            foreach (AudioSource a in allAudioSources)
            {
                allAudioSourcesRunningState.Add(a.isPlaying);
                if (a.isPlaying)
                {
                    a.Pause();
                }
            }
        }
    }
    private void Start()
    {
        Play("BackGround_Music1"); //I guess it plays music on start, idk
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// plays a sound
    /// </summary>
    /// <param name="name">name of a sound to play</param>
    public void Play(string name) 
    {
        Sound s =Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found");
            return;
        }
        s.source.Play();

        if(PauseMenu.GameIsPaused)
        {
            s.source.pitch *= .5f;
        }
    }
    public void UpdateMixerVolume()
    {
        SoundVolume.audioMixer.SetFloat("soundVolume", MathF.Log10(AudioOptionsManager.musicVolume) * 20);
    }

    /// <summary>
    /// finds and returns a sound
    /// </summary>
    /// <param name="name">name of the sound to find</param>
    /// <returns>Sound class object , null if not found</returns>
    public AudioSource GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found");
            return null;
        }
        return s.source;
    }

    /// <summary>
    /// stops playing that sound
    /// </summary>
    /// <param name="name">name of the sound</param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found");
            return;
        }
        s.source.Stop();
    }
    /// <summary>
    /// Plays only once, won't play if it is playing already
    /// </summary>
    /// <param name="name">name of the Sound</param>
    public void PlayOnlyOnce(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found");
            return;
        }
        if (s.source.isPlaying == false)
        {
            s.volume = UnityEngine.Random.Range(0.5f, 1);
            s.pitch = UnityEngine.Random.Range(0.3f, 2.5f);
            s.source.Play();
        }
    }
    /// <summary>
    /// bool method, just to check when Player tries to walk
    /// </summary>
    /// <returns>true if keys are pressed</returns>
    public static bool WalkingKeysPressed()
    {
        if (paused)
        {
            return false;
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                return true;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                return true;
            }
            return false;
        }

    }
}
