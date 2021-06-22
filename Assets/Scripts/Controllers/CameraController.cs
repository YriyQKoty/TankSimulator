using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float x, y;
    public float sensitivity, distance;
    public Vector2 xMinMax;
    public Transform target;

    private void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * sensitivity * -1;
        y += Input.GetAxis("Mouse Y") * sensitivity;

        x = Mathf.Clamp(x, xMinMax.x, xMinMax.y);

        transform.eulerAngles = new Vector3(x, y + 100, 0);

        transform.position = target.position - transform.forward * distance;
    }
}
