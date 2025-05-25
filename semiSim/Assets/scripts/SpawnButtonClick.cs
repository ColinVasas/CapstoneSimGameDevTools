using UnityEngine;

public class SpawnButtonClick : MonoBehaviour
{
    public TaskHubManager hub;
    
    void OnMouseDown()
    {
        if (hub != null) hub.SpawnOrReset();
    }
}

