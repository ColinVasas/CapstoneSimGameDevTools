using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HotPlateInteract : MonoBehaviour
{
    public HotPlate hotPlate; 
    public Transform waferLocation;
    public GameObject wafer;
    public List<string> validTags = new List<string> {}; // Add more tags as needed
    Interactable interactable;

    public Renderer zoneRenderer; // Assign the Renderer of the trigger zone
    public Material triggerZoneColor1; // Red material
    public Material triggerZoneColor2; // Green material

    private int objectsInZone = 0;

    void Awake()
    {
        hotPlate = GetComponentInParent<HotPlate>();
    }

    private void Start()
    {
        if (zoneRenderer != null && triggerZoneColor1 != null)
        {
            zoneRenderer.material = triggerZoneColor1;
        }

        // Quaternion cusRotation = Quaternion.Euler(0, 0, 0);
        // Instantiate(wafer, waferLocation.position, cusRotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (validTags.Contains(other.tag))
        {
            objectsInZone++;
            //popupText.gameObject.SetActive(true);
            Destroy(other.gameObject);
            hotPlate.heatWafer();
            // Change to green if at least one object is inside
            if (zoneRenderer != null && triggerZoneColor2 != null)
            {
                zoneRenderer.material = triggerZoneColor2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (validTags.Contains(other.tag))
        {
            objectsInZone = Mathf.Max(0, objectsInZone - 1);
            if (objectsInZone == 0)
            {
                //popupText.gameObject.SetActive(false);

                // Change back to red when no objects remain
                if (zoneRenderer != null && triggerZoneColor1 != null)
                {
                    zoneRenderer.material = triggerZoneColor1;
                }
            }
        }
    }
}
