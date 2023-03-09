using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class CharacterSwitch : MonoBehaviour
{
    public TextMeshProUGUI ePrompt;
    
    private GameObject player;
    public float fadeInTime = 0.2f;
    public float fadeOutTime = 0.2f;
    public KeyCode triggerKey = KeyCode.E;
    public GameObject newHero; // the prefab to switch to
    private CircleCollider2D circleCollider; // the sprite mask the script is attached to
    private GameObject currentPlayer; // the current player object
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particleSystem;
    

    // 
    public TMP_Text buyPrompt;

    private float lastPressTime;
    public float pressDelay = 1f;

    private Coroutine currentCoroutine;
 

    
    private void Start()
    {
        // Get the CircleCollider2D component on the game object
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (buyPrompt.gameObject.activeSelf && Input.GetKeyDown(triggerKey) && Time.time - lastPressTime > pressDelay && IsPlayerInsideMask())
        {
            lastPressTime = Time.time;
            if (!FindAnyObjectByType<MoneyManager>().TryBuy(newHero))
            {
                
            }
            currentCoroutine = StartCoroutine(FadeOut(buyPrompt));
        }

        if (ePrompt.gameObject.activeSelf && Input.GetKeyDown(triggerKey) && IsPlayerInsideMask() && Time.time - lastPressTime > pressDelay)
        {
            Debug.Log("E pressed");
            lastPressTime = Time.time;
            SwitchPlayer();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        buyPrompt.text = "This hero costs " + FindAnyObjectByType<MoneyManager>().INeedAHero(newHero).price + ", press [E] to buy";
        if (collision.gameObject == player)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            if (!FindAnyObjectByType<MoneyManager>().INeedAHero(newHero).isBought)
            {
                currentCoroutine = StartCoroutine(FadeIn(buyPrompt));
            }
            if (FindAnyObjectByType<MoneyManager>().INeedAHero(newHero).isBought)
            {
                currentCoroutine = StartCoroutine(FadeIn(ePrompt));
            }
            
            // set the currentPlayer object to the player that entered the collider
            currentPlayer = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            if (!FindAnyObjectByType<MoneyManager>().INeedAHero(newHero).isBought)
            {
                currentCoroutine = StartCoroutine(FadeOut(buyPrompt));
            }
            if (FindAnyObjectByType<MoneyManager>().INeedAHero(newHero).isBought)
            {
                currentCoroutine = StartCoroutine(FadeOut(ePrompt));
            }
        }
    }

    private IEnumerator FadeIn(TMP_Text prompt)
    {
        // wait until the player is outside the collider
        while (IsPlayerInsideMask())
        {
            yield return null;
        }
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

    private bool IsPlayerInsideMask()
    {
        // check if the player is inside the circle collider
        if (currentPlayer == null)
        {
            return false;
        }
        return GetComponent<CircleCollider2D>().bounds.Contains(currentPlayer.transform.position);
    }


    private void SwitchPlayer()
    {
        // spawn the new player prefab at the current position of the old player
        GameObject newPlayer = Instantiate(newHero, currentPlayer.transform.position, Quaternion.identity);

        LastDisabledObject.currentObject = newPlayer;

        // destroy the old player
        Destroy(currentPlayer);

        if (LastDisabledObject.lastDisabledObject != null)
        {
            Collider2D otherCollider = LastDisabledObject.lastDisabledObject.GetComponent<Collider2D>();
            otherCollider.enabled = true;
            SpriteRenderer otherSpriteRenderer = LastDisabledObject.lastDisabledObject.GetComponent<SpriteRenderer>();
            otherSpriteRenderer.enabled = true;
        }

        LastDisabledObject.lastDisabledObject = gameObject;

        spriteRenderer.enabled = false;
        circleCollider.enabled = false;

        particleSystem.Play();

        //text.gameObject.SetActive(false);

        // disable the game object
        //gameObject.SetActive(false);
    }
}