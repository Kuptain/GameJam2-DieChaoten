using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Camera cam;
    CubeManager cubeManager;
    GameObject currentCluster;
    [SerializeField] float meltingTime = 5;

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
        HoverCursor();
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
        /* if (Input.GetMouseButton(0))
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
         }*/

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position, cam.gameObject.transform.forward);


            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false)
                {
                    currentCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                    foreach (Transform child in currentCluster.transform)
                    {
                        foreach (Transform childChild in child)
                        {
                            childChild.gameObject.GetComponent<CubeDestroy>().pushMode = 1;
                            childChild.gameObject.GetComponent<CubeDestroy>().Explode();
                        }

                                         }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position, cam.gameObject.transform.forward);


            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false)
                {
                    currentCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                    foreach (Transform child in currentCluster.transform)
                    {

                        foreach (Transform childChild in child)
                        {

                            childChild.gameObject.GetComponent<CubeDestroy>().freezeThis = true;
                            childChild.gameObject.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;
                            StartCoroutine(Defreeze(objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject));

                        }
                    }
                }
            }
        }
    }

    void HoverCursor()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.gameObject.transform.forward);


        if (Physics.Raycast(ray, out hit))
        {
            Transform clusterHit = hit.transform;
            if (clusterHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false)
            {
                currentCluster = clusterHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                foreach (Transform child in currentCluster.transform)
                {
                    foreach (Transform childChild in child)
                    {
                        if(childChild.gameObject.GetComponent<CubeDestroy>().pushMode == 0)
                        {
                            childChild.gameObject.GetComponent<Renderer>().material.SetColor("_Color", childChild.gameObject.GetComponent<CubeDestroy>().colorHover);
                            print("aaaa");
                        }
                  
                    }
                }
            }
        }
    }

    public IEnumerator Defreeze(GameObject cluster)
    {
        yield return new WaitForSeconds(meltingTime);
        foreach (Transform child in cluster.transform)
        {
            child.GetChild(0).GetComponent<CubeDestroy>().freezeThis = false;
        }
    }
}
