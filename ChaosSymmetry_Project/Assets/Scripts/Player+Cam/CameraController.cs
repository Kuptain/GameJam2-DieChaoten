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
    GameObject player;

    private Quaternion camRotation, savedCamRot;
    public bool colliding;

    void Start()
    {
        cam = Camera.main;
        velocity = Vector3.zero;
        defaultPos = transform.localPosition;

        player = ObjectManager.instance.player;
        transform.SetParent(null);
    }

    void Update()
    {
        CollideWithGround();
        //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, player.transform.position, ref velocity, smoothTime);
        RotateCamera();

    }
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * 30);

        /*
        transform.Rotate(transform.rotation.x - rotationX,
                         0,
                         transform.rotation.z - rotationZ);
        */

    }

    void CollideWithGround()
    {
        if (colliding)
        {
            //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(0, 1, 0), ref velocity, smoothTime);
        }
        else
        {
            RaycastHit hit;

            Ray ray = new Ray(cam.transform.position, - cam.gameObject.transform.forward);

            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                //print("ray has hit");
            }
            else
            {
                //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, defaultPos, ref velocity, smoothTime);
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

        //transform.rotation = Quaternion.Euler(camRotation.x, transform.rotation.y, transform.rotation.z);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * 35);

        transform.eulerAngles = new Vector3(camRotation.x, transform.eulerAngles.y , 0 );


    }

    private void OnTriggerStay(Collider other)
    {   

        if (GetComponent<Collider>().GetType() == typeof(SphereCollider) && other.gameObject.CompareTag("terrain") && other.gameObject.GetComponent<CubeDestroy>() == false)
        {
            colliding = true;

        }

        if ( other.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.clusterHasShader)
        {
            if (other.GetComponent<MeshRenderer>() != null)
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;

            }
            if (other.transform.childCount > 3)
            {
                for (int i = 3; i < other.transform.childCount; i++)
                {
                    if (other.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                    {
                        other.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                    }
                    foreach (Transform child in other.transform.GetChild(i))
                    {
                        if (child.gameObject.GetComponent<MeshRenderer>() != null)
                        {
                            child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        }
                      
                    }
                }
            }
            
            other.gameObject.transform.GetChild(2).gameObject.SetActive(true);

        }
        if (other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.childCount > 3 && other.transform.parent.parent.gameObject.GetComponent<CubeDestroy>() != null)
        {
            other.transform.parent.parent.gameObject.transform.GetChild(2).gameObject.SetActive(true);

            for (int i = 3; i < other.transform.parent.parent.childCount; i++)
            {
                if (other.transform.parent.parent.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    other.transform.parent.parent.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                }
                foreach (Transform child in other.transform.parent.parent.GetChild(i))
                {
                    if (child.gameObject.GetComponent<MeshRenderer>() != null)
                    {
                        child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    }

                }
            }
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
            if (other.GetComponent<MeshRenderer>() != null)
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;

            }
            if (other.transform.childCount > 3)
            {
                for (int i = 3; i < other.transform.childCount; i++)
                {
                    if (other.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                    {
                        other.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                    }
                    foreach (Transform child in other.transform.GetChild(i))
                    {
                        if (child.gameObject.GetComponent<MeshRenderer>() != null)
                        {
                            child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                        }
                    
                    }
                }
            }            
            other.gameObject.transform.GetChild(2).gameObject.SetActive(false);

        }

        if (other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.childCount > 3 && other.transform.parent.parent.gameObject.GetComponent<CubeDestroy>() != null)
        {
            other.transform.parent.parent.gameObject.transform.GetChild(2).gameObject.SetActive(false);

            for (int i = 3; i < other.transform.parent.parent.childCount; i++)
            {
                if (other.transform.parent.parent.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    other.transform.parent.parent.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                }
                foreach (Transform child in other.transform.parent.parent.GetChild(i))
                {
                    if (child.gameObject.GetComponent<MeshRenderer>() != null)
                    {
                        child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }

                }
            }
        }
    }

}
