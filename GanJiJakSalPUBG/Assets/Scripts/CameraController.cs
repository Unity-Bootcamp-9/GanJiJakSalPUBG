using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineVirtualCamera thirdPersonCamera;

    public CinemachineVirtualCamera Aimming1Camera;
    public CinemachineVirtualCamera Aimming3Camera;

    private CinemachineVirtualCamera currentCamera;

    private bool isFirstPersonActive;

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
        currentCamera = thirdPersonCamera;
        isFirstPersonActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isFirstPersonActive)
            {
                firstPersonCamera.gameObject.SetActive(false);
                thirdPersonCamera.gameObject.SetActive(true);
                currentCamera = thirdPersonCamera;
                isFirstPersonActive = false;
            }
            else
            {
                thirdPersonCamera.gameObject.SetActive(false);
                firstPersonCamera.gameObject.SetActive(true);
                currentCamera = firstPersonCamera;
                isFirstPersonActive = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentCamera.gameObject.SetActive(false);
            if (currentCamera == firstPersonCamera)
            {
                Aimming1Camera.gameObject.SetActive(true);
            }

            else if (currentCamera == thirdPersonCamera)
            {
                Aimming3Camera.gameObject.SetActive(true);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            currentCamera.gameObject.SetActive(true);

            if (currentCamera == firstPersonCamera)
            {
                Aimming1Camera.gameObject.SetActive(false);
            }

            else if (currentCamera == thirdPersonCamera)
            {
                Aimming3Camera.gameObject.SetActive(false);
            }
        }

    }
}
