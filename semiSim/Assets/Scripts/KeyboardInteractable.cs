using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyboardInteractable : MonoBehaviour
{
    private XRGrabInteractable xrGrabInteractable;  // Reference to the XRGrabInteractable component

    private void Start()
    {
        // Try to get the XRGrabInteractable component attached to the same GameObject
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void OnPickUpEnter()
    {
        if (xrGrabInteractable != null)
        {
            // Check if the SelectEntered event has listeners
            if (xrGrabInteractable.selectEntered != null)
            {
                // Manually invoke the SelectEntered event for XRGrabInteractable
                xrGrabInteractable.selectEntered.Invoke(new SelectEnterEventArgs { interactableObject = xrGrabInteractable });
            }
        }
    }

   
    public void OnPickUpExit()
    {
        if (xrGrabInteractable != null)
        {
            // Check if the SelectExited event has listeners
            if (xrGrabInteractable.selectExited != null)
            {
                // Manually invoke the SelectExited event for XRGrabInteractable
                xrGrabInteractable.selectExited.Invoke(new SelectExitEventArgs { interactableObject = xrGrabInteractable });
            }
        }
    }

    // Call this in Update from PickupController to manage hover events
    public void OnHoverEnter()
    {
      
            if (xrGrabInteractable != null && xrGrabInteractable.hoverEntered != null)
            {
                xrGrabInteractable.hoverEntered.Invoke(new HoverEnterEventArgs { interactableObject = xrGrabInteractable });
            }
        
    }

    // This method is called by PickupController when no longer hovering
    public void OnHoverExit()
    {
        
            if (xrGrabInteractable != null && xrGrabInteractable.hoverExited != null)
            {
                xrGrabInteractable.hoverExited.Invoke(new HoverExitEventArgs { interactableObject = xrGrabInteractable });
            }
       
    }
}
