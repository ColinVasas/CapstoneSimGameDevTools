using UnityEngine;

public class TaskHubManager : MonoBehaviour
{
    [Header("References")]
    public Transform spawnPoint;
    public GameObject wafer;
    public LayerDefinition[] possibleLayers;
    
    public void SpawnOrReset()
    {
        Debug.Log("[TaskHub] Button activated.");

        WaferQuest current = FindObjectOfType<WaferQuest>();

        // No wafer yet, spawn first one
        if (current == null)
        {
            Debug.Log("[TaskHub] No wafer in scene → spawning first wafer.");
            SpawnWafer();
            return;
        }

        // Finished wafer, scrap + replace
        if (current.isComplete)
        {
            Debug.Log("[TaskHub] Current wafer complete → destroying & respawning.");
            Destroy(current.gameObject);
            SpawnWafer();
            return;
        }

        // Unfinished wafer, just reset position
        Debug.Log("[TaskHub] Wafer still in progress → resetting position.");
        current.transform.SetPositionAndRotation(
            spawnPoint.position,
            spawnPoint.rotation);

        // zero any motion so it doesn’t yeet into the stratosphere
        if (current.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity        = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("[TaskHub] Unfinished wafer reset to table.");
    }
    
    private void SpawnWafer()
    {
        int idx = Random.Range(0, possibleLayers.Length);
        Debug.Log($"[TaskHub] Spawning wafer with quest = {possibleLayers[idx].displayName}");

        var waferGO = Instantiate(wafer,
            spawnPoint.position,
            spawnPoint.rotation);

        waferGO.GetComponent<WaferQuest>()
            .Initialise(possibleLayers[idx]);
    }
}