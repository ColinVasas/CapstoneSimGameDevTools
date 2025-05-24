using UnityEngine;
using System.Collections;

public class roomTP : MonoBehaviour
{
     public GameObject playerObject;               // Assign the player GameObject directly
     public Transform teleportTarget;              // Empty GameObject as target location
     public float delayBeforeTeleport = 1f;        // Delay before teleporting

     private Coroutine teleportCoroutine = null;

     private void OnTriggerEnter(Collider other)
     {
          if (other.gameObject == playerObject)
          {
               teleportCoroutine = StartCoroutine(TeleportAfterDelay());
          }
     }

     private void OnTriggerExit(Collider other)
     {
          if (other.gameObject == playerObject && teleportCoroutine != null)
          {
               StopCoroutine(teleportCoroutine);
               teleportCoroutine = null;
          }
     }

     private IEnumerator TeleportAfterDelay()
     {
          yield return new WaitForSeconds(delayBeforeTeleport);

          // Confirm the player is still in the trigger
          Collider[] overlapping = Physics.OverlapSphere(transform.position, 1f);
          bool stillInTrigger = false;
          foreach (Collider col in overlapping)
          {
               if (col.gameObject == playerObject)
               {
                    stillInTrigger = true;
                    break;
               }
          }

          if (stillInTrigger && teleportTarget != null)
          {
               CharacterController controller = playerObject.GetComponent<CharacterController>();
               if (controller != null)
               {
                    controller.enabled = false;
                    playerObject.transform.position = teleportTarget.position;
                    controller.enabled = true;
               }
               else
               {
                    playerObject.transform.position = teleportTarget.position;
               }
          }

          teleportCoroutine = null;
     }
}
