using UnityEngine;
using System.Collections;

public class FadeInLast : MonoBehaviour
{
    public float fadeInTime = 0.2f;

    public IEnumerator FadeInLastDisabledObject()
    {
        // get the sprite renderer component of the last disabled object
        SpriteRenderer spriteRenderer = LastDisabledObject.lastDisabledObject.GetComponent<SpriteRenderer>();

        // make the object active so it can fade in
        LastDisabledObject.lastDisabledObject.SetActive(true);

        // set the alpha value to 0 before fading in
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);

        // fade in the object
        float t = 0f;
        Color color = spriteRenderer.color;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeInTime);
            spriteRenderer.color = color;
            yield return null;
        }
    }
}