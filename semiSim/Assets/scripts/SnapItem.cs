using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapItem : MonoBehaviour
{
    // Set true when the item is in a snap zone
    public bool isInSnapZone = false;
    
    public SnapZone snapZone;
    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    // Called when object is released
    private void OnSelectExited(SelectExitEventArgs args)
    {
        // Check that the exit event is not canceled and that we're in a snap zone
        if (!args.isCanceled && isInSnapZone && snapZone != null)
        {
            // Snap the objects pos and rotation to the snap point.
            transform.position = snapZone.snapPoint.position;
            transform.rotation = snapZone.snapPoint.rotation;
        }
    }
}


