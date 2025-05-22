using System.Collections;
using UnityEngine;

public class WaferDippingManager : MonoBehaviour
{

    private ChemicalSolution detectedSolution;
    private RecievingLiquidContainer beaker;

    [SerializeField] private WaferUI waferUI;
    [SerializeField] EtchingMaterial currentWaferMaterial;

    [Header("Visuals")]
    [SerializeField] private ParticleSystem etchingSmoke;
   

    private float etchTime;
    private float etchProgress = 0f;
    private bool isEtching = false;
    private bool isSuccessfulEtch = false;

    [Header("Set the global etch times for materials")]
    
    public static float AluminimEtchTime;
    public static float SiliconEtchTime;
    public static float SiliconDioxideEtchTime;
    public static float PhotoresistEtchTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        SetEtchTime();
        waferUI.ShowWaferInfo(gameObject.name, currentWaferMaterial);
        Debug.Log("waferUI ref is: " + waferUI);

    }
    private void SetEtchTime()
    {
        if (currentWaferMaterial == EtchingMaterial.Aluminim)
            etchTime = AluminimEtchTime;
        else if (currentWaferMaterial == EtchingMaterial.Silicon)
            etchTime = SiliconEtchTime;
        else if (currentWaferMaterial == EtchingMaterial.SiliconDioxide)
            etchTime = SiliconDioxideEtchTime;
        else if (currentWaferMaterial == EtchingMaterial.Photoresist)
            etchTime = PhotoresistEtchTime;
    }
    // Update is called once per frame
    void Update()
    {
        waferUI.ShowWaferInfo(gameObject.name, currentWaferMaterial);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((beaker = other.GetComponentInParent<RecievingLiquidContainer>()) != null)
        {
            // Now beaker is assigned and confirmed not null
            if((detectedSolution = beaker.GetMixture()) != null){
                StartEtch();
            }
            else
            {
                Debug.Log("This mixture can't etch anything");
            }
           
        }
        else { Debug.Log(other + "detected"); }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<RecievingLiquidContainer>() == beaker)
        {
            StopEtch();
        }
    }
    private void StartEtch()
    {
        Debug.Log("Starting etch with " + detectedSolution + " against " + currentWaferMaterial);
        if (detectedSolution.etchingMaterial == currentWaferMaterial)
        {
            Debug.Log("The happy wafer is bathing in the right liquid.");
            if (!isEtching)
            {
                isEtching = true;
               
                waferUI.ShowWaferInfo(gameObject.name, currentWaferMaterial);
                StartCoroutine(Etch());

                if (etchingSmoke != null)
                {
                    etchingSmoke.Play();
                    etchingSmoke.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                }

            }
        }
        else
        {
            Debug.Log("Wafer is sad. Wrong liquid.");
        }
    }
    private IEnumerator Etch()
    {
        float elapsed = 0f;

        while (isEtching && elapsed <= etchTime)
        {
            waferUI.UpdateEtchProgress(elapsed, etchTime, currentWaferMaterial);
            elapsed += Time.deltaTime;
            yield return null;
        }

        waferUI.UpdateEtchProgress(etchTime, etchTime, currentWaferMaterial);
    }
    private void StopEtch()
    {
        if (isEtching)
        {
            isEtching = false;
            if (etchingSmoke != null) etchingSmoke.Stop();
            if (etchTime - etchProgress <= 2f && etchProgress <= etchTime)
            {
                isSuccessfulEtch = true;
                Debug.Log("Etch successful!");
            }
            else
            {
                isSuccessfulEtch = false;
                Debug.Log("Etch failed.");
            }
        }
    }

}
