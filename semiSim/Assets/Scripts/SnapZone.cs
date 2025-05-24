using UnityEngine;

public class SnapZone : MonoBehaviour
{
    public Transform snapPoint;

    // When an object enters the trigger its notified that it's in a snap zone
    private void OnTriggerEnter(Collider other)
    {
        SnapItem snapItem = other.GetComponent<SnapItem>();
        if (snapItem != null)
        {
            snapItem.isInSnapZone = true;
            snapItem.snapZone = this;  // Let the item know which zone it's in.
            Debug.Log("SnapItem detected, setting isInSnapZone to TRUE");
        }
    }

    // When the object leaves the trigger no snapping allowed
    private void OnTriggerExit(Collider other)
    {
        SnapItem snapItem = other.GetComponent<SnapItem>();
        if (snapItem != null)
        {
            snapItem.isInSnapZone = false;
            snapItem.snapZone = null;
        }
    }
}
