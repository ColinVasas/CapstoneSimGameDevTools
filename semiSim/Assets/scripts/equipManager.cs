using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipManager : MonoBehaviour
{

     public GameObject firstObject; 
     public GameObject secondObject; 

     private void Start()
     {
          // Initialize tags: set the first object to "equip" and the second object to have no tag
          firstObject.tag = "equip";
          secondObject.tag = "Untagged";
     }

     public void HandleEquip(GameObject equippedObject)
     {
          // Check if the first object is being equipped
          if (equippedObject == firstObject && equippedObject.CompareTag("equip"))
          {
               Debug.Log("First object equipped.");

               // Disable the first object
               equippedObject.SetActive(false);

               // Enable the "equip" tag on the second object
               secondObject.tag = "equip";
               Debug.Log("Second object is now equippable.");
          }
     }
}
