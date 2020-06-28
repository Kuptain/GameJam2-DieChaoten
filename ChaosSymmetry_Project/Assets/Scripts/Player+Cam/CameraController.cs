using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    [SerializeField] float lookUpMax = 60;
    [SerializeField] float lookUpMin = -60;
    [SerializeField] float followVectorY;

    float camSmoothingFactor = 1f, smoothTime = 0.3f;

    Camera cam;

    Vector3 lastMousePosition, velocity,  followVector;


    GameObject player;

    private Quaternion camRotation, savedCamRot;
    public bool colliding;
    bool lockY = false;
    public bool playerCollide = false;
    float positionLerp;

    void Start()
    {
        cam = Camera.main;
        velocity = Vector3.zero;

        player = ObjectManager.instance.player;
        transform.SetParent(null);
    }

    void Update()
    {
        //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, player.transform.position, ref velocity, smoothTime);
        RotateCamera();
   

    }
    private void FixedUpdate()
    {
        CollideWithGround();

      

        transform.localPosition = Vector3.Lerp(transform.localPosition, followVector, positionLerp); ;


    }

    void CollideWithGround()
    {
        if (!colliding && !lockY)
        {
            followVector = player.transform.position + new Vector3(0, followVectorY, 0);

            positionLerp = Time.deltaTime * 25;

        }
 
        if (colliding || lockY)
        {
            //!playerCollide
            if (true)
            {
                //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, transform.position + transform.forward * 1.5f, ref velocity, smoothTime);
                followVector = player.transform.position + player.transform.forward * 3f + player.transform.up * 2 + new Vector3(0, followVectorY, 0);

            
                //lockY = true;
                positionLerp = Time.deltaTime * 5f;


            }

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

        if (Input.GetAxis("Mouse Y") < 0)
        {
            lockY = false;
        }
        //transform.rotation = Quaternion.Euler(camRotation.x, transform.rotation.y, transform.rotation.z);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * 35);

        transform.eulerAngles = new Vector3(camRotation.x, transform.eulerAngles.y , 0 );


    }

    private void OnTriggerStay(Collider other)
    {   


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
