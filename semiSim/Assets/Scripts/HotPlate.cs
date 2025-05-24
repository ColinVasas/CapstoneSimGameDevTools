using System.Collections;
using UnityEngine;

public class HotPlate : MonoBehaviour
{
    public Transform waferLocation;
    public GameObject blankWafer;
    public GameObject wafer;
    public GameObject placeHolder;
    public GameObject triggerZonePlate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void heatWafer()
    {
        Quaternion cusRotation = Quaternion.Euler(0, 0, 0);
        triggerZonePlate.SetActive(false);
        placeHolder = Instantiate(blankWafer, waferLocation.position, cusRotation);
        StartCoroutine(currentlyHeating(placeHolder));
    }

    IEnumerator currentlyHeating(GameObject dummy)
    {
        Debug.Log("wait 5 seconds");
        yield return new WaitForSeconds(5f);
        Debug.Log("Done");
        Destroy(dummy);
        Quaternion cusRotation = Quaternion.Euler(0, 0, 0);
        Instantiate(wafer, waferLocation.position, cusRotation);
    }
}
