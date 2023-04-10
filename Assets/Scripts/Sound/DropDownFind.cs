using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDownFind : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    void Start()
    {
        dropdown.onValueChanged.AddListener(delegate
        {
            ValueChanged(dropdown.value);
        });

    }
    public void ValueChanged(int value)
    {
        QualityController q = FindObjectOfType<QualityController>();
        q.HandleInputData(value);
    }

    public void SetValue()
    {
        int value = 0;

        if (PlayerPrefs.HasKey("Quality"))
        {
            value = PlayerPrefs.GetInt("Quality");
        }

        dropdown.value = value;
        ValueChanged(value);
        dropdown.RefreshShownValue();
    }
}
