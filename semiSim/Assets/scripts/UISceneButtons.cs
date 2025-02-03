using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISceneButtons : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject buttonPrefab;
    public SceneAsset[] scenes;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        foreach (var scene in scenes)
        {
            var buttonObject = Instantiate(buttonPrefab, menuPanel.transform);
            buttonObject.name = $"{scene.name}";
            buttonObject.GetComponentInChildren<TMP_Text>().SetText($"{scene.name}");

            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene(scene.name);
            });
        }
    }
}
