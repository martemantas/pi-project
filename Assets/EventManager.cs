using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EventManager : MonoBehaviour
{
    public ParticleSystem Rain, RainDrops;

    // Start is called before the first frame update
    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (GetComponent<AudioManager>().GetSound("BackGround_Music1").isPlaying)
            {
                GetComponent<AudioManager>().Stop("BackGround_Music1");
                GetComponent<AudioManager>().Play("BackGround_Music2");
                GetComponent<AudioManager>().Play("Rain_Sound");
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

        }
    }
}
