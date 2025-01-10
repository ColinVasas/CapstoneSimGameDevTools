using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
     public Camera playerCamera; // Already assigned.
     public pickUpController pickUpController; // Add this.

     public float walkSpeed = 6f;
     public float runSpeed = 12f;
     public float lookSpeed = 2f;
     public float lookXLimit = 60f;

     private Vector3 moveDirection = Vector3.zero;
     private float rotationX = 0;
     private CharacterController characterController;

     private void Start()
     {
          characterController = GetComponent<CharacterController>();
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;

          if (pickUpController == null)
          {
               Debug.LogError("pickUpController is not assigned in PlayerMove.");
          }
     }

     private void Update()
     {
          if (pickUpController != null && pickUpController.isRotating)
          {
               return; // Skip camera movement while rotating an object.
          }

          Vector3 forward = transform.TransformDirection(Vector3.forward);
          Vector3 right = transform.TransformDirection(Vector3.right);

          bool isRunning = Input.GetKey(KeyCode.LeftShift);
          float curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
          float curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");

          moveDirection = (forward * curSpeedX) + (right * curSpeedY);
          characterController.Move(moveDirection * Time.deltaTime);

          rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
          rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
          playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
          transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
     }
}

