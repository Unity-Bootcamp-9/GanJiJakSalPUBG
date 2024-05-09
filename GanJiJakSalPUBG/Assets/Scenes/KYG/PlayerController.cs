using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotateSpeed = 3f;
    public float lookAtSpeed = 3f;
    public Camera mainCamera;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = vertical * cameraForward + horizontal * mainCamera.transform.right;

        // 마우스 위치값 받기
        Vector3 mousePosition = Input.mousePosition;
        // 카메라와 마우스 위치값 사이의 각도 구하기
        float angle = Vector3.SignedAngle(mainCamera.transform.forward, mousePosition - mainCamera.WorldToScreenPoint(transform.position), Vector3.up);

        rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        //float horizontalAngle = Input.GetAxis("Mouse X") * lookAtSpeed;



    }
}
