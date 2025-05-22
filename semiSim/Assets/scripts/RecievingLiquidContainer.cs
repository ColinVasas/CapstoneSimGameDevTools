using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RecievingLiquidContainer : MonoBehaviour
{
    public List<ChemicalSolution> correctMixtures; // List of correct mixtures

    public Dictionary<LiquidType, Liquid> liquids = new Dictionary<LiquidType, Liquid>();
    private bool isPickedUp = false;
    public float maxCapacity = 1000f; // Maximum liquid volume



    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        
    }
    private void OnDestroy()
    {
        
    }
    public bool IsPickedUp()
    {
        Debug.Log(grabInteractable.isSelected);
        return grabInteractable.isSelected;
    }

    public void ReceiveLiquid(Liquid newLiquid)
    {
        Debug.Log("Recieved: " + newLiquid);
        if (liquids.ContainsKey(newLiquid.type))
        {
            if(GetLiquidLevel() + newLiquid.amount < maxCapacity)
                liquids[newLiquid.type].amount += newLiquid.amount; // Add to existing liquid amount
            else
            {
                Debug.Log(transform.name + " is full");
            }
        }
        else
        {
            liquids[newLiquid.type] = newLiquid; // Store new liquid type
        }
    }
    private void Update()
    {
        if(GetComponentInParent<PlayerMove>() != null && Input.GetKeyDown(KeyCode.E)) //Player is holding the thing and pressing E
        {
            IsCorrectMixture();
        }
    }
    public float GetMaxCapacity()
    {
        return maxCapacity;
    }
    public float GetLiquidLevel()
    {

        float totalAmount = 0;
        foreach (var liquid in liquids) {
            totalAmount += liquid.Value.amount;
        }
       // Debug.Log("total liquid amount: " + totalAmount);

        return totalAmount;
    }
    public bool IsCorrectMixture()
    {
        foreach (var mixture in correctMixtures)
        {
            if (MatchesMixture(mixture))
            {
                Debug.Log("Correct mixture detected: " + mixture.solutionName);
                return true;
            }
        }
        return false;
    }
    public ChemicalSolution GetMixture()
    {
        foreach (var mixture in correctMixtures)
        {
            if (MatchesMixture(mixture))
            {
                Debug.Log("Correct mixture detected: " + mixture.solutionName);
                return mixture;
            }
        }
        return null;
    }

    private bool MatchesMixture(ChemicalSolution mixture)
    {
        float totalAmount = GetLiquidLevel();
        if (totalAmount == 0) return false;

        // Calculate the current ratios of liquids in the container
        Dictionary<LiquidType, float> currentRatios = liquids.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.amount / totalAmount
        );

        // Calculate the expected ratios of the correct mixture
        Dictionary<LiquidType, float> expectedRatios = mixture.requiredLiquids.ToDictionary(
            liquid => liquid.type,
            liquid => (float)liquid.ratio / mixture.requiredLiquids.Sum(l => l.ratio)
        );

        // Define a tolerance (e.g., 15%)
        float tolerance = 0.15f;
        Debug.Log("the current ratios are" + currentRatios);
        // Check if all the ratios match within the tolerance margin
        foreach (var key in currentRatios.Keys)
        {
            if (!expectedRatios.ContainsKey(key))
            {
                // If the expected mixture requires a liquid that's not present, return false
                return false;
            }

            float currentRatio = currentRatios[key];
            float expectedRatio = expectedRatios[key];

            // Check if the absolute difference between the current ratio and expected ratio is within the tolerance margin
            if (Mathf.Abs(currentRatio - expectedRatio) > tolerance)
            {
                return false;  // If the difference is greater than the tolerance, it's not a match
            }
        }

        return true;  // If all checks pass, it's a correct mixture
    }

}
