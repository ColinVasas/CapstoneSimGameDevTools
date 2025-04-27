using UnityEngine;

public class WaferDippingManager : MonoBehaviour
{

    private ChemicalSolution detectedSolution;
    private RecievingLiquidContainer beaker;
    [SerializeField] EtchingMaterial currentWaferMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void StartEtch()
    {
        Debug.Log("Starting etch with " + detectedSolution + "against" + currentWaferMaterial);
        if (detectedSolution.etchingMaterial == currentWaferMaterial)
        {
            Debug.Log("The happy wafer is bathing in the right liquid");
        }
        else
        {
            Debug.Log("wafer is sad");
        }
    }
}
