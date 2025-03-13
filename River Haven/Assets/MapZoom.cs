using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MapZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomDuration = 4f;
    public float targetYPosition = 60f;
    public float targetXRotation = 80f;
    public float zoomSpeedFactor = 40f;
    private CinemachineTransposer transposer;
    private float initialYPosition;
    private float initialXRotation;
    private bool isZoomedOut = false;
    private float elapsedTime = 0f;
    [SerializeField] private Camera secondCamera;

    void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        initialYPosition = transposer.m_FollowOffset.y;
        initialXRotation = virtualCamera.transform.eulerAngles.x;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isZoomedOut)
            {
                StartCoroutine(ZoomIn());
                secondCamera.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(ZoomOut());
                secondCamera.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator ZoomOut()
    {
        elapsedTime = 0f;
        float initialTime = Time.time;
        float initialZoomY = transposer.m_FollowOffset.y;
        float initialRotationX = virtualCamera.transform.eulerAngles.x;

        while (transposer.m_FollowOffset.y < targetYPosition)
        {
            elapsedTime = Time.time - initialTime;
            float speedFactor = Mathf.Pow(zoomSpeedFactor, elapsedTime);
            transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, Mathf.Lerp(initialZoomY, targetYPosition, speedFactor / zoomDuration), transposer.m_FollowOffset.z);
            float currentRotationX = Mathf.Lerp(initialRotationX, targetXRotation, speedFactor / zoomDuration);
            virtualCamera.transform.eulerAngles = new Vector3(currentRotationX, virtualCamera.transform.eulerAngles.y, virtualCamera.transform.eulerAngles.z);

            yield return null;
        }

        transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, targetYPosition, transposer.m_FollowOffset.z);
        virtualCamera.transform.eulerAngles = new Vector3(targetXRotation, virtualCamera.transform.eulerAngles.y, virtualCamera.transform.eulerAngles.z);

        isZoomedOut = true;
    }

    private IEnumerator ZoomIn()
    {
        elapsedTime = 0f;
        float initialTime = Time.time;
        float targetZoomY = initialYPosition;
        float targetRotationX = initialXRotation;

        while (transposer.m_FollowOffset.y > initialYPosition)
        {
            elapsedTime = Time.time - initialTime;
            float speedFactor = Mathf.Pow(zoomSpeedFactor, elapsedTime);
            transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, Mathf.Lerp(targetYPosition, initialYPosition, speedFactor / zoomDuration), transposer.m_FollowOffset.z);

            float currentRotationX = Mathf.Lerp(targetXRotation, initialXRotation, speedFactor / zoomDuration);
            virtualCamera.transform.eulerAngles = new Vector3(currentRotationX, virtualCamera.transform.eulerAngles.y, virtualCamera.transform.eulerAngles.z);

            yield return null;
        }

        transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, initialYPosition, transposer.m_FollowOffset.z);
        virtualCamera.transform.eulerAngles = new Vector3(initialXRotation, virtualCamera.transform.eulerAngles.y, virtualCamera.transform.eulerAngles.z);

        isZoomedOut = false;
    }
}