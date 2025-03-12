using System.Collections.Generic;
using UnityEngine;

public enum EtchingMaterial
{
    Silicon,
    SiliconDioxide,
    Aluminim,
    Photoresist
}

[CreateAssetMenu(fileName = "ChemicalSolution", menuName = "Scriptable Objects/ChemicalSolution")]
public class ChemicalSolution : ScriptableObject
{
    public string solutionName;
    public EtchingMaterial etchingMaterial; //Which material can these solution etch?
    public List<LiquidPortion> requiredLiquids;
}

[System.Serializable]
public class LiquidPortion
{
    public LiquidType type;
    public int ratio; // Portion of the mixture
}
