using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject player;
    public float fadeInTime = 0.2f;
    public float fadeOutTime = 0.2f;
    public KeyCode triggerKey = KeyCode.E;
    private GameObject currentPlayer; // the current player object

    public int sceneID;

    private float lastPressTime;
    public float pressDelay = 1f;

    private Coroutine currentCoroutine;

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (text.gameObject.activeSelf && Input.GetKeyDown(triggerKey) && IsPlayerInsideMask() && Time.time - lastPressTime > pressDelay)
        {
            Debug.Log("E pressed");
            lastPressTime = Time.time;
            ChangeScene(sceneID);
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
        // check if the player is inside the box collider
        if (currentPlayer == null)
        {
            return false;
        }
        return GetComponent<BoxCollider2D>().bounds.Contains(currentPlayer.transform.position);
    }

    private void ChangeScene(int ID)
    {
        // Save the current player object as the last disabled object
        LastDisabledObject.lastDisabledObject = currentPlayer;
        // Save the name of the current player object
        PlayerPrefs.SetString(player.name, currentPlayer.name);
        SceneManager.LoadScene(ID);
    }
}
