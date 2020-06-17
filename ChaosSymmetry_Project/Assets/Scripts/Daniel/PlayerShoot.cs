using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Camera cam;
    CubeManager cubeManager;
    [HideInInspector] public GameObject currentCluster, frozenCluster;
    public float meltingTime = 5;

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

        /*Show Cursor with Control
         * 
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
        */
    }

    void ShootRay()
    {     

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
                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false && frozenCluster == null)
                {
                    frozenCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                    foreach (Transform child in frozenCluster.transform)
                    {
                        foreach (Transform childChild in child)
                        {
                            childChild.gameObject.GetComponent<CubeDestroy>().freezeThisCluster = true;
                            childChild.gameObject.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;
                            StartCoroutine(Defreeze(frozenCluster));
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
            //&& CubeManager.instance.gameModeAllClusters == false
            if (clusterHit.gameObject.GetComponent<CubeDestroy>() != null )
            {
                currentCluster = clusterHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                foreach (Transform child in currentCluster.transform)
                {

                    foreach (Transform childChild in child)
                    {

                        if (childChild.gameObject.GetComponent<CubeDestroy>().pushMode == 0)
                        {

                            childChild.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", childChild.gameObject.GetComponent<CubeDestroy>().colorHover);
                            childChild.gameObject.GetComponent<CubeDestroy>().isHovered = true;


                        }

                        if (childChild.gameObject.GetComponent<CubeDestroy>().pushMode == 1)
                        {
                            Debug.Log("Check4.2");

                            childChild.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", childChild.gameObject.GetComponent<CubeDestroy>().colorHoverExploded);
                            childChild.gameObject.GetComponent<CubeDestroy>().isHovered = true;

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
            child.GetChild(0).GetComponent<CubeDestroy>().freezeThisCluster = false;
            print("defreeze");
        }
        frozenCluster = null;
    }
}
