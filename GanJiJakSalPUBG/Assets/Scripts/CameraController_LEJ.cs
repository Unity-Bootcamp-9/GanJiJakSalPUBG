using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using System.Collections;

public class CameraController_LEJ : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineVirtualCamera thirdPersonCamera;

    public CinemachineVirtualCamera Aimming1Camera;
    public CinemachineVirtualCamera Aimming3Camera;

    public GameObject playerUpperBody; // 플레이어 상체를 나타내는 GameObject

    public GameObject imageToShow; // 화면에 보여줄 이미지 GameObject
    public float zoomAmount = 2f; // 화면 확대량

    private CinemachineVirtualCamera currentCamera;

    private bool isFirstPersonActive;
    private bool reboundRunning;
    private bool isRightClicking; // 오른쪽 마우스 클릭 상태를 저장하는 변수

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
        currentCamera = thirdPersonCamera;
        isFirstPersonActive = false;
        reboundRunning = false;
        isRightClicking = false; // 초기값으로 오른쪽 클릭 상태는 false로 설정
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
            isRightClicking = true; // 오른쪽 마우스 클릭이 시작됨
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

            // 플레이어 상체를 z축으로 45도 회전
            if (playerUpperBody != null)
            {
                playerUpperBody.transform.Rotate(Vector3.back, 20f);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightClicking = false; // 오른쪽 마우스 클릭이 종료됨
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

            // 플레이어 상체의 회전을 원래대로 되돌림
            if (playerUpperBody != null)
            {
                playerUpperBody.transform.rotation = Quaternion.identity;
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

        // 오른쪽 마우스 클릭이 짧게 눌렸을 때 이미지 보이기 및 화면 확대
        if (Input.GetMouseButtonDown(1) && !isRightClicking)
        {
            if (imageToShow != null)
            {
                imageToShow.SetActive(true); // 이미지 보이기
            }
            // 현재 카메라의 화면 확대
            currentCamera.m_Lens.FieldOfView *= zoomAmount;
        }
        // 오른쪽 마우스 클릭이 떼어졌을 때 이미지 숨기기 및 화면 축소
        else if (Input.GetMouseButtonUp(1) && !isRightClicking)
        {
            if (imageToShow != null)
            {
                imageToShow.SetActive(false); // 이미지 숨기기
            }
            // 현재 카메라의 화면 축소
            currentCamera.m_Lens.FieldOfView /= zoomAmount;
        }
        // 오른쪽 마우스 클릭이 눌린 상태에서 다시 눌린 경우
        else if (Input.GetMouseButtonDown(1) && isRightClicking)
        {
            if (imageToShow != null)
            {
                imageToShow.SetActive(false); // 이미지 숨기기
            }
            // 현재 카메라의 화면 축소
            currentCamera.m_Lens.FieldOfView /= zoomAmount;
            isRightClicking = false; // 오른쪽 마우스 클릭 상태 초기화
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
