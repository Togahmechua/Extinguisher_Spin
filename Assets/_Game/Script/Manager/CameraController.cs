using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera cam;
    [SerializeField] private float lerpSpeed = 2f;

    [Header("=== Camera ===")]
    [SerializeField] private CameraDetails low;
    [SerializeField] private CameraDetails mid;
    [SerializeField] private CameraDetails high;

    [SerializeField] private float yThreshold1 = 10f;
    [SerializeField] private float yThreshold2 = 20f;

    private CameraDetails currentTarget;
    [HideInInspector] public Transform extinguisher;

    [Serializable]
    public class CameraDetails
    {
        public Vector2 camPos;
        public float camSize;
    }

    private void Update()
    {
        if (extinguisher == null)
            return;

        UpdateTargetByHeight();
        SmoothTransition();
        ForceToExtinguisher();
    }

    private void UpdateTargetByHeight()
    {
        float y = extinguisher.position.y;

        if (y < yThreshold1)
            currentTarget = low;
        else if (y < yThreshold2)
            currentTarget = mid;
        else
            currentTarget = high;
    }

    private void SmoothTransition()
    {
        if (currentTarget == null) return;

        cam.transform.position = Vector3.Lerp(
            cam.transform.position,
            new Vector3(currentTarget.camPos.x, currentTarget.camPos.y, cam.transform.position.z),
            Time.deltaTime * lerpSpeed
        );

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, currentTarget.camSize, Time.deltaTime * lerpSpeed);
    }

    private void ForceSnapToTarget()
    {
        cam.transform.position = new Vector3(currentTarget.camPos.x, currentTarget.camPos.y, cam.transform.position.z);
        cam.orthographicSize = currentTarget.camSize;
    }

    private void ForceToExtinguisher()
    {
        cam.transform.position = new Vector3(extinguisher.transform.position.x, cam.transform.position.y, cam.transform.position.z);
    }
}
