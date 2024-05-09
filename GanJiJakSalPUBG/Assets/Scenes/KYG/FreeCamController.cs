using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamController : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    
    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.X))
            cam.Priority = 11;
        else
            cam.Priority = 9;
    }
}
