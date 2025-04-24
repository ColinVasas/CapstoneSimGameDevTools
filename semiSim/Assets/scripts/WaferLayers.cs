using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WaferLayers : MonoBehaviour
{
    private bool running;
    
    private bool mask;
    
    private MeshRenderer meshRenderer;
    private List<Material> materials;

    private Texture2D maskTexture;
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        materials = new List<Material>(meshRenderer.materials);
    }

    public void Deposition(Material depositionMaterial, float transitionTime = 2.5f)
    {
        if (!running)
        {
            running = true;
            Debug.Log($"WaferLayers: Depositing layer{(mask ? " with mask " : "")}...");
        }
        else
        {
            Debug.LogWarning("WaferLayers: Deposition() called while already depositing!");
            return;
        }

        // Create instance of depositionMaterial so that changes don't persist to the original
        var material = new Material(depositionMaterial);
        
        // Set alpha to 0 to prepare for FadeIn()
        Color c = material.GetColor("_BaseColor");
        c.a = 0;
        material.SetColor("_BaseColor", c);
        
        // Apply mask texture if enabled
        if (mask)
        {
            // Create instance of maskTexture so that changes to maskTexture don't apply to past layers
            var _maskTexture = new Texture2D(
                maskTexture.width,
                maskTexture.height,
                maskTexture.format,
                maskTexture.mipmapCount > 0);
            Graphics.CopyTexture(maskTexture, _maskTexture);
            
            material.SetTexture("_BaseMap", _maskTexture);
            materials.Insert(materials.Count - 1, material);
        }
        else
            materials.Add(material);

        // Add material to the mesh renderer
        meshRenderer.SetMaterials(materials);
        
        // Start "deposition"
        StartCoroutine(Apply(transitionTime));
    }

    private IEnumerator Apply(float transitionTime)
    {
        float elapsedTime = 0;
        int materialIndex = meshRenderer.materials.Length - 1 - (mask ? 1 : 0);
        Color c = meshRenderer.materials[materialIndex].color;

        while (elapsedTime < transitionTime)
        {
            c.a = elapsedTime / transitionTime;
            meshRenderer.materials[materialIndex].color = c;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        c.a = 1;
        meshRenderer.materials[materialIndex].color = c;
        
        // Update material alpha of layer in local list
        materials[materialIndex].SetColor("_BaseColor", c);
        
        // If a layer is applied with no mask
        // then all previous layers are no longer visible
        // Therefore, all previous layers should be removed
        if (!mask)
        {
            materials = new List<Material> {
                meshRenderer.materials[materialIndex]
            };            
            meshRenderer.SetMaterials(materials);
        }

        running = false;
        
        Debug.Log("WaferLayers: Layer deposited!");
    }

    public void AddMask(Material material, Texture2D texture, Texture2D textureMask)
    {
        if (running)
        {
            Debug.LogWarning("WaferLayers: AddMask() called while already depositing!");
            return;
        }
        
        if (mask)
        {
            Debug.LogWarning("WaferLayers: AddMask() called with mask already applied!");
        }

        Debug.Log("WaferLayers: Added layer mask.");
        
        mask = true;
        maskTexture = texture;
        
        // Create instance of MaskMaterial so that changes don't persist to the original
        var _material = new Material(material);
        _material.SetTexture("_BaseMap", textureMask);
        
        // Add mask to the mesh renderer
        materials.Add(_material);
        meshRenderer.SetMaterials(materials);
    }
    
    public void RemoveMask()
    {
        if (running)
        {
            Debug.LogWarning("WaferLayers: RemoveMask() called while already depositing!");
            return;
        }

        if (!mask)
        {
            Debug.LogWarning("WaferLayers: RemoveMask() called without mask applied!");
            return;
        }
        
        Debug.Log("WaferLayers: Removed layer mask.");
        
        mask = false;
        maskTexture = null;

        // Remove mask from the mesh renderer
        materials.RemoveAt(materials.Count - 1);
        meshRenderer.SetMaterials(materials);
    }
}
