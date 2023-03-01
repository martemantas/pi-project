using UnityEngine;
using TMPro;
using System.Collections;

public class SpriteMaskTrigger : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject player;
    public float fadeInTime = 0.2f;
    public float fadeOutTime = 0.2f;
    public KeyCode triggerKey = KeyCode.E;

    private float lastPressTime;
    public float pressDelay = 1f;

    private Coroutine currentCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeIn());
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
    private void Update()
    {
        if (text.gameObject.activeSelf && Input.GetKeyDown(triggerKey) && player.GetComponent<Collider2D>().IsTouching(this.GetComponent<Collider2D>()) && Time.time - lastPressTime > pressDelay)
        {
            Debug.Log("E pressed");
            lastPressTime = Time.time;
        }
    }
}