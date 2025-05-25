using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;              

public class SinkWasher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecievingLiquidContainer targetBeaker;
    [SerializeField] private SnapZone snapZone;

    [Header("Input")]
    [SerializeField] private KeyCode washKey = KeyCode.E;
    
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
        if (Input.GetKeyDown(washKey))
            Wash();
        
        if (!targetDevice.isValid)
            GetDevice();

        if (targetDevice.isValid)
        {
            bool pressed = false;
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out pressed);

            if (pressed && !wasButtonPressed)
                Wash();

            wasButtonPressed = pressed;
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
        
        if (targetBeaker)
        {
            targetBeaker.DrainAll();
            Debug.Log("SinkWasher: Beaker emptied / solution refreshed.");
        }
        
        if (waferInSink.NeedsWashing)
        {
            waferInSink.Clean();
            Debug.Log($"SinkWasher: {waferInSink.name} washed successfully!");
        }
        else
        {
            Debug.Log("SinkWasher: Wafer already clean — skipped wash step.");
        }
    }
}
