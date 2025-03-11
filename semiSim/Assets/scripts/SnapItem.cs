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
        Debug.Log("Item released, isInSnapZone: " + isInSnapZone);
        if (!args.isCanceled && isInSnapZone && snapZone != null)
        {
            // Snap the objects pos and rotation to the snap point.
            Debug.Log("Attempting to snap to: " + snapZone.gameObject.name);
            transform.position = snapZone.snapPoint.position;
            transform.rotation = snapZone.snapPoint.rotation;
        }
    }
    public void TrySnap()
    {
        // Check that we're in a snap zone
        Debug.Log("TrySnap called, isInSnapZone: " + isInSnapZone);
        if (isInSnapZone && snapZone != null && snapZone.snapPoint != null)
        {
            // Snap the objects pos and rotation to the snap point.
            Debug.Log("Attempting to snap to: " + snapZone.gameObject.name);
            transform.position = snapZone.snapPoint.position;
            transform.rotation = snapZone.snapPoint.rotation;
            
            // Make the object kinematic so it stays in place
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            
            Debug.Log("Snapped to holder");
        }
    }
}


