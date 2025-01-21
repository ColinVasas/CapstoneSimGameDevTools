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
     public TextMeshProUGUI popupText;

     private void Start()
     {
          popupText.gameObject.SetActive(false);
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
                         PickupObject(hit.transform.gameObject);
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

          // Only check when the "E" key is pressed
          if (Input.GetKeyDown(KeyCode.E))
          {
               TryEquipHeldObject();
          }
     }

     void PickupObject(GameObject pickObj)
     {
          if (pickObj.GetComponent<Rigidbody>())
          {
               heldObjRB = pickObj.GetComponent<Rigidbody>();
               heldObjRB.useGravity = false;
               heldObjRB.linearDamping = 10;
               heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

               heldObjRB.transform.parent = holdArea;
               heldObj = pickObj;
          }
     }

     void DropObject()
     {
          heldObjRB.useGravity = true;
          heldObjRB.linearDamping = 1;
          heldObjRB.constraints = RigidbodyConstraints.None;

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
               EquipObject();
          }
     }

     void EquipObject()
     {
          Debug.Log("The object has been equipped.");

          popupText.gameObject.SetActive(true);


          equipManager equipManager = Object.FindFirstObjectByType<equipManager>();
          if (equipManager != null)
          {
               equipManager.HandleEquip(heldObj);
          }

          StartCoroutine(HidePopupTextAfterDelay(3f));

          heldObj.SetActive(false);
          heldObj = null;
     }

     IEnumerator HidePopupTextAfterDelay(float delay)
     {
          yield return new WaitForSeconds(delay);
          popupText.gameObject.SetActive(false);
     }


     public bool IsRotatingObject()
     {
          return isRotating;
     }
}