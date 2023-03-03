using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class CharacterSwitch : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject player;
    public float fadeInTime = 0.2f;
    public float fadeOutTime = 0.2f;
    public KeyCode triggerKey = KeyCode.E;
    public GameObject playerPrefab; // the prefab to switch to
    public CircleCollider2D circleCollider; // the sprite mask the script is attached to
    private GameObject currentPlayer; // the current player object
    public SpriteRenderer spriteRenderer;
    public ParticleSystem particleSystem;

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

        if (text.gameObject.activeSelf && Input.GetKeyDown(triggerKey) && IsPlayerInsideMask() && Time.time - lastPressTime > pressDelay)
        {
            Debug.Log("E pressed");
            lastPressTime = Time.time;
            SwitchPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeIn());

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
            currentCoroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        // wait until the player is outside the collider
        while (IsPlayerInsideMask())
        {
            yield return null;
        }
        // activate the text and start fading it in
        text.gameObject.SetActive(true);
        float t = 0f;
        Color color = text.color;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeInTime);
            text.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        Color color = text.color;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            text.color = color;
            yield return null;
        }
        text.gameObject.SetActive(false);
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
        GameObject newPlayer = Instantiate(playerPrefab, currentPlayer.transform.position, Quaternion.identity);

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