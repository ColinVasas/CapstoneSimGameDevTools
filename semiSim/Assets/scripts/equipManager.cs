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
     public TextMeshProUGUI equipText; // Single TMP UI element for all messages
     public float fadeDuration = 0.5f;
     public float displayDuration = 1.5f;

     private string[] equipMessages = new string[]
     {
        "Face Masks should be in the bin next to the hairnets",
        "Good Good…. You got a locker assigned to you",
        "You should find a fresh clean hood, goggles, and “bunny suit”",
        "Lookin good so far, Last few things, on that bench over there…",
        "There's a pair of booties, please put them over your shoes",
        "Lastly, put on some gloves",
        "Alright…. Are you ready to head in the Clean Room?"
     };

     private int currentMessageIndex = 0;

     private void Start()
     {
          equipText.gameObject.SetActive(false);
          Color textColor = equipText.color;
          textColor.a = 0f;
          equipText.color = textColor;

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
                    EquipItem(hairNet, faceMask);
                    break;
               case var obj when obj == faceMask:
                    EquipItem(faceMask, cleanHood);
                    break;
               case var obj when obj == cleanHood:
                    EquipItem(cleanHood, goggles);
                    break;
               case var obj when obj == goggles:
                    EquipItem(goggles, bunnySuit);
                    break;
               case var obj when obj == bunnySuit:
                    EquipItem(bunnySuit, boots);
                    break;
               case var obj when obj == boots:
                    EquipItem(boots, gloves);
                    break;
               case var obj when obj == gloves:
                    trigger.SetActive(true);
                    EquipItem(gloves, null);
                    break;
               default:
                    Debug.Log("Invalid equipment sequence.");
                    break;
          }
     }

     private void EquipItem(GameObject current, GameObject next)
     {
          current.SetActive(false);
          if (next != null) next.tag = "equip";

          if (currentMessageIndex < equipMessages.Length)
          {
               StartCoroutine(DisplayTextMessage(equipMessages[currentMessageIndex]));
               currentMessageIndex++;
          }
     }

     private IEnumerator DisplayTextMessage(string message)
     {
          equipText.text = message;
          equipText.gameObject.SetActive(true);

          yield return StartCoroutine(FadeText(0f, 1f, fadeDuration)); // Fade in
          yield return new WaitForSeconds(displayDuration);
          yield return StartCoroutine(FadeText(1f, 0f, fadeDuration)); // Fade out

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
