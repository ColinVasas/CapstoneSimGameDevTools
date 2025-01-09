using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
     public Camera playerCamera;
     public float walkSpeed = 6f;
     public float runSpeed = 12f;
     public float lookSpeed = 2f;
     public float lookXLimit = 60f;

     private Vector3 moveDirection = Vector3.zero;
     private float rotationX = 0;
     private CharacterController characterController;

     private bool canMove = true;

     void Start()
     {
          characterController = GetComponent<CharacterController>();
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;
     }

     void Update()
     {
          Vector3 forward = transform.TransformDirection(Vector3.forward);
          Vector3 right = transform.TransformDirection(Vector3.right);

          bool isRunning = Input.GetKey(KeyCode.LeftShift);
          float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
          float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

          moveDirection = (forward * curSpeedX) + (right * curSpeedY);


          characterController.Move(moveDirection * Time.deltaTime);

          if (canMove)
          {
               rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
               rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
               playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
               transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
          }
     }
}
