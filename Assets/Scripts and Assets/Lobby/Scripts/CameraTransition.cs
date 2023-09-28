using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float transitionDuration = 2.0f; // Duration of the camera transition

    public GameObject Selector;

    public Transform targetTransform1; // First target transform
    public Transform targetTransform2; // Second target transform
    public Transform targetTransform3; // Third target transform
    public Transform targetTransform4; // Fourth target transform
    public Transform targetTransform5; // Fifth target transform

    public void Start()
    {
        
    }

    public void MoveCameraToTarget1()
    {
        MoveCameraToTarget(targetTransform1);
        Selector.SetActive(true);
    }

    public void MoveCameraToTarget2()
    {
        MoveCameraToTarget(targetTransform2);
    }

    public void MoveCameraToTarget3()
    {
        MoveCameraToTarget(targetTransform3);
    }

    public void MoveCameraToTarget4()
    {
        MoveCameraToTarget(targetTransform4);
    }

    public void MoveCameraToTarget5()
    {
        MoveCameraToTarget(targetTransform5);
    }

    private void MoveCameraToTarget(Transform newTarget)
    {
        if (newTarget != null)
        {
            StartCoroutine(TransitionCamera(newTarget));
        }
        else
        {
            Debug.LogError("newTarget is null. Cannot move camera.");
        }
    }

    private IEnumerator TransitionCamera(Transform newTarget)
    {
        Transform cameraTransform = virtualCamera.transform;
        Vector3 initialPosition = cameraTransform.position;
        Quaternion initialRotation = cameraTransform.rotation;
        Vector3 finalPosition = newTarget.position;
        Quaternion finalRotation = newTarget.rotation;

        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            cameraTransform.position = Vector3.Lerp(initialPosition, finalPosition, elapsedTime / transitionDuration);
            cameraTransform.rotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera reaches the exact final position and rotation
        cameraTransform.position = finalPosition;
        cameraTransform.rotation = finalRotation;
    }
}