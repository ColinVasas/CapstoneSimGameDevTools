using TMPro;
using UnityEngine;
using System.Collections;

public class deskCanvasText : MonoBehaviour
{
    public TextMeshProUGUI equipText;  
    public float fadeDuration = 0.5f;
    public float displayDuration = 2.0f;
    public float delayBetweenTexts = 0.5f;

    private string[] messages = new string[]
    {
        "Welcome",
        "This is a\nsemiconductor clean room",
        "To begin, we will\nhave you change\ninto a gown",
        "This is a necessary step\nas cleanrooms are\nsensitive to particles",
        "Start by finding a hairnet\non the counter to the right",
        "Once you find it, left click\nto pick it up or put it down"
    };

    private Coroutine introCoroutine;
    private bool stopIntro = false;

    void Start()
    {
        equipText.text = "";
        equipText.color = new Color(equipText.color.r, equipText.color.g, equipText.color.b, 0); 
        equipText.gameObject.SetActive(false);

        introCoroutine = StartCoroutine(DisplayTextSequence());
    }

    private IEnumerator DisplayTextSequence()
    {
        foreach (string message in messages)
        {
            if (stopIntro) yield break;

            yield return StartCoroutine(DisplayTextMessage(message));
            yield return new WaitForSeconds(delayBetweenTexts);
        }
    }

    private IEnumerator DisplayTextMessage(string message)
    {
        equipText.text = message;
        equipText.gameObject.SetActive(true);

        yield return StartCoroutine(FadeText(0f, 1f, fadeDuration)); 
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeText(1f, 0f, fadeDuration)); 

        equipText.gameObject.SetActive(false);
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = equipText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            equipText.color = color;
            yield return null;
        }

        color.a = endAlpha;
        equipText.color = color;
    }

    // stop the intro text if we started picking up stuff
    public void StopIntro()
    {
        stopIntro = true;
        if (introCoroutine != null) StopCoroutine(introCoroutine);

        equipText.gameObject.SetActive(false);
        var c = equipText.color;
        c.a = 0;
        equipText.color = c;
    }
}
