using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TweezerGrabber : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gripPoint;
    [SerializeField] private Collider tweezersGripZone;
    [SerializeField] private XRNode controllerNode = XRNode.RightHand;
    
    // State tracking
    private bool isHoldingWafer = false;
    private GameObject heldWafer = null;
    private Rigidbody heldWaferRb = null;
    private GameObject currentWaferInRange = null;
    
    private XRGrabInteractable grabInteractable;
    private bool isTweezerHeld = false;
    private bool wasButtonPressed = false;
    
    // Input devices
    private readonly List<InputDevice> devices = new List<InputDevice>();
    private InputDevice targetDevice;
    
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        
        grabInteractable.selectEntered.AddListener(OnTweezerGrabbed);
        grabInteractable.selectExited.AddListener(OnTweezerReleased);
        
        // Set the trigger as a trigger
        if (tweezersGripZone != null)
        {
            tweezersGripZone.isTrigger = true;
        }
    }

    private void Update()
    {
        // Get the controller if we don't have it yet
        if (!targetDevice.isValid)
        {
            GetDevice();
        }

        bool isVRButtonPressed = false;
        if (targetDevice.isValid)
        {
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isVRButtonPressed);
        }

        // Check input if tweezers are held
        if (isTweezerHeld)
        {
            bool isKeyboardPressed = Input.GetKeyDown(KeyCode.E);
            bool isInteractionPressed = isVRButtonPressed && !wasButtonPressed;

            if ((isInteractionPressed || isKeyboardPressed))
            {
                if (!isHoldingWafer && currentWaferInRange != null)
                {
                    GrabWafer(currentWaferInRange);
                }
                else if (isHoldingWafer)
                {
                    ReleaseWafer();
                }
            }

            wasButtonPressed = isVRButtonPressed;
        }
    }

    private void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }
    
    private void OnTweezerGrabbed(SelectEnterEventArgs args)
    {
        isTweezerHeld = true;
    }
    
    private void OnTweezerReleased(SelectExitEventArgs args)
    {
        isTweezerHeld = false;
        
        // If we're still holding a wafer, release it
        if (isHoldingWafer)
        {
            ReleaseWafer();
        }
    }
    
    private void GrabWafer(GameObject wafer)
    {
        if (wafer == null || gripPoint == null) return;
        
        heldWafer = wafer;
        heldWaferRb = wafer.GetComponent<Rigidbody>();
        
        if (heldWaferRb != null)
        {
            // Store original state
            heldWaferRb.isKinematic = true;
            
            // Parent the wafer to the grip point
            wafer.transform.SetParent(gripPoint);
            wafer.transform.localPosition = Vector3.zero;
            wafer.transform.localRotation = Quaternion.identity;
            
            // Disable the XRGrabInteractable if it has one
            XRGrabInteractable waferGrabbable = wafer.GetComponent<XRGrabInteractable>();
            if (waferGrabbable != null)
            {
                waferGrabbable.enabled = false;
            }
            
            isHoldingWafer = true;
            
            Debug.Log("Wafer grabbed by tweezers");
        }
    }
    
    public void ReleaseWafer()
    {
        if (!isHoldingWafer || heldWafer == null) return;
    
        // Get the SnapItem component before unparenting
        SnapItem snapItem = heldWafer.GetComponent<SnapItem>();
    
        // Unparent the wafer
        heldWafer.transform.SetParent(null);
    
        // Restore physics
        if (heldWaferRb != null)
        {
            heldWaferRb.isKinematic = false;
        }
    
        // Re-enable the XRGrabInteractable
        XRGrabInteractable waferGrabbable = heldWafer.GetComponent<XRGrabInteractable>();
        if (waferGrabbable != null)
        {
            waferGrabbable.enabled = true;
        }
    
        // Try to snap if in snap zone
        if (snapItem != null)
        {
            snapItem.TrySnap();
        }
    
        isHoldingWafer = false;
        heldWafer = null;
        heldWaferRb = null;
    
        Debug.Log("Wafer released from tweezers");
    }
    
    // Track wafers entering the grip zone
    private void OnTriggerEnter(Collider other)
    {
        if (!isTweezerHeld) return;
        
        // Check if it's a wafer by tag
        if (other.CompareTag("Wafer") && !isHoldingWafer)
        {
            currentWaferInRange = other.gameObject;
            Debug.Log("Wafer in tweezers range");
        }
    }
    
    // Track wafers exiting the grip zone
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentWaferInRange)
        {
            currentWaferInRange = null;
            Debug.Log("Wafer left tweezers range");
        }
    }
}
