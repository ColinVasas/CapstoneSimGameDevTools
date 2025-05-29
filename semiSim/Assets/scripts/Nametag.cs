// This code controls the nametags on the yellow room bottles.
// This way, the player will be able to tell which bottle is
// the PMMA and which bottle is the PI. The code makes sure the
// nametags on the bottles are always facing the player.

using UnityEngine;

public class Nametag : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // defaults to having nametags face camera with the tag "MainCamera"
        // if code isn't working, verify that the scene's camera has the correct tag
        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
        else
            Debug.LogError("Nametag: No Camera tagged MainCamera in scene!");
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // compute direction from camera to nametag
        Vector3 dir = transform.position - cameraTransform.position;
        // create rotation looking down that vector
        Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
        // set rotation
        transform.rotation = lookRotation;
    }
}