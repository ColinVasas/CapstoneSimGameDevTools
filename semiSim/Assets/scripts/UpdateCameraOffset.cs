using UnityEngine;
using UnityEngine.UI;

public class UpdateCameraOffset : MonoBehaviour
{
    public Slider SliderComponent;
    public Transform CameraOffsetTransform;

    public void UpdateOffset()
    {
        CameraOffsetTransform.SetLocalPositionAndRotation(
            new Vector3(0, SliderComponent.value, 0),
            Quaternion.identity
            );
    }
}
