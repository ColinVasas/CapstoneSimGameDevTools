using System.Collections.Generic;
using UnityEngine;

public class RecievingLiquidContainer : MonoBehaviour
{
    public Dictionary<LiquidType, Liquid> liquids = new Dictionary<LiquidType, Liquid>();

    public void ReceiveLiquid(Liquid newLiquid)
    {
        Debug.Log("Recieved: " + newLiquid);
        if (liquids.ContainsKey(newLiquid.type))
        {
            liquids[newLiquid.type].amount += newLiquid.amount; // Add to existing liquid amount
        }
        else
        {
            liquids[newLiquid.type] = newLiquid; // Store new liquid type
        }
    }
    public float GetLiquidLevel()
    {

        float totalAmount = 0;
        foreach (var liquid in liquids) {
            totalAmount += liquid.Value.amount;
        }
        Debug.Log("total liquid amount: " + totalAmount);

        return totalAmount;
    }
}
