using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
     [Header("UI References")]
     public GameObject pauseMenuUI;
     public CanvasGroup darkOverlayCanvasGroup; 
     public CanvasGroup buttonsCanvasGroup;
     public string nextSceneName; 

     [Header("Camera")]
     public GameObject cameraObject;

     private bool isPaused = false;
     private float fadeDuration = 0.2f;
     private Quaternion frozenRotation;

     void Start()
     {
          buttonsCanvasGroup.alpha = 0.0f;
          Resume(); 
     }

     void Update()
     {
          if (Input.GetKeyDown(KeyCode.Escape))
          {
               if (isPaused)
                    Resume();
               else
                    Pause();
          }

          if (isPaused && cameraObject != null)
          {
               cameraObject.transform.rotation = frozenRotation;
          }
     }

     void Pause()
     {
          pauseMenuUI.SetActive(true);
          Time.timeScale = 0f;
          isPaused = true;

          if (cameraObject != null)
               frozenRotation = cameraObject.transform.rotation;

          Cursor.lockState = CursorLockMode.None;
          Cursor.visible = true;

          StartCoroutine(FadeCanvasGroup(darkOverlayCanvasGroup, 0f, 0.4f, fadeDuration, true));
          StartCoroutine(FadeCanvasGroup(buttonsCanvasGroup, 0f, 1f, fadeDuration, true));
     }

     public void Resume()
     {
          StartCoroutine(FadeOutAndResume());
     }

     IEnumerator FadeOutAndResume()
     {
          yield return StartCoroutine(FadeCanvasGroup(buttonsCanvasGroup, 1f, 0f, fadeDuration, false));
          yield return StartCoroutine(FadeCanvasGroup(darkOverlayCanvasGroup, 0.4f, 0f, fadeDuration, false));

          pauseMenuUI.SetActive(false);
          Time.timeScale = 1f;
          isPaused = false;

          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;
     }

     IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration, bool enableInput)
     {
          float time = 0f;
          group.alpha = from;
          group.interactable = enableInput;
          group.blocksRaycasts = enableInput;

          while (time < duration)
          {
               group.alpha = Mathf.Lerp(from, to, time / duration);
               time += Time.unscaledDeltaTime;
               yield return null;
          }
          group.alpha = to;
     }


     public void LoadNextScene()
     {
          Time.timeScale = 1f;

          if (!string.IsNullOrEmpty(nextSceneName))
          {
               SceneManager.LoadScene(nextSceneName);
          }
          else
          {
               Debug.LogWarning("Next scene name not set!");
          }
     }


     public void QuitGame()
     {
          Debug.Log("Quitting game...");
          Application.Quit();

#if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false;
#endif
     }
}
