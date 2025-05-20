using System.Collections;
using TMPro;
using UnityEngine;

public class pickUpController : MonoBehaviour
{
     [Header("Pickup Settings")]
     [SerializeField] private Transform holdArea;
     private GameObject heldObj;
     private Rigidbody heldObjRB;
     private GameObject hoveredObj;

     [Header("Physics Parameters")]
     [SerializeField] private float pickupRange = 5.0f;
     [SerializeField] private float pickupForce = 150.0f;
     [SerializeField] private float rotationSpeed = 100.0f;

     public bool isRotating { get; private set; } = false;

     [Header("UI")]
     public TextMeshProUGUI equipText;
     public float fadeDuration = 0.5f;
     public float displayDuration = 1.5f;
     private Coroutine messageCoroutine;
     private Coroutine fadeCoroutine;
     private Coroutine persistentCoroutine;
     private deskCanvasText introText;
     private KeyboardInteractable keyboardInteractable;

     private void Start()
     {
          introText = FindFirstObjectByType<deskCanvasText>();

          equipText.gameObject.SetActive(false);
          var c = equipText.color;
          c.a = 0f;
          equipText.color = c;
     }

     private void Update()
     {
          if (Input.GetMouseButtonDown(0))
          {
               if (heldObj == null)
               {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
                    {
                         if (PickupObject(hit.transform.gameObject) && hit.transform.CompareTag("equip"))
                         {
                              StopMessage();
                              messageCoroutine = StartCoroutine(ShowPersistentMessage("Press 'E' to equip"));
                         }
                         // else if heldObj is the chuck
                         else if (PickupObject(hit.transform.gameObject) && heldObj.name == "chuck")
                         {
                              StopMessage();
                              messageCoroutine = StartCoroutine(DisplayTextMessage("This is a chuck"));
                         }
                         // else if heldObj is the wafer
                         else if (PickupObject(hit.transform.gameObject) && (heldObj.name == "wafer" || hit.transform.CompareTag("wafer") || hit.transform.CompareTag("coldWafer")))
                         {
                              StopMessage();
                              messageCoroutine = StartCoroutine(DisplayTextMessage("This is a wafer"));
                         }
                         // else if heldObj is a pipet
                         else if (PickupObject(hit.transform.gameObject) && (heldObj.name == "pipette_cap" || heldObj.name == "pipette_cap (1)"))
                         {
                              StopMessage();
                              messageCoroutine = StartCoroutine(DisplayTextMessage("This is a pipet"));
                         }
                         // else if heldObj is a spay gun
                         else if (PickupObject(hit.transform.gameObject) && heldObj.name == "spray gun")
                         {
                              StopMessage();
                              messageCoroutine = StartCoroutine(DisplayTextMessage("This is a spray gun"));
                         }
                         else if (heldObj != null)
                         {
                              StopMessage();
                              Debug.Log(heldObj.name);
                              messageCoroutine = StartCoroutine(ShowPersistentMessage("Whoops! Wrong object!\nLeft click to put down"));
                         }
                    }
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

          HoverManager();
     }

     private bool PickupObject(GameObject pickObj)
     {
          Rigidbody rb = pickObj.GetComponent<Rigidbody>();
          if (rb == null) return false;

          introText?.StopIntro();

          heldObjRB = rb;
          heldObjRB.useGravity = false;
          heldObjRB.linearDamping = 10;
          heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
          pickObj.layer = LayerMask.NameToLayer("TransparentFX");
          heldObjRB.transform.parent = holdArea;
          heldObj = pickObj;

          keyboardInteractable = pickObj.GetComponent<KeyboardInteractable>();
          keyboardInteractable?.OnPickUpEnter();

          return true;
     }

     private void DropObject()
     {
          StopMessage();

          if (heldObjRB != null)
          {
               heldObjRB.useGravity = true;
               heldObjRB.linearDamping = 1;
               heldObjRB.constraints = RigidbodyConstraints.None;
               heldObjRB.transform.parent = null;
               heldObj.layer = LayerMask.NameToLayer("Default");
          }

          keyboardInteractable?.OnPickUpExit();

          heldObj = null;
          heldObjRB = null;
          keyboardInteractable = null;
     }

     private void MoveObject()
     {
          if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
          {
               Vector3 moveDirection = holdArea.position - heldObj.transform.position;
               heldObjRB.AddForce(moveDirection * pickupForce);
          }
     }

     private void RotateObject()
     {
          float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
          float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

          heldObjRB.angularVelocity = Vector3.zero;
          heldObj.transform.Rotate(Vector3.up, -mouseX, Space.World);
          heldObj.transform.Rotate(Vector3.right, mouseY, Space.World);
     }

     private void TryEquipHeldObject()
     {
          if (heldObj != null && heldObj.CompareTag("equip"))
          {
               StopMessage();
               EquipObject();
          }
     }

     private void EquipObject()
     {
          Debug.Log("The object has been equipped.");

          equipManager manager = FindFirstObjectByType<equipManager>();
          manager?.HandleEquip(heldObj);

          heldObj.SetActive(false);
          heldObj = null;
     }

     private void HoverManager()
     {
          if (heldObj != null) return;

          RaycastHit hit;
          if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
          {
               var newInteractable = hit.transform.GetComponent<KeyboardInteractable>();
               if (newInteractable != null)
               {
                    if (hoveredObj == null || hoveredObj != hit.transform.gameObject)
                    {
                         hoveredObj?.GetComponent<KeyboardInteractable>()?.OnHoverExit();
                         newInteractable.OnHoverEnter();
                         hoveredObj = hit.transform.gameObject;
                    }
               }
               else if (hoveredObj != null)
               {
                    hoveredObj.GetComponent<KeyboardInteractable>()?.OnHoverExit();
                    hoveredObj = null;
               }
          }
          else if (hoveredObj != null)
          {
               hoveredObj.GetComponent<KeyboardInteractable>()?.OnHoverExit();
               hoveredObj = null;
          }
     }

     private void StopMessage()
     {
          if (messageCoroutine != null) StopCoroutine(messageCoroutine);
          if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
          if (persistentCoroutine != null) StopCoroutine(persistentCoroutine);

          messageCoroutine = null;
          fadeCoroutine = null;
          persistentCoroutine = null;

          equipText.gameObject.SetActive(false);
          var c = equipText.color;
          c.a = 0f;
          equipText.color = c;
     }

     private IEnumerator ShowPersistentMessage(string msg)
     {
          StopFadeCoroutine();

          equipText.text = msg;
          equipText.gameObject.SetActive(true);
          yield return FadeText(0, 1, fadeDuration);

          while (true) yield return null;
     }

     public IEnumerator DisplayTextMessage(string message)
     {
          StopFadeCoroutine();

          equipText.text = message;
          equipText.gameObject.SetActive(true);
          yield return FadeText(0f, 1f, fadeDuration);
          yield return new WaitForSeconds(displayDuration);
          yield return FadeText(1f, 0f, fadeDuration);
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

     private void StopFadeCoroutine()
     {
          if (fadeCoroutine != null)
          {
               StopCoroutine(fadeCoroutine);
               fadeCoroutine = null;
          }
     }

     public bool IsRotatingObject()
     {
          return isRotating;
     }
}
