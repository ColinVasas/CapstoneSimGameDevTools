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
        "This is a semiconductor clean room",
        "To begin we will have you change into a gown",
        "This is a necessary step as cleanrooms are prone to particles",
        "Why don’t you go find a hairnet… I think they’re on the counter to the right"
     };

     void Start()
     {
          equipText.text = "";
          equipText.color = new Color(equipText.color.r, equipText.color.g, equipText.color.b, 0); 
          equipText.gameObject.SetActive(false);

          StartCoroutine(DisplayTextSequence());
     }

     private IEnumerator DisplayTextSequence()
     {
          foreach (string message in messages)
          {
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
}
