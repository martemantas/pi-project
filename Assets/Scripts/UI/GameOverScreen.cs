using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameOverScreen : MonoBehaviour, IDataPersistance
{
    public GameObject gameOverScreen;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI titleText;
    public Button restartButton;
    public TextMeshProUGUI moneyText;
    public float time;

    public void LoadData(GameData data)
    {
        //nothing
        time = data.time;
    }
    public void SaveData(ref GameData data)
    {
        data.money += MoneyManager.gottenCoins;
        data.time = 0;
    }

    public void Setup() {
        gameOverScreen.SetActive(true);

        StartCoroutine(FadeImageToFullAlpha(0.5f, gameOverScreen.GetComponent<Image>()));
        StartCoroutine(FadeTextToFullAlpha(0.5f, timeText));
        StartCoroutine(FadeTextToFullAlpha(0.5f, titleText));
        StartCoroutine(FadeTextToFullAlpha(0.5f, moneyText));
        StartCoroutine(FadeImageToFullAlpha(0.5f, restartButton.image));

        timeText.text = string.Format("You lived for {0:0.00} minutes", (Time.timeSinceLevelLoad+time) / 60);
        moneyText.text = string.Format("You've got " + MoneyManager.gottenCoins.ToString() + " coins");

    }
    
    public void Restart() {
        
        MoneyManager.AddMoney(MoneyManager.gottenCoins);
        MoneyManager.ResetMoney();
        FindObjectOfType<AudioManager>().Destroy();
        SceneManager.LoadScene("MainHub");
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
