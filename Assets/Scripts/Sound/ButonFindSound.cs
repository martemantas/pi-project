using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButonFindSound : MonoBehaviour
{
    public Button button;
    public Toggle toggle;
    void Start()
    {
        button.onClick.AddListener(delegate () { this.ButtonClicked(); });
    }
    public void ButtonClicked()
    {
        AudioController c = FindObjectOfType<AudioController>();
        c.ApplyChanges();

        QualityController q = FindObjectOfType<QualityController>();
        q.ApplyChanges(toggle.isOn);
    }

    public void SetValue()
    {
        bool value;
        int v = 1;

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            v = PlayerPrefs.GetInt("Fullscreen");
        }
        if (v == 0)
            value = false;
        else
            value = true;

        toggle.isOn = value;
    }
}
