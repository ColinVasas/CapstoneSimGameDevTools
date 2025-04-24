using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class failSafeObj : MonoBehaviour
{
     public LayerMask targetLayer;
     public Transform teleportTarget;
     public bool useFixedTeleport = false;

     private Dictionary<Collider, float> objectsInTrigger = new Dictionary<Collider, float>();

     private void OnTriggerEnter(Collider other)
     {
          if (((1 << other.gameObject.layer) & targetLayer) != 0)
          {
               TeleportObject(other);

               // Start tracking time for the object that entered the trigger
               if (!objectsInTrigger.ContainsKey(other))
               {
                    objectsInTrigger[other] = Time.time;
               }
          }
     }

     private void OnTriggerStay(Collider other)
     {
          if (((1 << other.gameObject.layer) & targetLayer) != 0)
          {
               // If the object is inside the trigger and its layer is correct, continue tracking time
               if (!objectsInTrigger.ContainsKey(other))
               {
                    objectsInTrigger[other] = Time.time;
               }

               // If the object has been inside for 1 second, teleport it again
               if (Time.time - objectsInTrigger[other] >= 1f)
               {
                    TeleportObject(other);
                    objectsInTrigger.Remove(other);  // Remove it after teleporting
               }
          }
     }

     private void OnTriggerExit(Collider other)
     {
          // Remove the object from the dictionary when it exits the trigger zone
          if (objectsInTrigger.ContainsKey(other))
          {
               objectsInTrigger.Remove(other);
          }
     }

     private void Update()
     {
          // Check if any object has been inside for more than 1 second
          foreach (var kvp in new List<KeyValuePair<Collider, float>>(objectsInTrigger))
          {
               if (Time.time - kvp.Value >= 1f)
               {
                    TeleportObject(kvp.Key);
                    objectsInTrigger.Remove(kvp.Key);
               }
          }
     }

     private void TeleportObject(Collider other)
     {
          Vector3 newPosition = useFixedTeleport && teleportTarget != null ? teleportTarget.position : other.transform.position + new Vector3(0, 5f, 0);
          other.transform.position = newPosition;

          if (other.attachedRigidbody != null)
          {
               other.attachedRigidbody.linearVelocity = Vector3.zero;
               other.attachedRigidbody.angularVelocity = Vector3.zero;
          }
     }
}
