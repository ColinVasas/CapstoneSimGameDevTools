using UnityEngine;
using System.Collections.Generic;

public class TaskInitializer : MonoBehaviour
{
    public TaskListUI taskListUI;

    [SerializeField] private List<string> taskDescriptions = new List<string>
    {
        "Open spincoater lid",
        "Place chuck",
        "Place cleaned wafer",
        "Turn on vacuum",
        "Apply PMMA",
        "Start spincoater",
        "Use hotplate/return wafer",
        "Apply PI",
        "Final spincoater run",
        "Head to the wet bench room"
    };

    void Start()
    {
        Debug.Log("TaskInitializer Start called");
        foreach (string task in taskDescriptions)
        {
            Debug.Log("Adding task: " + task);
            taskListUI?.AddTask(task);
        }
    }

    public TaskListUI GetTaskListUI()
    {
        return taskListUI;
    }
}
