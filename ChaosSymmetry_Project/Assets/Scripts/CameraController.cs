using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    [SerializeField] float lookUpMax = 60;
    [SerializeField] float lookUpMin = -60;

    float camSmoothingFactor = 1f;

    Camera cam;

    Vector3 lastMousePosition;

    private Quaternion camRotation;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
    }

    void Update()
    {

        RotateCamera();

    }

 

    private void RotateCamera()
    {
        camRotation.x += Input.GetAxis("Mouse Y") * camSmoothingFactor * (-1);
        //camRotation.y += Input.GetAxis("Mouse X") * camSmoothingFactor;

        camRotation.x = Mathf.Clamp(camRotation.x, lookUpMin, lookUpMax);

        transform.localRotation = Quaternion.Euler(camRotation.x, 0, 0);
    }


}
