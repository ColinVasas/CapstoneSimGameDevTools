using System.Collections;
using UnityEngine;

public class ChemicalEtchMaterial : MonoBehaviour
{
    public EtchingMaterial etchingMaterial;
    private float etchTime;
    private float etchProgress = 0f;
    bool isEtching = false;  // To track if etching is ongoing
    bool isSuccessfulEtch = false;  // To track if etch was successful


    [Header("Set the global etch times for materials")]
    public static float AluminimEtchTime;
    public static float SiliconEtchTime;
    public static float SiliconDioxideEtchTime;
    public static float PhotoresistEtchTime;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEtchTime();
    }
    void SetEtchTime()
    {
        if (etchingMaterial == EtchingMaterial.Aluminim)
        {
            etchTime = AluminimEtchTime;
        }
        else if (etchingMaterial == EtchingMaterial.Silicon)
        {
            etchTime = SiliconEtchTime;
        }
        else if (etchingMaterial == EtchingMaterial.SiliconDioxide)
        {
            etchTime = SiliconDioxideEtchTime;
        }
        else if (etchingMaterial == EtchingMaterial.Photoresist)
        {
            etchTime = PhotoresistEtchTime;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void StartEtching()
    {
        if (!isEtching)
        {
            isEtching = true;
            StartCoroutine(Etch());
        }
    }

    IEnumerator Etch()
    {
        while (isEtching)
        {
            etchProgress += Time.deltaTime; // Increase progress over time
            yield return null; // Wait until next frame
        }
    }

    void StopEtching()
    {
        isEtching = false; // Stop the etching process

        // Check if the etchProgress is within 2 seconds of etchTime
        if (etchTime - etchProgress <= 2f && etchProgress <= etchTime)
        {
            isSuccessfulEtch = true; // Consider it a successful etch
            Debug.Log("Etch successful!");
        }
        else
        {
            isSuccessfulEtch = false; // Consider it a failed etch
            Debug.Log("Etch failed.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EtchingLiquid")
        {
            var container = other.GetComponentInParent<RecievingLiquidContainer>();
            if (container && container.GetMixture())
            {
                if (etchingMaterial == container.GetMixture().etchingMaterial)
                {
                    StartEtching();
                }
            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        StopEtching();
    }
}
