using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipManager : MonoBehaviour
{

     public GameObject firstObject; 
     public GameObject secondObject; 

     private void Start()
     {
          firstObject.tag = "equip";
          secondObject.tag = "Untagged";
     }

     public void HandleEquip(GameObject equippedObject)
     {

          if (equippedObject == firstObject && equippedObject.CompareTag("equip"))
          {
               Debug.Log("First object equipped.");

               equippedObject.SetActive(false);

               secondObject.tag = "equip";
               Debug.Log("Second object is now equippable.");
          }
     }
}
