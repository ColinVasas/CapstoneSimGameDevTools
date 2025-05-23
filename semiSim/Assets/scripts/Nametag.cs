using UnityEngine;

public class Nametag : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
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