using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Xml.Serialization;

public class equipManVr : MonoBehaviour
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

     public InputActionReference lC;
     public InputActionReference rC;

     private XRGrabInteractable currentObj;

     public GameObject textObject;  

     private int currentTextIndex = 0;


     private void Update()
     {
          if (currentObj != null && (IsButtonPressed(lC) || IsButtonPressed(rC)))
          {
               HandleEquip();
          }
     }

     private void Start()
     {
          textObject.SetActive(false);


          hairNet.tag = "equip";
          faceMask.tag = "Untagged";
          cleanHood.tag = "Untagged";
          goggles.tag = "Untagged";
          bunnySuit.tag = "Untagged";
          boots.tag = "Untagged";
          gloves.tag = "Untagged";
     }

     private bool IsButtonPressed(InputAction buttonAction)
     {
          return buttonAction.ReadValue<float>() > 0.5f;
     }

     public void PickedUp(GameObject obj)
     {
          currentObj = obj.GetComponent<XRGrabInteractable>();
     }

     public void Dropped()
     {
          currentObj = null;
     }

     private void HandleEquip()
     {
          if (currentObj == null || !currentObj.CompareTag("equip"))
          {
               EquipItem(null, null, "Woop's wrong Item!!!");
               return; 
          }

          GameObject equippedObject = currentObj.gameObject;

          if (equippedObject == hairNet)
               EquipItem(hairNet, faceMask, "Find and put on\nthe face mask");
          else if (equippedObject == faceMask)
               EquipItem(faceMask, cleanHood, "Now for the clean hood");
          else if (equippedObject == cleanHood)
               EquipItem(cleanHood, goggles, "Goggles are next");
          else if (equippedObject == goggles)
               EquipItem(goggles, bunnySuit, "Now for the hazmat suit");
          else if (equippedObject == bunnySuit)
               EquipItem(bunnySuit, boots, "Don't forget your boots");
          else if (equippedObject == boots)
               EquipItem(boots, gloves, "Lastly, put on some gloves");
          else if (equippedObject == gloves)
               EquipItem(gloves, null, "Alright. Now head to the door\nto enter the Yellow Room!");
     }

     private void EquipItem(GameObject current, GameObject next, string message)
     {
          current.SetActive(false);
          StartCoroutine(DisplayTextSequence(message));

          if (next != null)
          {
               next.tag = "equip";
          }
          else
          {
               trigger.SetActive(true);
          }
     }

     private IEnumerator DisplayTextSequence(string message)
     {
          textObject.SetActive(true);

          TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
          textComponent.text = message;

          yield return StartCoroutine(FadeText(textComponent, 0f, 1f, fadeDuration));
          yield return new WaitForSeconds(displayDuration);
          yield return StartCoroutine(FadeText(textComponent, 1f, 0f, fadeDuration));

          textObject.SetActive(false);
     }

     private IEnumerator FadeText(TextMeshProUGUI textComponent, float startAlpha, float endAlpha, float duration)
     {
          float elapsed = 0f;
          Color color = textComponent.color;

          while (elapsed < duration)
          {
               elapsed += Time.deltaTime;
               color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
               textComponent.color = color;
               yield return null;
          }

          color.a = endAlpha;
          textComponent.color = color;
     }
}
