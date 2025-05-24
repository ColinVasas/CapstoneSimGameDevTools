using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class ETCTriggerZone : MonoBehaviour
{
//public TextMeshProUGUI popupText;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    Interactable Interactable;

    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnInteract);
        Interactable = GetComponentInParent<Interactable>();
    }

    private void OnInteract(SelectEnterEventArgs args)
    {
        Debug.Log("Special object interacted with!");
        // Call your special function here
        Interactable.Interact();
    }

    private void ActivateSpecialFunction()
    {
        // Custom functionality, e.g., opening a door or triggering an event
        Debug.Log("Special function activated!");
    }
}
