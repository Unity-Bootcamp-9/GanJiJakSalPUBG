using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using System.Collections;

public class CameraController_KDY : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera;
    private CinemachineVirtualCamera thirdPersonCamera;

    public CinemachineFreeLook thirdFreeCamera;
    public CinemachineFreeLook Aimming3FreeCamera;

    public CinemachineVirtualCamera Aimming1Camera;
    private CinemachineVirtualCamera Aimming3Camera;

    public CinemachineFreeLook AltCamera;

    public GameObject playerUpperBody; // �÷��̾� ��ü�� ��Ÿ���� GameObject

    // FreeLook ī�޶�� ���� ����ؾ��ؼ� �׳� GameObject�� ��ġ�� �ٶ��� �ڵ尡 ������
    private GameObject currentCamera;

    private bool isFirstPersonActive;
    private bool reboundRunning;

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdFreeCamera.gameObject.SetActive(true);
        currentCamera = thirdFreeCamera.gameObject;
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
                thirdFreeCamera.gameObject.SetActive(true);
                currentCamera = thirdFreeCamera.gameObject;
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
            currentCamera.SetActive(false);
            if (currentCamera == firstPersonCamera.gameObject)
            {
                Aimming1Camera.gameObject.SetActive(true);
                currentCamera = Aimming1Camera.gameObject;
            }
            else if (currentCamera == thirdFreeCamera.gameObject)
            {
                Aimming3FreeCamera.transform.position = thirdFreeCamera.transform.position;
                Aimming3FreeCamera.gameObject.SetActive(true);
                currentCamera = Aimming3FreeCamera.gameObject;
            }

/*            // �÷��̾� ��ü�� z������ 45�� ȸ��
            if (playerUpperBody != null)
            {
                playerUpperBody.transform.Rotate(Vector3.back, 20f);
            }
*/
        }

        if (Input.GetMouseButtonUp(1))
        {
            currentCamera.SetActive(false);

            if (currentCamera == Aimming1Camera.gameObject)
            {
                firstPersonCamera.gameObject.SetActive(true);
                currentCamera = firstPersonCamera.gameObject;
            }
            else if (currentCamera == Aimming3FreeCamera.gameObject)
            {
                thirdFreeCamera.gameObject.SetActive(true);
                currentCamera = thirdFreeCamera.gameObject;
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

            if(currentCamera == thirdFreeCamera.gameObject || currentCamera == Aimming3FreeCamera.gameObject)
            {
                StartCoroutine(FreeLookRebound());
            }

            else
            {
                StartCoroutine(Rebound());
            }
        }

/*        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            AltCamera.transform.position = thirdFreeCamera.transform.position;
            AltCamera.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            AltCamera.gameObject.SetActive(false);
        }*/

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
