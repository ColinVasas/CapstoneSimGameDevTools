using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class equipManager : MonoBehaviour
{

     public GameObject hairNet; 
     public GameObject faceMask;
     public GameObject cleanHood;
     public GameObject goggles;
     public GameObject bunnySuit;
     public GameObject boots;
     public GameObject gloves;

     public GameObject trigger;

     public TextMeshProUGUI equipText;
     public float fadeDuration = .5f;
     public float displayDuration = 1.5f;

     private void Start()
     {
          hairNet.tag = "equip";
          faceMask.tag = "Untagged";
          cleanHood.tag = "Untagged";
          goggles.tag = "Untagged";
          bunnySuit.tag = "Untagged";
          boots.tag = "Untagged";
          gloves.tag = "Untagged";

          trigger.SetActive(false);
     }

     public void HandleEquip(GameObject equippedObject)
     {
          if (!equippedObject.CompareTag("equip")) return; 

          switch (equippedObject)
          {
               case var obj when obj == hairNet:
                    EquipItem(hairNet, faceMask, "HairNet Equipped!");
                    break;

               case var obj when obj == faceMask:
                    EquipItem(faceMask, cleanHood, "FaceMask Equipped!");
                    break;

               case var obj when obj == cleanHood:
                    EquipItem(cleanHood, goggles, "CleanHood Equipped!");
                    break;

               case var obj when obj == goggles:
                    EquipItem(goggles, bunnySuit, "Goggles Equipped!");
                    break;

               case var obj when obj == bunnySuit:
                    EquipItem(bunnySuit, boots, "BunnySuit Equipped!");
                    break;

               case var obj when obj == boots:
                    EquipItem(boots, gloves, "Boots Equipped!");
                    break;

               case var obj when obj == gloves:
                    trigger.SetActive(true);
                    EquipItem(gloves, null, "Gloves Equipped!");
                    break;

               default:
                    Debug.Log("Invalid equipment sequence.");
                    break;
          }
     }

     private void EquipItem(GameObject current, GameObject next, string message)
     {
          current.SetActive(false);
          if (next != null) next.tag = "equip";
          StartCoroutine(ShowEquipText(message));
     }

     private IEnumerator ShowEquipText(string message)
     {
          equipText.text = message;
          equipText.gameObject.SetActive(true);
          yield return StartCoroutine(FadeText(0f, 1f, fadeDuration));
          yield return new WaitForSeconds(displayDuration);
          yield return StartCoroutine(FadeText(1f, 0f, fadeDuration));
          equipText.gameObject.SetActive(false);
     }

     private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
     {
          float elapsed = 0f;
          Color color = equipText.color;

          while (elapsed < duration)
          {
               elapsed += Time.deltaTime;
               color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
               equipText.color = color;
               yield return null;
          }

          color.a = endAlpha; 
          equipText.color = color;
     }
}

