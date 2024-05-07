using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera;
    public CinemachineVirtualCamera thirdPersonCamera;

    public Transform[] CameraFollow;
    public Transform[] CameraLookAt;

    CinemachineTransposer FollowOffset;
    CinemachineComposer TrackedObjectOffset;


    private CinemachineVirtualCamera currentCamera;
    private bool isFirstPersonActive;

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
        isFirstPersonActive = false;
        currentCamera = thirdPersonCamera;

        FollowOffset = currentCamera.GetCinemachineComponent<CinemachineTransposer>();
        TrackedObjectOffset = currentCamera.GetCinemachineComponent<CinemachineComposer>();
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

                FollowOffset = currentCamera.GetCinemachineComponent<CinemachineTransposer>();
                TrackedObjectOffset = currentCamera.GetCinemachineComponent<CinemachineComposer>();
            }
            else
            {
                thirdPersonCamera.gameObject.SetActive(false);
                firstPersonCamera.gameObject.SetActive(true);
                currentCamera = firstPersonCamera;
                isFirstPersonActive = true;

                FollowOffset = currentCamera.GetCinemachineComponent<CinemachineTransposer>();
                TrackedObjectOffset = currentCamera.GetCinemachineComponent<CinemachineComposer>();
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            currentCamera.LookAt = CameraLookAt[1];
            currentCamera.Follow = CameraFollow[1];

            FollowOffset.m_FollowOffset = FollowOffset.m_FollowOffset - new Vector3(0, 1f, -1f);
            TrackedObjectOffset.m_TrackedObjectOffset = TrackedObjectOffset.m_TrackedObjectOffset - new Vector3(0, 1.94f, 0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentCamera.LookAt = CameraLookAt[0];
            currentCamera.Follow = CameraFollow[0];
            FollowOffset.m_FollowOffset = FollowOffset.m_FollowOffset + new Vector3(0, 1f, -1f);
            TrackedObjectOffset.m_TrackedObjectOffset = TrackedObjectOffset.m_TrackedObjectOffset + new Vector3(0, 1.94f, 0);
        }

/*        if (!isFirstPersonActive && Input.GetKeyDown(KeyCode.LeftShift))
        {

        }
        if (!isFirstPersonActive && Input.GetKeyUp(KeyCode.LeftShift))
        {

        }
*/    }
}
