using UnityEngine;

public class WaferQuest : MonoBehaviour
{
    [HideInInspector] public LayerDefinition targetLayer;
    [HideInInspector] public bool isComplete;

    public void Initialise(LayerDefinition layer)
    {
        targetLayer = layer;
        isComplete  = false;
        Debug.Log($"Quest set: make {layer.displayName}");
    }
}