using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pickUpController : MonoBehaviour
{
     [Header("Pickup Settings")]
     [SerializeField] Transform holdArea;
     private GameObject heldObj;
     private Rigidbody heldObjRB;

     [Header("Physics Parameters")]
     [SerializeField] private float pickupRange = 5.0f;
     [SerializeField] private float pickupForce = 150.0f;
     [SerializeField] private float rotationSpeed = 100.0f;

     public bool isRotating { get; private set; } = false;

    // variables for "Press E to equip" and stopping intro text
     [Header("UI")]
     // public GameObject trigger;
     public TextMeshProUGUI equipText; // Single TMP UI element for all messages
     public float fadeDuration = 0.5f;
     public float displayDuration = 1.5f;
     private Coroutine messageCoroutine;
    private deskCanvasText introText;
    private Coroutine fadeCoroutine;
    private Coroutine persistentCoroutine;

    private void Start()
    {
        introText = FindFirstObjectByType<deskCanvasText>();


        equipText.gameObject.SetActive(false);
        var c = equipText.color;
        c.a = 0f;
        equipText.color = c;
        // trigger.SetActive(false);
    }
    private void Update()
     {
          if (Input.GetMouseButtonDown(0))
          {
               if (heldObj == null)
               {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                    {
                         if (PickupObject(hit.transform.gameObject) && hit.transform.CompareTag("equip"))
                        {
                        StopMessage();
                        messageCoroutine = StartCoroutine(ShowPersistentMessage("Press 'E' to equip"));
                    }
                    
                    }
                    // DisplayTextMessage("Press 'E' to equip");
            }
               else
               {
                    DropObject();
               }
          }

          if (Input.GetKey(KeyCode.R) && heldObj != null)
          {
               isRotating = true;
               RotateObject();
          }
          else
          {
               isRotating = false;
               if (heldObj != null) MoveObject();
          }
          if (Input.GetKeyDown(KeyCode.E))
          {
            
                TryEquipHeldObject();
          }
     }

     bool PickupObject(GameObject pickObj)
     {
        var rb = pickObj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return false;
        }

        introText?.StopIntro();  
        
        if (pickObj.GetComponent<Rigidbody>())
          {
               heldObjRB = pickObj.GetComponent<Rigidbody>();
               heldObjRB.useGravity = false;
               heldObjRB.linearDamping = 10;
               heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
               pickObj.layer = LayerMask.NameToLayer("TransparentFX");
               heldObjRB.transform.parent = holdArea;
               heldObj = pickObj;
          }

        return true;
     }

     void DropObject()
     {
        // get rid of "Press E to equip" if object dropped
        StopMessage();
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            HideMessageImmediate();
            messageCoroutine = null;
        }
          heldObjRB.useGravity = true;
          heldObjRB.linearDamping = 1;
          heldObjRB.constraints = RigidbodyConstraints.None;
          heldObj.layer = LayerMask.NameToLayer("Default");
          heldObj.transform.parent = null;
          heldObj = null;
     }

     void MoveObject()
     {
          if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
          {
               Vector3 moveDirection = holdArea.position - heldObj.transform.position;
               heldObjRB.AddForce(moveDirection * pickupForce);
          }
     }

     void RotateObject()
     {
          float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
          float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

          heldObjRB.angularVelocity = Vector3.zero;
          heldObj.transform.Rotate(Vector3.up, -mouseX, Space.World);
          heldObj.transform.Rotate(Vector3.right, mouseY, Space.World);
     }

     void TryEquipHeldObject()
     {
          if (heldObj != null && heldObj.CompareTag("equip"))
          {
                StopMessage();
                EquipObject();
          }
     }

    private void StopMessage()
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }
        if (fadeCoroutine != null)
        {
            StopFadeCoroutine();
            fadeCoroutine = null;
        }
        if (persistentCoroutine != null)
        {
            StopPersistentCoroutine();
            persistentCoroutine = null;
        }
        
        // hide and reset fade
        equipText.gameObject.SetActive(false);
        var c = equipText.color;
        c.a = 0;
        equipText.color = c;
    }

     void EquipObject()
     {
          Debug.Log("The object has been equipped.");


          equipManager equipManager = Object.FindFirstObjectByType<equipManager>();
          if (equipManager != null)
          {
               equipManager.HandleEquip(heldObj);
          }

          heldObj.SetActive(false);
          heldObj = null;
     }

     public IEnumerator DisplayTextMessage(string message)
     {
        // this is helpful for when we call "PRESS E" text shenanigans
        StopFadeCoroutine();

        // trigger.SetActive(true);
          equipText.text = message;
          equipText.gameObject.SetActive(true);

          yield return FadeText(0f, 1f, fadeDuration); // Fade in
          yield return new WaitForSeconds(displayDuration);
          yield return FadeText(1f, 0f, fadeDuration); // Fade out

          equipText.gameObject.SetActive(false);
          // trigger.SetActive(false);
     }

    // for when we DON'T want text fade
    private IEnumerator ShowPersistentMessage(string msg)
    {
        StopFadeCoroutine();

        equipText.text = msg;
        equipText.gameObject.SetActive(true);

        yield return FadeText(0,1,fadeDuration);

        // don't want text to fade
        while (true) yield return null;
    }

    private void StopFadeCoroutine()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }

    private void StopPersistentCoroutine()
    {
        if (persistentCoroutine != null)
        {
            StopCoroutine(persistentCoroutine);
            persistentCoroutine = null;
        }
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

    private void HideMessageImmediate()
    {
        // trigger.SetActive(false);
        equipText.gameObject.SetActive(false);
        var c = equipText.color;
        c.a = 0f;
        equipText.color = c;
    }

     public bool IsRotatingObject()
     {
          return isRotating;
     }



}