using UnityEngine;

public class WaferStatus : MonoBehaviour
{
    public bool NeedsWashing = false;
    public void Soil()  => NeedsWashing = true;
    public void Clean() => NeedsWashing = false;
}
