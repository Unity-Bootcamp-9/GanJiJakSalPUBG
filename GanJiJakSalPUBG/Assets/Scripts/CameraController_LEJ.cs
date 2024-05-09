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

    public GameObject playerUpperBody; // �÷��̾� ��ü�� ��Ÿ���� GameObject

    public CinemachineFreeLook AltCamera;

    private GameObject currentCamera;

    private bool isFirstPersonActive;
    private bool reboundRunning;
    private bool isRightClicking; // ������ ���콺 Ŭ�� ���¸� �����ϴ� ����
    private bool isAltPressed; // ���� Alt Ű ���¸� �����ϴ� ����

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
        currentCamera = thirdPersonCamera.gameObject;
        isFirstPersonActive = false;
        reboundRunning = false;
        isRightClicking = false; // �ʱⰪ���� ������ Ŭ�� ���´� false�� ����
        isAltPressed = false; // �ʱⰪ���� ���� Alt Ű ���´� false�� ����
    }

    void Update()
    {
        // ���� Alt Ű ����
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isAltPressed = true;
            thirdFreeCamera.gameObject.SetActive(true);
            // FreeLook ī�޶� Ȱ��ȭ
            //AltCamera.gameObject.SetActive(true);
        }

        // ���� Alt Ű ��
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            isAltPressed = false;
            // FreeLook ī�޶� ��Ȱ��ȭ
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
            isRightClicking = true; // ������ ���콺 Ŭ���� ���۵�
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

            // �÷��̾� ��ü�� z������ 45�� ȸ��
            if (playerUpperBody != null)
            {
                playerUpperBody.transform.Rotate(Vector3.back, 20f);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightClicking = false; // ������ ���콺 Ŭ���� �����
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

            // �÷��̾� ��ü ������� �ǵ���
            if (playerUpperBody != null)
            {
                playerUpperBody.transform.rotation = Quaternion.identity;
            }
        }

        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // ī�޶� FreeLook�� �ƴ� ��쿡�� ���콺 �Է¿� ���� ȸ���ϵ��� ��
        if (!(currentCamera is CinemachineFreeLook))
        {
            // ���� ȸ���� ĳ������ �¿� ȸ���� ������ �ֵ��� ��
            transform.Rotate(Vector3.up, mouseX);

            // ���� ȸ���� ���콺�� y�� �̵��� ���� ���� ȸ���ϵ��� ��
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

        // AmplitudeGain�� 1�� ����
        SetAmplitudeGain(1f);

        // 0.3�� ���
        yield return new WaitForSeconds(0.3f);

        // AmplitudeGain�� 0���� ����
        SetAmplitudeGain(0f);

        reboundRunning = false;

    }

    void SetAmplitudeGain(float amplitudeGain)
    {
        for (int i = 0; i < 3; i++) // TopRig (0), MiddleRig (1), BottomRig (2)�� ���� ����
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
