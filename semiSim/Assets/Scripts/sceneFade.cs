using UnityEngine;
using System.Collections;

public class sceneFade : MonoBehaviour
{
     public CanvasGroup fadeCanvas;
     public float fadeDuration = 1.0f;

     private void Start()
     {
          if (fadeCanvas != null)
          {
               fadeCanvas.alpha = 1; 
               StartCoroutine(Fade(0)); 
          }
     }

     private IEnumerator Fade(float targetAlpha)
     {
          float elapsedTime = 0f;

          while (elapsedTime < fadeDuration)
          {
               elapsedTime += Time.deltaTime;
               fadeCanvas.alpha = 1 - (elapsedTime / fadeDuration); 
               yield return null;
          }

          fadeCanvas.alpha = 0; 
     }
}
