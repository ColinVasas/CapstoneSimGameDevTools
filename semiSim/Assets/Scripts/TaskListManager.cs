using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TaskListUI : MonoBehaviour
{
    [System.Serializable]
    public class Task
    {
        public string taskName;
        public bool isComplete = false;
        public GameObject uiElement; // Link to the instantiated UI item
    }

    public Transform taskListContainer; // The parent with Vertical Layout Group
    public GameObject taskItemPrefab;   // The prefab we instantiate
    private List<Task> tasks = new List<Task>();

    // Initial tasks
    // private string[] initialTasks = { "Goggles", "Gown", "Shoes", "Gloves", "Face Mask" };

    // void Start()
    // {
    //     foreach (string item in initialTasks)
    //     {
    //         AddTask(item);
    //     }
    // }

    public void AddTask(string taskName)
    {
        GameObject taskUI = Instantiate(taskItemPrefab, taskListContainer);
        TextMeshProUGUI text = taskUI.GetComponentInChildren<TextMeshProUGUI>();
        text.text = taskName;

        tasks.Add(new Task
        {
            taskName = taskName,
            uiElement = taskUI
        });
    }

    public void CompleteTask(string taskName)
    {
        foreach (var task in tasks)
        {
            if (task.taskName == taskName && !task.isComplete)
            {
                task.isComplete = true;
                // You can change the appearance here:
                TextMeshProUGUI text = task.uiElement.GetComponentInChildren<TextMeshProUGUI>();
                text.text = $"<s>{task.taskName}</s>";
                text.color = Color.green; // or strikethrough, fade, etc.
                break;
            }
        }
    }
}
