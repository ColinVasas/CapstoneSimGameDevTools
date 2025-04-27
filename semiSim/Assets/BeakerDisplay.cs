using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeakerDisplay : MonoBehaviour
{
    private RecievingLiquidContainer recievingLiquidContainer;
    [SerializeField] private TextMeshProUGUI liquidAmount;
    [SerializeField] private TextMeshProUGUI detectedMixture;
    [SerializeField] private TextMeshProUGUI status;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        recievingLiquidContainer = GetComponent<RecievingLiquidContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(recievingLiquidContainer.GetMixture() != null)
        {
            detectedMixture.text = recievingLiquidContainer.GetMixture().solutionName.ToString() + " prepared";
        }
        else
        {
            detectedMixture.text = "Solution not prepared";
        }
        liquidAmount.text = recievingLiquidContainer.GetLiquidLevel().ToString() + "ml";

    }
}
