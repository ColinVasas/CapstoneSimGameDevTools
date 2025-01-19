using UnityEngine;
using TMPro;

public class TriggerZoneText : MonoBehaviour
{
    public TextMeshProUGUI popupText;

    private void Start()
    {
        // Ensure the text starts hidden
        popupText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure player has the "Player" tag
        {
            popupText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupText.gameObject.SetActive(false);
        }
    }
}
