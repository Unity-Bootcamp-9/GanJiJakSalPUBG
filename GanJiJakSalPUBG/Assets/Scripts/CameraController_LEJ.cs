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

    public GameObject playerUpperBody; // �÷��̾� ��ü�� ��Ÿ���� GameObject

    public GameObject imageToShow; // ȭ�鿡 ������ �̹��� GameObject
    public float zoomAmount = 2f; // ȭ�� Ȯ�뷮

    private CinemachineVirtualCamera currentCamera;

    private bool isFirstPersonActive;
    private bool reboundRunning;
    private bool isRightClicking; // ������ ���콺 Ŭ�� ���¸� �����ϴ� ����

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
        currentCamera = thirdPersonCamera;
        isFirstPersonActive = false;
        reboundRunning = false;
        isRightClicking = false; // �ʱⰪ���� ������ Ŭ�� ���´� false�� ����
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
            isRightClicking = true; // ������ ���콺 Ŭ���� ���۵�
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

            // �÷��̾� ��ü�� z������ 45�� ȸ��
            if (playerUpperBody != null)
            {
                playerUpperBody.transform.Rotate(Vector3.back, 20f);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightClicking = false; // ������ ���콺 Ŭ���� �����
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

            // �÷��̾� ��ü�� ȸ���� ������� �ǵ���
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

        // ������ ���콺 Ŭ���� ª�� ������ �� �̹��� ���̱� �� ȭ�� Ȯ��
        if (Input.GetMouseButtonDown(1) && !isRightClicking)
        {
            if (imageToShow != null)
            {
                imageToShow.SetActive(true); // �̹��� ���̱�
            }
            // ���� ī�޶��� ȭ�� Ȯ��
            currentCamera.m_Lens.FieldOfView *= zoomAmount;
        }
        // ������ ���콺 Ŭ���� �������� �� �̹��� ����� �� ȭ�� ���
        else if (Input.GetMouseButtonUp(1) && !isRightClicking)
        {
            if (imageToShow != null)
            {
                imageToShow.SetActive(false); // �̹��� �����
            }
            // ���� ī�޶��� ȭ�� ���
            currentCamera.m_Lens.FieldOfView /= zoomAmount;
        }
        // ������ ���콺 Ŭ���� ���� ���¿��� �ٽ� ���� ���
        else if (Input.GetMouseButtonDown(1) && isRightClicking)
        {
            if (imageToShow != null)
            {
                imageToShow.SetActive(false); // �̹��� �����
            }
            // ���� ī�޶��� ȭ�� ���
            currentCamera.m_Lens.FieldOfView /= zoomAmount;
            isRightClicking = false; // ������ ���콺 Ŭ�� ���� �ʱ�ȭ
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
