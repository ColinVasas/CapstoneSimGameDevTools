using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSwitcher : MonoBehaviour
{
     public string scene1;
     public string scene2;
     public GameObject player;

     private static sceneSwitcher instance;
     private static Vector3 lastPlayerPosition;
     private string currentScene;

     private void Awake()
     {
          if (instance != null && instance != this)
          {
               Destroy(gameObject);
               return;
          }

          instance = this;
          DontDestroyOnLoad(gameObject);
     }

     private void Start()
     {
          currentScene = SceneManager.GetActiveScene().name;
          DontDestroyOnLoad(gameObject);

          Debug.Log($"Start: Current Scene = {currentScene}, Last Player Position = {lastPlayerPosition}");
     }

     private void Update()
     {
          if (Input.GetKeyDown(KeyCode.P))
          {
               SwitchScene();
          }
     }

     private void SwitchScene()
     {
          if (player != null)
          {
               lastPlayerPosition = player.transform.position;
          }

          string nextScene = currentScene == scene1 ? scene2 : scene1;
          SceneManager.sceneLoaded += OnSceneLoaded;
          SceneManager.LoadScene(nextScene);
          currentScene = nextScene;
     }

     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
     {
          SceneManager.sceneLoaded -= OnSceneLoaded;

          Debug.Log($"OnSceneLoaded: Loaded Scene = {scene.name}, Last Player Position = {lastPlayerPosition}");

          if (player != null)
          {
               player.transform.position = lastPlayerPosition;
          }
          else
          {
               GameObject newPlayer = GameObject.FindWithTag("Player");
               if (newPlayer != null)
               {
                    newPlayer.transform.position = lastPlayerPosition;
                    player = newPlayer;
               }
          }
     }
}
