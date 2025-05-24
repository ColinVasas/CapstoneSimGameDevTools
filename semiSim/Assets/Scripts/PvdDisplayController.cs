using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PvdDisplayController : MonoBehaviour
{
    [SerializeField] private bool debug;
    
    [SerializeField] private TextMeshProUGUI materialLabel;
    [SerializeField] private MeshRenderer materialDishMaterialRenderer;
    [SerializeField] private List<Material> dishMaterials;

    [SerializeField] private WaferLayers waferLayers;
    
    private Material currentMaterial;
    private int currentMaterialIndex;
    
    private void Start()
    {
        UpdateDish();
    }

    public void Previous()
    {
        if (currentMaterialIndex == 0)
            currentMaterialIndex = dishMaterials.Count - 1;
        else
            currentMaterialIndex--;
        
        UpdateDish();
        
        if (debug)
            Debug.Log("Previous()");
    }

    public void Next()
    {
        if (currentMaterialIndex == dishMaterials.Count - 1)
            currentMaterialIndex = 0;
        else
            currentMaterialIndex++;
        
        UpdateDish();
        
        if (debug)
            Debug.Log("Next()");
    }

    private void UpdateDish()
    {
        currentMaterial = dishMaterials[currentMaterialIndex];
        materialLabel.text = dishMaterials[currentMaterialIndex].name;
        
        if (debug)
            Debug.Log("Material selected: " + dishMaterials[currentMaterialIndex].name);
        
        materialDishMaterialRenderer.material = currentMaterial;
    }

    public void Deposit()
    {
        waferLayers.Deposition(currentMaterial);
        
        if (debug)
            Debug.Log("Deposit()");
    }
}
