using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;                 // ← XR input (same names as in TweezerGrabber)

public class SinkWasher : MonoBehaviour
{
    [Header("Optional reference")]
    public SnapZone snapZone;
    
    [SerializeField] private XRNode controllerNode = XRNode.RightHand;
    private readonly List<InputDevice> devices = new();
    private InputDevice targetDevice;
    private bool wasButtonPressed = false;
    
    private WaferStatus waferInSink;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wafer"))
        {
            waferInSink = other.GetComponent<WaferStatus>();
            Debug.Log($"SinkWasher: {other.name} entered sink trigger.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wafer") && waferInSink &&
            other.gameObject == waferInSink.gameObject)
        {
            Debug.Log($"SinkWasher: {other.name} left sink trigger.");
            waferInSink = null;
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Wash();
        
        if (!targetDevice.isValid)
            GetDevice();                       // fetch controller once

        if (targetDevice.isValid)
        {
            bool isPressed = false;
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);

            if (isPressed && !wasButtonPressed)
            {
                Debug.Log("SinkWasher: A-button pressed.");
                Wash();
            }
            wasButtonPressed = isPressed;
        }
    }
    
    private void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log($"SinkWasher: bound to {targetDevice.characteristics}");
        }
    }
    
    public void Wash()
    {
        if (!waferInSink)
        {
            Debug.Log("SinkWasher: No wafer in sink.");
            return;
        }

        if (!waferInSink.NeedsWashing)
        {
            Debug.Log("SinkWasher: Wafer is already clean.");
            return;
        }

        waferInSink.Clean();
        Debug.Log($"SinkWasher: {waferInSink.name} washed successfully!");
    }
}
