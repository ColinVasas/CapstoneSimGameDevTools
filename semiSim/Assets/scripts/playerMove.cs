using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
     public Camera desktopCamera;
     public Camera vrCamera;
     public pickUpController pickUpController;

     public GameObject desktopPlayer;
     public GameObject vrPlayer;

     public float walkSpeed = 6f;
     public float runSpeed = 12f;
     public float lookSpeed = 2f;
     public float lookXLimit = 60f;

     private Vector3 moveDirection = Vector3.zero;
     private float rotationX = 0;
     private CharacterController characterController;

     private bool isInDesktopMode = true;

     private void Start()
     {
          characterController = GetComponent<CharacterController>();
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;

          if (pickUpController == null)
          {
               Debug.LogError("pickUpController is not assigned in PlayerMove.");
          }

          SetPerspective(isInDesktopMode);
     }

     private void Update()
     {
          if (pickUpController != null && pickUpController.isRotating)
          {
               return;
          }

          if (Input.GetKeyDown(KeyCode.P))
          {
               isInDesktopMode = !isInDesktopMode;
               SetPerspective(isInDesktopMode);
          }

          if (isInDesktopMode)
          {
               Vector3 forward = transform.TransformDirection(Vector3.forward);
               Vector3 right = transform.TransformDirection(Vector3.right);

               bool isRunning = Input.GetKey(KeyCode.LeftShift);
               float curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
               float curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");

               moveDirection = (forward * curSpeedX) + (right * curSpeedY);
               characterController.Move(moveDirection * Time.deltaTime);

               rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
               rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
               desktopCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
               transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
          }
     }

     private void SetPerspective(bool desktopMode)
     {
          desktopPlayer.SetActive(desktopMode);
          vrPlayer.SetActive(!desktopMode);
          desktopCamera.gameObject.SetActive(desktopMode);
          vrCamera.gameObject.SetActive(!desktopMode);

          if (desktopMode)
          {
               Cursor.lockState = CursorLockMode.Locked;
               Cursor.visible = false;
          }
          else
          {
               Cursor.lockState = CursorLockMode.None;
               Cursor.visible = true;
          }
     }
}
