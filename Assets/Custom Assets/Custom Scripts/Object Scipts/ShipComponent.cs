using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ShipComponent : MonoBehaviour
{
    public Action OnRotationStatusChanged;
    private static bool isRotating = false;
    public static bool IsRotating
    {
        get { return isRotating; }
        private set { isRotating = value; }
    }
    private void UpdateRotationStatus(bool isRotating)
    {
        IsRotating = isRotating;
        OnRotationStatusChanged?.Invoke();
    }

    private readonly Queue<RotationRequest> rotationQueue = new Queue<RotationRequest>();

    private struct RotationRequest
    {
        public Vector3 Axis;
        public float Angle;

        public RotationRequest (Vector3 axis, float angle)
        {
            Axis = axis;
            Angle = angle;
        }
    }

    private void EnqueueRotation (Vector3 axis, float angle)
    {
        rotationQueue.Enqueue(new RotationRequest(axis, angle));
        if (!IsRotating)
        {
            ProcessNextRotationRequest();
        }
    }

    private void ProcessNextRotationRequest ()
    {
        if (rotationQueue.Count > 0)
        {
            var request = rotationQueue.Dequeue();
            StartCoroutine(RotateOverTime(request.Axis, request.Angle, 1f));
        }
    }

    public void HandleAxisYForLeftClick ()
    {
        //if (!isRotating)
        //{
        //    Vector3 rotationAxis = Vector3.up;
        //    float angle = 90f;
        //    StartCoroutine(RotateOverTime(rotationAxis, angle, 1f));
        //}
        EnqueueRotation(Vector3.up, 90f);
    }
    public void HandleAxisYForRightClick ()
    {
        //if (!isRotating)
        //{
        //    Vector3 rotationAxis = Vector3.down;
        //    float angle = 90f;
        //    StartCoroutine(RotateOverTime(rotationAxis, angle, 1f));
        //}
        EnqueueRotation(Vector3.down, 90f);
    }
    public void HandleAxisXForUpClick ()
    {
        //if (!isRotating)
        //{
        //    Vector3 rotationAxis = Vector3.right;
        //    float angle = 90f;
        //    StartCoroutine(RotateOverTime(rotationAxis, angle, 1f));
        //}
        EnqueueRotation(Vector3.right, 90f);
    }
    public void HandleAxisForDownClick ()
    {
        //if (!isRotating)
        //{
        //    Vector3 rotationAxis = Vector3.left;
        //    float angle = 90f;
        //    StartCoroutine(RotateOverTime(rotationAxis, angle, 1f));
        //}
        EnqueueRotation(Vector3.left, 90f);
    }

    private IEnumerator RotateOverTime (Vector3 rotationAxis, float angle, float duration)
    {
        UpdateRotationStatus(true);

        Quaternion startRotation = this.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(rotationAxis * angle) * startRotation;
        float time = 0.0f;

        while (time < duration)
        {
            this.transform.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        this.transform.rotation = endRotation;
        UpdateRotationStatus(false);
        ProcessNextRotationRequest();
    }

    public float rotationSpeed = 300.0f;

    private void Update ()
    {
        if (Input.GetMouseButton(1))
        {
            UpdateRotationStatus(true);
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            Quaternion rotX = Quaternion.AngleAxis(-mouseX, Vector3.up);
            Quaternion rotY = Quaternion.AngleAxis(mouseY, Vector3.right);

            transform.rotation = transform.rotation * rotX * rotY;
            UpdateRotationStatus(false);
        }
    }
}
