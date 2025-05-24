using UnityEngine;

public class WaferLayersDemo : MonoBehaviour
{
    public WaferLayers waferLayers;

    public Material FirstMaterial;
    public Material SecondMaterial;

    public Material MaskMaterial;
    public Texture2D MaskTexture;
    public Texture2D MaskTextureMask;
    
    public void A()
    {
        waferLayers.Deposition(FirstMaterial);
    }

    public void B()
    {
        waferLayers.AddMask(MaskMaterial, MaskTexture, MaskTextureMask);
    }

    public void C()
    {
        waferLayers.Deposition(SecondMaterial);
    }

    public void D()
    {
        waferLayers.RemoveMask();
    }
}
