using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class Camrotate : MonoBehaviour
{
    float rotateSpeed = 3f;
    float minXRotation = -35f;
    float maxXRotation = 65f;

    float xRotation = 0f;
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse Y");

        xRotation = Mathf.Clamp(xRotation - mouseX * rotateSpeed,
            minXRotation, maxXRotation);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0f); ;
    }
}




