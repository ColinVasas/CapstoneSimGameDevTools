using TMPro;
using UnityEngine;

public class WaferUI : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI waferNameText;
    [SerializeField] private TextMeshProUGUI waferMaterialText;
    [SerializeField] private TextMeshProUGUI instructionsText;
    [SerializeField] private TextMeshProUGUI etchTimeText;

    public void ShowWaferInfo(string waferName, EtchingMaterial material)
    {
        //Debug.Log(" ShowWaferInfo(String,Material) hit!");
       // waferNameText.text = $"Wafer: {waferName}";
        waferMaterialText.text = $"Material: {material}";
        instructionsText.text = GetInstructionsForMaterial(material);
        etchTimeText.text = GetEtchRange(material);
    }


    public void UpdateEtchProgress(float currentTime, float targetTime, EtchingMaterial material)
    {
        etchTimeText.text = $"{GetEtchRange(material)}\nCurrent: {currentTime:F1} / {targetTime:F1} sec";
    }

    private string GetInstructionsForMaterial(EtchingMaterial material)
    {
        return material switch
        {
            EtchingMaterial.Silicon => $"Use HF solution. \n5 Water to 1 KOH ",
            EtchingMaterial.SiliconDioxide => $"Use buffered HF. \n",
            EtchingMaterial.Aluminim => $"Use Phosphoric Acid.\n 3 Water to 1 HCL Etch",
            EtchingMaterial.Photoresist => $"Use Acetone.\n 1 Water to 1 KOH Etch",
            _ => "Unknown instructions."
        };
    }

    private string GetEtchRange(EtchingMaterial material)
    {
        return material switch
        {
            EtchingMaterial.Silicon => "Etch Time Range: 3–6 sec",
            EtchingMaterial.SiliconDioxide => "Etch Time Range: 2–6 sec",
            EtchingMaterial.Aluminim => "Etch Time Range: 2–4 sec",
            EtchingMaterial.Photoresist => "Etch Time Range: 1–3 sec",
            _ => "Etch Time Range: ?"
        };
    }
}
