using UnityEngine;

[CreateAssetMenu(fileName="LayerDef",
    menuName="Semicon/Layer Definition")]
public class LayerDefinition : ScriptableObject
{
    public string displayName;
    public Material depositionMaterial;
}