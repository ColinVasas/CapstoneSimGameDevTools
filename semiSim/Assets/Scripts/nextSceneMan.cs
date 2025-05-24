using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class nextSceneMan : MonoBehaviour
{
     public string nextSceneName;
     public CanvasGroup fadeCanvas;
     public float fadeDuration = 1.0f;


     private void OnTriggerEnter(Collider other)
     {
          Debug.Log($"Trigger entered by: {other.gameObject.name}, Tag: {other.gameObject.tag}");

          if (other.CompareTag("Player")) 
          {
               Debug.Log("Player detected! Loading next scene...");
               StartCoroutine(LoadNextScene());
          }
     }

     private IEnumerator LoadNextScene()
     {
          if (fadeCanvas != null)
          {
               yield return StartCoroutine(Fade(1)); 
          }

          Debug.Log($"Loading scene: {nextSceneName}");
          SceneManager.LoadScene(nextSceneName);

          if (fadeCanvas != null)
          {
               yield return StartCoroutine(Fade(0)); 
          }
     }

     private IEnumerator Fade(float targetAlpha)
     {
          float startAlpha = fadeCanvas.alpha;
          float elapsedTime = 0f;

          while (elapsedTime < fadeDuration)
          {
               elapsedTime += Time.deltaTime;
               fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
               yield return null;
          }

          fadeCanvas.alpha = targetAlpha;
     }
}
