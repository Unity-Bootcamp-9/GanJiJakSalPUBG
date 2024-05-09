using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraController_LEJ : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineVirtualCamera thirdPersonCamera;

    public CinemachineFreeLook thirdFreeCamera;
    //public CinemachineFreeLook Aimming3FreeCamera;

    public CinemachineVirtualCamera Aimming1Camera;
    public CinemachineVirtualCamera Aimming3Camera;

    public GameObject playerUpperBody; // 플레이어 상체를 나타내는 GameObject

    public CinemachineFreeLook AltCamera;

    private GameObject currentCamera;

    private bool isFirstPersonActive;
    private bool reboundRunning;
    private bool isRightClicking; // 오른쪽 마우스 클릭 상태를 저장하는 변수
    private bool isAltPressed; // 왼쪽 Alt 키 상태를 저장하는 변수

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
        currentCamera = thirdPersonCamera.gameObject;
        isFirstPersonActive = false;
        reboundRunning = false;
        isRightClicking = false; // 초기값으로 오른쪽 클릭 상태는 false로 설정
        isAltPressed = false; // 초기값으로 왼쪽 Alt 키 상태는 false로 설정
    }

    void Update()
    {
        // 왼쪽 Alt 키 누름
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isAltPressed = true;
            thirdFreeCamera.gameObject.SetActive(true);
            // FreeLook 카메라 활성화
            //AltCamera.gameObject.SetActive(true);
        }

        // 왼쪽 Alt 키 뗌
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            isAltPressed = false;
            // FreeLook 카메라 비활성화
            if (AltCamera != null)
            {
                thirdFreeCamera.gameObject.SetActive(false);
                //AltCamera.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isFirstPersonActive)
            {
                firstPersonCamera.gameObject.SetActive(false);
                thirdFreeCamera.gameObject.SetActive(true);
                currentCamera = thirdPersonCamera.gameObject;
                isFirstPersonActive = false;
            }
            else
            {
                thirdFreeCamera.gameObject.SetActive(false);
                firstPersonCamera.gameObject.SetActive(true);
                currentCamera = firstPersonCamera.gameObject;
                isFirstPersonActive = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            isRightClicking = true; // 오른쪽 마우스 클릭이 시작됨
            currentCamera.SetActive(false);
            if (currentCamera == firstPersonCamera.gameObject)
            {
                Aimming1Camera.gameObject.SetActive(true);
                currentCamera = Aimming1Camera.gameObject;
            }
            else if (currentCamera == thirdPersonCamera.gameObject)
            {
                Aimming3Camera.transform.position = thirdFreeCamera.transform.position;
                Aimming3Camera.gameObject.SetActive(true);
                currentCamera = Aimming3Camera.gameObject;
            }

            // 플레이어 상체를 z축으로 45도 회전
            if (playerUpperBody != null)
            {
                playerUpperBody.transform.Rotate(Vector3.back, 20f);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightClicking = false; // 오른쪽 마우스 클릭이 종료됨
            currentCamera.SetActive(false);

            if (currentCamera == Aimming1Camera.gameObject)
            {
                firstPersonCamera.gameObject.SetActive(true);
                currentCamera = firstPersonCamera.gameObject;
            }
            else if (currentCamera == Aimming3Camera.gameObject)
            {
                thirdPersonCamera.gameObject.SetActive(true);
                currentCamera = thirdPersonCamera.gameObject;
            }

            // 플레이어 상체 원래대로 되돌림
            if (playerUpperBody != null)
            {
                playerUpperBody.transform.rotation = Quaternion.identity;
            }
        }

        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 카메라가 FreeLook가 아닌 경우에만 마우스 입력에 따라 회전하도록 함
        if (!(currentCamera is CinemachineFreeLook))
        {
            // 수평 회전은 캐릭터의 좌우 회전에 영향을 주도록 함
            transform.Rotate(Vector3.up, mouseX);

            // 수직 회전은 마우스의 y축 이동에 따라 직접 회전하도록 함
            float newRotationX = transform.eulerAngles.x - mouseY;
            transform.rotation = Quaternion.Euler(newRotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (reboundRunning)
            {
                StopCoroutine(Rebound());
            }

            if (currentCamera == thirdFreeCamera.gameObject || currentCamera == Aimming3Camera.gameObject)
            {
                StartCoroutine(FreeLookRebound());
            }
            else
            {
                StartCoroutine(Rebound());
            }
        }
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

    IEnumerator FreeLookRebound()
    {
        reboundRunning = true;

        // AmplitudeGain을 1로 설정
        SetAmplitudeGain(1f);

        // 0.3초 대기
        yield return new WaitForSeconds(0.3f);

        // AmplitudeGain을 0으로 설정
        SetAmplitudeGain(0f);

        reboundRunning = false;

    }

    void SetAmplitudeGain(float amplitudeGain)
    {
        for (int i = 0; i < 3; i++) // TopRig (0), MiddleRig (1), BottomRig (2)에 대해 실행
        {
            var virtualCamera = currentCamera.GetComponent<CinemachineFreeLook>().GetRig(i);
            var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                noise.m_AmplitudeGain = amplitudeGain;
            }
        }
    }
}
