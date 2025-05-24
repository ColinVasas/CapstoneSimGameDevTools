using UnityEngine;

public class wetbenchTriggerZone : MonoBehaviour
{
   

    [Tooltip("Drag your Canvas or Panel GameObject here")]
    public GameObject uiRoot;

    void Start()
    {
        if (uiRoot != null)
            uiRoot.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            uiRoot?.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            uiRoot?.SetActive(false);
    }
}

