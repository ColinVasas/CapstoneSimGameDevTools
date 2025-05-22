using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
     [Header("UI References")]
     public GameObject pauseMenuUI;
     public CanvasGroup darkOverlayCanvasGroup; // dark semi-transparent panel (alpha 0-0.2)
     public CanvasGroup buttonsCanvasGroup; // parent of buttons (alpha 0-1)

     [Header("Camera")]
     public GameObject cameraObject;

     private bool isPaused = false;
     private float fadeDuration = 0.2f;
     private Quaternion frozenRotation;

     void Start()
     {
          buttonsCanvasGroup.alpha = 0.0f;
          Resume(); // start unpaused
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

          // Lock camera rotation while paused
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

          // Fade in dark overlay (to 0.2 alpha) and buttons (to full alpha)
          StartCoroutine(FadeCanvasGroup(darkOverlayCanvasGroup, 0f, 0.2f, fadeDuration, true));
          StartCoroutine(FadeCanvasGroup(buttonsCanvasGroup, 0f, 1f, fadeDuration, true));
     }

     public void Resume()
     {
          StartCoroutine(FadeOutAndResume());
     }

     IEnumerator FadeOutAndResume()
     {
          // Fade buttons out fully then fade overlay out partially
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

     // Call this from your "Next Scene" button
     public void LoadNextScene()
     {
          Time.timeScale = 1f;
          int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

          // Optional: check if next scene exists, otherwise wrap or do nothing
          if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
               SceneManager.LoadScene(nextSceneIndex);
          else
               Debug.Log("No next scene available");

     }

     // Call this from your "Quit" button
     public void QuitGame()
     {
          Debug.Log("Quitting game...");
          Application.Quit();

#if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false;
#endif
     }
}
