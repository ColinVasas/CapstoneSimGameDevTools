using TMPro;
using UnityEngine;

public class EtchTimerDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text readout;
    [SerializeField] private WaferDippingManager etch;

    void Update()
    {
        if (etch == null || readout == null) return;

        if (etch.IsEtching)
        {
            float remain = Mathf.Max(0f, etch.EtchTime - etch.EtchProgress);
            readout.text = FormatMMSS(remain);
        }
        else
        {
            readout.text = "00:00";
        }
    }

    string FormatMMSS(float t)
    {
        int total = Mathf.CeilToInt(t);      // rounding uppies for display
        int minutes = total / 60;
        int seconds = total % 60;
        return $"{minutes:00}:{seconds:00}";
    }
}