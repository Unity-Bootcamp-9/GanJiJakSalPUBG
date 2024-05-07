using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineVirtualCamera thirdPersonCamera;

    public Transform[] CameraFollow;
    public Transform[] CameraLookAt;

    private bool isFirstPersonActive;

    private void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
        isFirstPersonActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleCamera();
        }
    }

    private void ToggleCamera()
    {
        thirdPersonCamera.gameObject.SetActive(!thirdPersonCamera.gameObject.activeSelf);
        firstPersonCamera.gameObject.SetActive(!firstPersonCamera.gameObject.activeSelf);
        isFirstPersonActive = !isFirstPersonActive;
    }
}
