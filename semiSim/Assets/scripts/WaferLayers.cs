using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Runtime.CompilerServices;
=======
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
using UnityEngine;

public class WaferLayers : MonoBehaviour
{
<<<<<<< HEAD
=======
    [SerializeField] private bool debug;
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
    private bool running;
    
    private bool mask;
    
    private MeshRenderer meshRenderer;
    private List<Material> materials;

    private Texture2D maskTexture;
    
<<<<<<< HEAD
    void Start()
=======
    private void Start()
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
    {
        meshRenderer = GetComponent<MeshRenderer>();
        materials = new List<Material>(meshRenderer.materials);
    }

    public void Deposition(Material depositionMaterial, float transitionTime = 2.5f)
    {
        if (!running)
        {
            running = true;
<<<<<<< HEAD
            Debug.Log($"WaferLayers: Depositing layer{(mask ? " with mask " : "")}...");
=======
            if (debug)
                Debug.Log($"WaferLayers: Depositing layer{(mask ? " with mask " : "")}...");
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
        }
        else
        {
            Debug.LogWarning("WaferLayers: Deposition() called while already depositing!");
            return;
        }

        // Create instance of depositionMaterial so that changes don't persist to the original
        var material = new Material(depositionMaterial);
        
        // Set alpha to 0 to prepare for FadeIn()
<<<<<<< HEAD
        Color c = material.GetColor("_BaseColor");
        c.a = 0;
        material.SetColor("_BaseColor", c);
=======
        var c = material.GetColor(BaseColor);
        c.a = 0;
        material.SetColor(BaseColor, c);
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
        
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
            
<<<<<<< HEAD
            material.SetTexture("_BaseMap", _maskTexture);
=======
            material.SetTexture(BaseMap, _maskTexture);
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
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
<<<<<<< HEAD
        int materialIndex = meshRenderer.materials.Length - 1 - (mask ? 1 : 0);
        Color c = meshRenderer.materials[materialIndex].color;
=======
        var materialIndex = meshRenderer.materials.Length - 1 - (mask ? 1 : 0);
        var c = meshRenderer.materials[materialIndex].color;
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6

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
<<<<<<< HEAD
        materials[materialIndex].SetColor("_BaseColor", c);
=======
        materials[materialIndex].SetColor(BaseColor, c);
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
        
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
        
<<<<<<< HEAD
        Debug.Log("WaferLayers: Layer deposited!");
=======
        if (debug)
            Debug.Log("WaferLayers: Layer deposited!");
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
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

<<<<<<< HEAD
        Debug.Log("WaferLayers: Added layer mask.");
=======
        if (debug)
            Debug.Log("WaferLayers: Added layer mask.");
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
        
        mask = true;
        maskTexture = texture;
        
        // Create instance of MaskMaterial so that changes don't persist to the original
        var _material = new Material(material);
<<<<<<< HEAD
        _material.SetTexture("_BaseMap", textureMask);
=======
        _material.SetTexture(BaseMap, textureMask);
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
        
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
<<<<<<< HEAD
        
        Debug.Log("WaferLayers: Removed layer mask.");
=======

        if (debug)
            Debug.Log("WaferLayers: Removed layer mask.");
>>>>>>> c65e585d381111c8d2e3eb6bcfe9c79794ca04f6
        
        mask = false;
        maskTexture = null;

        // Remove mask from the mesh renderer
        materials.RemoveAt(materials.Count - 1);
        meshRenderer.SetMaterials(materials);
    }
}
