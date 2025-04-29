using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpController : MonoBehaviour
{
     [Header("Pickup Settings")]
     [SerializeField] Transform holdArea;
     private GameObject heldObj;
     private Rigidbody heldObjRB;
    private GameObject hoveredObj;

     [Header("Physics Parameters")]
     [SerializeField] private float pickupRange = 5.0f;
     [SerializeField] private float pickupForce = 150.0f;
     [SerializeField] private float rotationSpeed = 100.0f;

    private KeyboardInteractable keyboardInteractable;

     public bool isRotating { get; private set; } = false;


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
        else
        {
            HoverManager();
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

    private void HoverManager()
    {
        if (heldObj == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
            {
                keyboardInteractable = hit.transform.GetComponent<KeyboardInteractable>();
                if (keyboardInteractable != null)
                {
                    if (hoveredObj == null)
                    {
                        keyboardInteractable.OnHoverEnter();
                        hoveredObj = hit.transform.gameObject;
                    }
                    else if (hoveredObj != hit.transform.gameObject)
                    {
                        // Exit old hover
                        hoveredObj.GetComponent<KeyboardInteractable>().OnHoverExit();

                        // Enter new hover
                        keyboardInteractable.OnHoverEnter();
                        hoveredObj = hit.transform.gameObject;
                    }
                }
                else if (hoveredObj != null)
                {
                    // No interactable hit, but was hovering something
                    hoveredObj.GetComponent<KeyboardInteractable>().OnHoverExit();
                    hoveredObj = null;
                }
            }
            else if (hoveredObj != null)
            {
                // No hit at all
                hoveredObj.GetComponent<KeyboardInteractable>().OnHoverExit();
                hoveredObj = null;
            }
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
               pickObj.layer = LayerMask.NameToLayer("TransparentFX");
               heldObjRB.transform.parent = holdArea;
               heldObj = pickObj;


            if (keyboardInteractable != null)
            {
                keyboardInteractable.OnPickUpEnter();                
            }
          }
     }

     void DropObject()
     {
          heldObjRB.useGravity = true;
          heldObjRB.linearDamping = 1;
          heldObjRB.constraints = RigidbodyConstraints.None;
          heldObj.layer = LayerMask.NameToLayer("Default");
          heldObj.transform.parent = null;
          if (keyboardInteractable != null)
            {
                keyboardInteractable.OnPickUpExit();
            }
        heldObj = null;
        keyboardInteractable = null;
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


          equipManager equipManager = Object.FindFirstObjectByType<equipManager>();
          if (equipManager != null)
          {
               equipManager.HandleEquip(heldObj);
          }

          heldObj.SetActive(false);
          heldObj = null;
     }



     public bool IsRotatingObject()
     {
          return isRotating;
     }
}