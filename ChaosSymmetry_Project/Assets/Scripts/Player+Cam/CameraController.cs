using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    [SerializeField] float lookUpMax = 60;
    [SerializeField] float lookUpMin = -60;

    float camSmoothingFactor = 1f, smoothTime = 0.3f;

    Camera cam;

    Vector3 lastMousePosition, velocity, defaultPos;

    private Quaternion camRotation, savedCamRot;
    public bool colliding;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
        velocity = Vector3.zero;
        defaultPos = transform.localPosition;
    }

    void Update()
    {
        CollideWithGround();
        RotateCamera();
    }

    void CollideWithGround()
    {
        if (colliding)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(0, 1, 0), ref velocity, smoothTime);
        }
        else
        {
            RaycastHit hit;

            Ray ray = new Ray(cam.transform.position, -cam.gameObject.transform.forward);

            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                print("ray has hit");
            }
            else
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, defaultPos, ref velocity, smoothTime);
            }
        }
    }

    private void RotateCamera()
    {
        savedCamRot.x = camRotation.x;
        camRotation.x += Input.GetAxis("Mouse Y") * camSmoothingFactor * (-1);
        //camRotation.y += Input.GetAxis("Mouse X") * camSmoothingFactor;

        camRotation.x = Mathf.Clamp(camRotation.x, lookUpMin, lookUpMax);
        if (colliding && Input.GetAxis("Mouse Y") > 0)
        {
            //camRotation.x = savedCamRot.x;
                //Mathf.Clamp(camRotation.x, -30, lookUpMax);
        }

        transform.localRotation = Quaternion.Euler(camRotation.x, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {   

        if (GetComponent<Collider>().GetType() == typeof(SphereCollider) && other.gameObject.CompareTag("terrain") && other.gameObject.GetComponent<CubeDestroy>() == false)
        {
            colliding = true;

        }

        if ( other.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.clusterHasShader)
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GetComponent<Collider>().GetType() == typeof(SphereCollider) && other.gameObject.CompareTag("terrain") && other.gameObject.GetComponent<CubeDestroy>() == false)
        {
            colliding = false;
        }

        if (other.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.clusterHasShader)
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            other.gameObject.transform.GetChild(2).gameObject.SetActive(false);

        }
    }

}
