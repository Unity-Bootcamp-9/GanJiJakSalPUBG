using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public CinemachineVirtualCamera firstCamera;
    public CinemachineVirtualCamera thirdCamera;

    public CinemachineVirtualCamera thirdAimCamera;
    public CinemachineVirtualCamera firstAimCamera;
    public CinemachineFreeLook AltCamera;

    private bool isFirstPersonActive;
    private bool reboundRunning;

    private CinemachineVirtualCamera currentCamera;

    public GameObject player;

    private void Start()
    {
        firstCamera.gameObject.SetActive(false);
        thirdCamera.gameObject.SetActive(true);
        currentCamera = thirdCamera;
        isFirstPersonActive = false;
        reboundRunning = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isFirstPersonActive)
            {
                firstCamera.gameObject.SetActive(false);
                thirdCamera.gameObject.SetActive(true);
                isFirstPersonActive = false;
                currentCamera = thirdCamera;

                player.SetActive(true);
            }
            else
            {
                thirdCamera.gameObject.SetActive(false);
                firstCamera.gameObject.SetActive(true);
                isFirstPersonActive = true;
                currentCamera = firstCamera;
                
                player.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentCamera.gameObject.SetActive(false);
            if (currentCamera == firstCamera)
            {
                firstAimCamera.gameObject.SetActive(true);
                currentCamera = firstAimCamera;
            }
            else if (currentCamera == thirdCamera)
            {
                thirdAimCamera.gameObject.SetActive(true);
                currentCamera = thirdAimCamera;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            currentCamera.gameObject.SetActive(false);
            if (currentCamera == firstAimCamera)
            {
                firstCamera.gameObject.SetActive(true);
                currentCamera = firstCamera;
            }
            else if (currentCamera == thirdAimCamera)
            {
                thirdCamera.gameObject.SetActive(true);
                currentCamera = thirdCamera;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (reboundRunning)
            {
                StopCoroutine(Rebound());
            }
            StartCoroutine(Rebound());
        }

        IEnumerator Rebound()
        {
            reboundRunning = true;

            CinemachineBasicMultiChannelPerlin cbmcp =
                currentCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbmcp.m_AmplitudeGain = 1;

            yield return new WaitForSeconds(0.3f);

            cbmcp.m_AmplitudeGain = 0;
            reboundRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            AltCamera.transform.position = thirdCamera.transform.position;
            AltCamera.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            AltCamera.gameObject.SetActive(false);
        }
    }
}
