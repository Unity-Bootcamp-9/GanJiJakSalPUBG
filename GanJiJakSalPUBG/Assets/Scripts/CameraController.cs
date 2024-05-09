using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineVirtualCamera thirdPersonCamera;

    public CinemachineVirtualCamera Aimming1Camera;
    public CinemachineVirtualCamera Aimming3Camera;

    private CinemachineVirtualCamera currentCamera;

    private bool isFirstPersonActive;
    private bool reboundRunning;

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
        currentCamera = thirdPersonCamera;
        isFirstPersonActive = false;
        reboundRunning = false;
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
                currentCamera = Aimming1Camera;
            }

            else if (currentCamera == thirdPersonCamera)
            {
                Aimming3Camera.gameObject.SetActive(true);
                currentCamera = Aimming3Camera;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            currentCamera.gameObject.SetActive(false);

            if (currentCamera == Aimming1Camera)
            {
                firstPersonCamera.gameObject.SetActive(true);
                currentCamera = firstPersonCamera;
            }

            else if (currentCamera == Aimming3Camera)
            {
                thirdPersonCamera.gameObject.SetActive(true);
                currentCamera = thirdPersonCamera;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(reboundRunning)
            {
                StopCoroutine(Rebound());
            }
            StartCoroutine(Rebound());
        }


    }

    IEnumerator Rebound()
    {
        reboundRunning = true;

        CinemachineBasicMultiChannelPerlin cbmcp =
            currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 1;

        yield return new WaitForSeconds(0.3f);

        cbmcp.m_AmplitudeGain = 0;
        reboundRunning = false;
    }

}
