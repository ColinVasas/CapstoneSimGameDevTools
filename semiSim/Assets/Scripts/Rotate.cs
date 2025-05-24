using UnityEngine;

public class Rotate : MonoBehaviour
{
    private void Update()
    {
        this.transform.RotateAround(
            this.transform.position,
            Vector3.up,
            Time.deltaTime * 10f
            );
        
        this.transform.RotateAround(
            this.transform.position,
            Vector3.right,
            Time.deltaTime * 7f
        );
        
        this.transform.RotateAround(
            this.transform.position,
            Vector3.forward,
            Time.deltaTime * 5f
        );
    }
}
