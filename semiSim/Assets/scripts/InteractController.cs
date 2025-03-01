using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    [SerializeField] private float pickupRange = 5.0f;
    pickUpController pickUpController;


    void Awake()
    {
        pickUpController = GetComponent<pickUpController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if(interactable != null)
                {
                    interactable.Interact();
                    // if(pickUpController.heldObj != null)
                    // {
                    //     if(pickUpController.heldObj.CompareTag(chuck)) {
                    //         interactable.ActivateChuck();
                    //     }
                    //     if(pickUpController.heldObj.CompareTag(wafer)) {
                    //         interactable.ActivateWafer();
                    //     }
                    // }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if(interactable != null)
                {
                    interactable.Interact();
                    // if(pickUpController.heldObj != null)
                    // {
                    //     if(pickUpController.heldObj.CompareTag(chuck)) {
                    //         interactable.ActivateChuck();
                    //     }
                    //     if(pickUpController.heldObj.CompareTag(wafer)) {
                    //         interactable.ActivateWafer();
                    //     }
                    // }
                }
            }
        }
    }
}
