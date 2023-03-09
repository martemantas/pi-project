using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public UnlockableCharacters[] characters;
    [HideInInspector]
    private static int playerCoins;
    public  TMP_Text moneyText;
    public TMP_Text buyingFailed;
    public TMP_Text buyingsuccesfull;
    private float fadeInTime = 0.2f;
    private float fadeOutTime = 0.2f;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        moneyText.text = "Money: " + playerCoins.ToString();
    }


    public bool TryBuy(GameObject hero)
    {
        foreach (UnlockableCharacters foundcharacter in characters)
        {
            if (foundcharacter.character == hero && foundcharacter.price <= playerCoins)
            {
                playerCoins -= foundcharacter.price;
                foundcharacter.isBought = true;
                Debug.Log("Hero is bought"); return true;

            }
            if (foundcharacter.character == hero && foundcharacter.price > playerCoins)
            {
                StartCoroutine(FadeIn(buyingFailed));
                StartCoroutine(Wait(buyingFailed, 1));
                
                Debug.Log("No money ");
                return false;
            }
        }
        Debug.Log("should not be included");
        return false;
    }

    public bool TryBuy(string heroNAme)
    {
        foreach (UnlockableCharacters foundcharacter in characters)
        {
            if (foundcharacter.name == heroNAme && foundcharacter.price <= playerCoins)
            {
                playerCoins -= foundcharacter.price;
                foundcharacter.isBought = true;
                Debug.Log("Hero is bought"); return true;
            }
            if (foundcharacter.name == heroNAme && foundcharacter.price > playerCoins)
            {
                StartCoroutine(FadeIn(buyingFailed));
                StartCoroutine(Wait(buyingFailed, 1));

                return false;
            }  
        }
        Debug.Log("No Heroes");
        return false;
    }

    public static void MoneyChange(int amount)
    {
        playerCoins += amount;
        if (playerCoins < 0)
        {
            playerCoins = 0;
        }
    }

    public UnlockableCharacters INeedAHero(GameObject hero)
    {
        foreach (UnlockableCharacters foundcharacter in characters)
        {
            if (foundcharacter.character == hero)
            {
                return foundcharacter;
            }
        }
        Debug.Log("Toks herojus nebuvo rastas");
        return null;
    }

    public UnlockableCharacters INeedAHero(string heroNAme)
    {
        foreach (UnlockableCharacters foundcharacter in characters)
        {
            if (foundcharacter.name == heroNAme)
            {
                
                Debug.Log("rastas - " + foundcharacter.name);
                return foundcharacter;
            }
        }
        Debug.Log("Toks herojus nebuvo rastas");
        return null;
    }
    private IEnumerator FadeIn(TMP_Text prompt)
    {
        // activate the text and start fading it in
        prompt.gameObject.SetActive(true);
        float t = 0f;
        Color color = prompt.color;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeInTime);
            prompt.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOut(TMP_Text prompt)
    {
        float t = 0f;
        Color color = prompt.color;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            prompt.color = color;
            yield return null;
        }
        prompt.gameObject.SetActive(false);
    }
    private IEnumerator Wait(TMP_Text prompt, float seconds)
    {
        float t = 0f;
        while (t < seconds)
        {
            t += Time.deltaTime;
            yield return null;
        }
        prompt.gameObject.SetActive(false);
    }
}
