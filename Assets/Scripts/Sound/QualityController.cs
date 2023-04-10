using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QualityController : MonoBehaviour
{
    public RenderTexture texture;
    public void HandleInputData(int val)
    {
        //save value
        
        if(val == 0)
            Resize(texture, 1920, 1080);
        if(val == 1)
            Resize(texture, 640, 360);
        if(val == 2)
            Resize(texture, 256, 144);

        PlayerPrefs.SetInt("Quality", val);
    }

    void Resize(RenderTexture renderTexture, int width, int height)
    {
        Debug.Log(renderTexture);
        if (renderTexture)
        {
            renderTexture.Release();
            renderTexture.width = width;
            renderTexture.height = height;
            //renderTexture.Create();
        }
        Debug.Log(width);
    }

    public void ApplyChanges(bool value)
    {
        int v = 1;
        if (!value) v = 0;

        PlayerPrefs.SetInt("Fullscreen", v);
    }
}
