using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject spawnedCubePrefab;
    CubeManager cubeManager;
    TutorialManager tm;
    //[HideInInspector] 
    public GameObject currentCluster, frozenCluster, secondFrozenCluster;
    public float meltingTime = 5;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cubeManager = CubeManager.instance;
        tm = TutorialManager.instance;

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

    GameObject spawnedCube;
    [SerializeField] float spawnDistance = 15;


    void ShootRay()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position + cam.gameObject.transform.forward * 2, cam.gameObject.transform.forward);


            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false)
                {
                    currentCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                    
                    if(tm.currentHint == "left")
                    {
                        tm.ChangeType("slow");
                    }

                    foreach (Transform child in currentCluster.transform)
                    {
                        foreach (Transform childChild in child)
                        {
                            if (childChild.gameObject.GetComponent<CubeDestroy>() != null )
                            {
                                childChild.gameObject.GetComponent<CubeDestroy>().pushMode = 1;
                                childChild.gameObject.GetComponent<CubeDestroy>().Explode();
                            }                           
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position + cam.gameObject.transform.forward * 2, cam.gameObject.transform.forward);


            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                if (tm.currentHint == "right")
                {
                    tm.ChangeType("float");
                }

                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false && frozenCluster == null)
                {
                    frozenCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                    foreach (Transform child in frozenCluster.transform)
                    {
                        foreach (Transform childChild in child )
                        {
                            if (childChild.gameObject.GetComponent<CubeDestroy>() != null )
                            {
                                childChild.gameObject.GetComponent<CubeDestroy>().freezeThisCluster = true;
                                childChild.gameObject.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;
                                StartCoroutine(Defreeze(frozenCluster));
                            }
                        }
                    }
                }

                else if (objectHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false && frozenCluster != null)
                {
                    if (PowerUpManager.instance.currentPowerUp == "secondClusterFreeze" && secondFrozenCluster == null)
                    {
                        secondFrozenCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                        foreach (Transform child in secondFrozenCluster.transform)
                        {                        
                            foreach (Transform childChild in child)
                            {
                                if (childChild.gameObject.GetComponent<CubeDestroy>() != null )
                                {
                                    childChild.gameObject.GetComponent<CubeDestroy>().freezeThisCluster = true;
                                    childChild.gameObject.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;
                                    StartCoroutine(DefreezeClusterTwo(secondFrozenCluster));
                                }
                            }
                        }
                    }
                }
            }
        }

        //Cube Spawn PowerUp
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                spawnedCube = Instantiate(spawnedCubePrefab, cam.transform.position + cam.transform.forward * spawnDistance, Quaternion.identity) as GameObject;

                if (spawnedCube != null)
                {
                    //spawnedCube.transform.GetChild(1).gameObject.SetActive(true);
                    spawnedCube.transform.GetChild(2).gameObject.SetActive(true);

                    spawnedCube.GetComponent<Collider>().enabled = false;
                    spawnedCube.GetComponent<MeshRenderer>().enabled = false;

                }


            }
            if (Input.GetKey(KeyCode.Q))
            {
                if (spawnedCube != null)
                {

                    spawnedCube.transform.position = Vector3.Lerp(spawnedCube.transform.position, cam.transform.position + cam.transform.forward * spawnDistance, 0.05f);
                }
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                if (spawnedCube != null)
                {
                    //spawnedCube.transform.GetChild(1).gameObject.SetActive(false);
                    if (spawnedCube.transform.childCount > 0)
                    {
                        spawnedCube.transform.GetChild(2).gameObject.SetActive(false);

                    }
                    spawnedCube.GetComponent<Collider>().enabled = true;
                    spawnedCube.GetComponent<MeshRenderer>().enabled = true;

                }
            }
        }
      
    }

    void HoverCursor()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position + cam.gameObject.transform.forward * 2, cam.gameObject.transform.forward);


        if (Physics.Raycast(ray, out hit))
        {
            Transform clusterHit = hit.transform;
            if (cubeManager.clusterHasShader && clusterHit.gameObject.GetComponent<CubeDestroy>() != null )
            {
                currentCluster = clusterHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                foreach (Transform child in currentCluster.transform)
                {
                    foreach (Transform childChild in child)
                    {
                        if (childChild.gameObject.GetComponent<CubeDestroy>() != null && childChild.gameObject.GetComponent<CubeDestroy>().freezeThisCluster == false)
                        {
                            if (childChild.gameObject.GetComponent<CubeDestroy>().pushMode == 0 && childChild.childCount > 0)
                            {

                                //childChild.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", childChild.gameObject.GetComponent<CubeDestroy>().colorHover);
                                childChild.GetChild(0).gameObject.SetActive(true);

                                childChild.gameObject.GetComponent<CubeDestroy>().isHovered = true;


                            }

                            if (childChild.gameObject.GetComponent<CubeDestroy>().pushMode == 1 && childChild.childCount > 0)
                            {

                                //childChild.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", childChild.gameObject.GetComponent<CubeDestroy>().colorHoverExploded);
                                childChild.GetChild(0).gameObject.SetActive(true);
                                childChild.gameObject.GetComponent<CubeDestroy>().isHovered = true;

                            }
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
        }
        frozenCluster = null;
    }
    public IEnumerator DefreezeClusterTwo(GameObject cluster)
    {
        yield return new WaitForSeconds(meltingTime);
        foreach (Transform child in cluster.transform)
        {
            child.GetChild(0).GetComponent<CubeDestroy>().freezeThisCluster = false;
        }
        secondFrozenCluster = null;
    }
}
