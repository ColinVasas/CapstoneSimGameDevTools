using UnityEngine;
using System.Collections.Generic;

public class LiquidVisual : MonoBehaviour
{
    private RecievingLiquidContainer container;
    public Transform liquidTransform; // Reference to the liquid object
    public float maxFillHeight = 1f; // Maximum liquid height
    public float minFillHeight = 0.1f; // Minimum liquid height
    public float maxCapacity = 1000f; // Maximum liquid volume

    // Color definitions
    private Color waterColor = Color.blue;
    private Color acidColor = Color.green;
    private Color hclColor = Color.red;
    private Color defaultColor = Color.white; // Default color if no liquid is present

    private void Start()
    {
        container = GetComponent<RecievingLiquidContainer>();
    }
    void Update()
    {
        if (container.liquids.Count > 0)
        {
            UpdateLiquidLevel(GetTotalLiquidAmount());
            UpdateLiquidColor();
        }
        else
        {
            liquidTransform.localScale = new Vector3(1, 0, 1); // Hide if empty
        }
    }

    void UpdateLiquidLevel(float totalAmount)
    {
        float fillPercent = Mathf.Clamp01(totalAmount / maxCapacity);
        float newY = Mathf.Lerp(minFillHeight, maxFillHeight, fillPercent);

        liquidTransform.localScale = new Vector3(1, fillPercent, 1);
        liquidTransform.localPosition = new Vector3(0, newY, 0);
    }

    void UpdateLiquidColor()
    {
        Color mixedColor = defaultColor;
        float totalAmount = GetTotalLiquidAmount();
        List<Color> validLiquids = new List<Color>();

        // Loop through each liquid type in the container and apply color mixing
        foreach (var liquid in container.liquids)
        {
            if (liquid.Value.amount > 10f) // Only count liquids with more than 10 mL
            {
                float percentage = liquid.Value.amount / totalAmount;

                // Mix colors based on liquid type
                switch (liquid.Key)
                {
                    case LiquidType.Water:
                        validLiquids.Add(waterColor * percentage);
                        break;
                    case LiquidType.Acid:
                        validLiquids.Add(acidColor * percentage);
                        break;
                    case LiquidType.Hcl:
                        validLiquids.Add(hclColor * percentage);
                        break;
                    default:
                        break;
                }
            }
        }

        // If there are valid liquids, mix the colors
        if (validLiquids.Count > 0)
        {
            mixedColor = MixColors(validLiquids);
        }

        // Set the color of the liquid object
        liquidTransform.GetComponent<Renderer>().material.color = mixedColor;
    }

    float GetTotalLiquidAmount()
    {
        float totalAmount = 0f;
        foreach (var liquid in container.liquids)
        {
            if (liquid.Value.amount > 10f) // Only count liquids with more than 10 mL
            {
                totalAmount += liquid.Value.amount;
            }
        }
        return totalAmount;
    }

    Color MixColors(List<Color> colors)
    {
        Color mixedColor = Color.black;

        foreach (Color color in colors)
        {
            mixedColor += color;
        }

        return mixedColor;
    }
}

