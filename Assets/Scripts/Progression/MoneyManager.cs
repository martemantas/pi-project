using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public UnlockableCharacters[] characters;
    [HideInInspector]
    private static int playerCoins;
   // public  TMP_Text moneyText;
    private GameObject buyStatus;
   // private GameObject buyingsuccesfull;
    private float fadeInTime = 0.2f;
    private float fadeOutTime = 0.2f;
    public static MoneyManager instance;
    public static int gottenCoins;

    void Awake() //shouold remove second MoneyManager if it appears on scene
    {
        playerCoins = PlayerPrefs.GetInt("Money", 0);
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

    //    DontDestroyOnLoad(gameObject); // makes it so this object would persist trough scenes
        
    }
    private void Start()
    {
        buyStatus = FindInActiveObjectByTag("buyingText");
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("MoneyText") != null)
        {
            GameObject.FindGameObjectWithTag("MoneyText").GetComponent<TextMeshProUGUI>().text = "Money: " + playerCoins.ToString(); //should constantly update 
        }
    }

    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }



    /// <summary>
    /// tries to buy a hero by its gameObject
    /// </summary>
    /// <param name="hero">which hero you are searching for</param>
    /// <returns>true if bought</returns>
    public bool TryBuy(GameObject hero)
    { 
        foreach (UnlockableCharacters foundcharacter in characters)
        {
            if (foundcharacter.character == hero && foundcharacter.price <= playerCoins)
            {
                playerCoins -= foundcharacter.price;
                foundcharacter.isBought = true; //    buyingSuccess

                buyStatus.GetComponent<TextMeshProUGUI>().text = "You now own this hero!"; buyStatus.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                StartCoroutine(FadeIn(buyStatus.GetComponent<TextMeshProUGUI>()));
                StartCoroutine(Wait(buyStatus.GetComponent<TextMeshProUGUI>(), 2));

                Debug.Log("Hero is bought"); return true;

            }
            if (foundcharacter.character == hero && foundcharacter.price > playerCoins)
            {
                int debt = foundcharacter.price - playerCoins;
                buyStatus.GetComponent<TextMeshProUGUI>().text = "You can't buy this hero, yet" + '\n' + "You are " + debt + " coins short"; buyStatus.GetComponent<TextMeshProUGUI>().color = Color.red;
                StartCoroutine(FadeIn(buyStatus.GetComponent<TextMeshProUGUI>()));
                StartCoroutine(Wait(buyStatus.GetComponent<TextMeshProUGUI>(), 2));

                Debug.Log("No money ");
                return false;
            }
        }
        Debug.Log("should not be included");
        return false;
    }

    public static int MoneyGainOnRun(int amount)
    {
       return gottenCoins += amount;
    }
    public static void ResetMoney()
    {
        gottenCoins = 0;
    }
    /// <summary>
    /// tries to buy a hero by it's name
    /// </summary>
    /// <param name="heroNAme">hero you are searching for</param>
    /// <returns>true if bought</returns>
    public bool TryBuy(string heroNAme) 
    {
        foreach (UnlockableCharacters foundcharacter in characters)
        {
            if (foundcharacter.name == heroNAme && foundcharacter.price <= playerCoins)
            {
                playerCoins -= foundcharacter.price;
                foundcharacter.isBought = true;

                buyStatus.GetComponent<TextMeshProUGUI>().text = "You now own this hero!"; buyStatus.GetComponent<TextMeshProUGUI>().color = Color.yellow;
                StartCoroutine(FadeIn(buyStatus.GetComponent<TextMeshProUGUI>()));
                StartCoroutine(Wait(buyStatus.GetComponent<TextMeshProUGUI>(), 2));
                Debug.Log("Hero is bought"); return true;
            }
            if (foundcharacter.name == heroNAme && foundcharacter.price > playerCoins)
            {
                int debt = foundcharacter.price - playerCoins;
                buyStatus.GetComponent<TextMeshProUGUI>().text = "You can't buy this hero, yet" + '\n' + "You are " + debt + " coins short"; buyStatus.GetComponent<TextMeshProUGUI>().color = Color.red;
                StartCoroutine(FadeIn(buyStatus.GetComponent<TextMeshProUGUI>()));
                StartCoroutine(Wait(buyStatus.GetComponent<TextMeshProUGUI>(), 2));

                Debug.Log("No money ");
                return false;
            }  
        }
        Debug.Log("No Heroes");
        return false;
    }

    public static void AddMoney(int amount) //changes your current money by the given amount
    {
        playerCoins += amount;
        if (playerCoins < 0)
        {
            playerCoins = 0;
        }
        PlayerPrefs.SetInt("Money", playerCoins);
        PlayerPrefs.Save();
        Debug.Log("Issaugota" + playerCoins);
    }
    public static void RemoveMoney(int amount)
    {
        playerCoins -= amount;
        if (playerCoins < 0)
        {
            playerCoins = 0;
        }
        PlayerPrefs.SetInt("Money", playerCoins);
        PlayerPrefs.Save();
        Debug.Log("Issaugota" + playerCoins);
    }


    public UnlockableCharacters INeedAHero(GameObject hero) // finds and returns a hero by a gameObject
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

    public UnlockableCharacters INeedAHero(string heroNAme) //finds and returns a hero by a name
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
    private IEnumerator Wait(TMP_Text prompt, float seconds) //waits for t seconds an then stops showing text
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
