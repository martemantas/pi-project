using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverScreen;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI titleText;
    public Button restartButton;

    public void Setup() {
        gameOverScreen.SetActive(true);

        StartCoroutine(FadeImageToFullAlpha(0.5f, gameOverScreen.GetComponent<Image>()));
        StartCoroutine(FadeTextToFullAlpha(0.5f, timeText));
        StartCoroutine(FadeTextToFullAlpha(0.5f, titleText));
        StartCoroutine(FadeImageToFullAlpha(0.5f, restartButton.image));

        timeText.text = string.Format("You lived for {0:0.00} minutes", Time.timeSinceLevelLoad / 60);
    }
    
    public void Restart() {
        SceneManager.LoadScene("Starting Room");
    }

    public IEnumerator FadeImageToFullAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 0.7f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}
