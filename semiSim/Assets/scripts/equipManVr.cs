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

     public GameObject[] textObjects;  
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
          foreach (GameObject textObject in textObjects)
          {
               textObject.SetActive(false);
          }

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
          if (currentObj == null || !currentObj.CompareTag("equip")) return;

          GameObject equippedObject = currentObj.gameObject;

          if (equippedObject == hairNet)
               EquipItem(hairNet, faceMask, "HairNet Equipped!");
          else if (equippedObject == faceMask)
               EquipItem(faceMask, cleanHood, "FaceMask Equipped!");
          else if (equippedObject == cleanHood)
               EquipItem(cleanHood, goggles, "CleanHood Equipped!");
          else if (equippedObject == goggles)
               EquipItem(goggles, bunnySuit, "Goggles Equipped!");
          else if (equippedObject == bunnySuit)
               EquipItem(bunnySuit, boots, "BunnySuit Equipped!");
          else if (equippedObject == boots)
               EquipItem(boots, gloves, "Boots Equipped!");
          else if (equippedObject == gloves)
               EquipItem(gloves, null, "Gloves Equipped!");
     }

     private void EquipItem(GameObject current, GameObject next, string message)
     {
          current.SetActive(false);
          if (next != null)
          {
               next.tag = "equip";
          }
          else
          {
               trigger.SetActive(true);
          }
          if (currentTextIndex < textObjects.Length)
          {
               StartCoroutine(DisplayTextSequence(textObjects[currentTextIndex]));
               currentTextIndex++;
          }
     }

     private IEnumerator DisplayTextSequence(GameObject textObject)
     {
          textObject.SetActive(true);
          yield return StartCoroutine(FadeText(textObject, 0f, 1f, fadeDuration));
          yield return new WaitForSeconds(displayDuration);
          yield return StartCoroutine(FadeText(textObject, 1f, 0f, fadeDuration));
          textObject.SetActive(false);
     }

     private IEnumerator FadeText(GameObject textObject, float startAlpha, float endAlpha, float duration)
     {
          float elapsed = 0f;
          TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
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
