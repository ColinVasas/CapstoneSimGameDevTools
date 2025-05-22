using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
     public GameObject pauseMenuUI;

     private bool isPaused = false;

     void Start()
     {
          // Ensure the game starts unpaused
          Resume();
     }

     void Update()
     {
          if (Input.GetKeyDown(KeyCode.M))
          {

               if (isPaused)
                    Resume();
               else
                    Pause();
          }
     }

     public void Resume()
     {
          pauseMenuUI.SetActive(false);
          Time.timeScale = 1f;
          isPaused = false;
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;
     }

     void Pause()
     {
          pauseMenuUI.SetActive(true);
          Time.timeScale = 0f;
          isPaused = true;
          Cursor.lockState = CursorLockMode.None;
          Cursor.visible = true;
     }

     public void LoadNextScene()
     {
          // Unpause before loading next scene
          Time.timeScale = 1f;
          int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

          if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
          {
               SceneManager.LoadScene(nextSceneIndex);
          }
          else
          {
               Debug.LogWarning("No next scene found in build settings!");
          }
     }

     public void QuitGame()
     {
          Debug.Log("Quitting game...");
          Application.Quit();

#if UNITY_EDITOR
          // This is needed to "quit" the game when testing in the Unity Editor
          UnityEditor.EditorApplication.isPlaying = false;
#endif
     }
}
