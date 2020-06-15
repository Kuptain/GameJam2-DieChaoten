using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Camera cam;
    CubeManager cubeManager;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
            
    }

    // Update is called once per frame
    void Update()
    {
        ShootRay();
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void ShootRay()
    {
        //Explode
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position, cam.gameObject.transform.forward);


            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null)
                {
                    CubeDestroy cube = objectHit.gameObject.GetComponent<CubeDestroy>();

                    cube.pushMode = 1;
                    cube.Explode();
                }
            }
        }

        //Send Back

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position, cam.gameObject.transform.forward);


            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null)
                {
                    CubeDestroy cube = objectHit.gameObject.GetComponent<CubeDestroy>();

                    cube.sendingBack = true;
                }
            }
        }
    }
}
