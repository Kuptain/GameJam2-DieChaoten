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
    public int cubeSpawnCharges = 0; 
    GameObject spawnedCube;
    float spawnDistance = 12;

    float rayForwardMultiply = 7;

    // Start is called before the first frame update
    void Start()
    {
        cubeManager = CubeManager.instance;
        tm = TutorialManager.instance;
        spawnDistance = 30;

    }

    // Update is called once per frame
    void Update()
    {
        ShootRay();
        HoverCursor();
    }

    void ShootRay()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position + cam.gameObject.transform.forward * rayForwardMultiply, cam.gameObject.transform.forward);


            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false)
                {
                    currentCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent

                    int chance = Random.Range(1, 3);
                    if(chance == 1)
                    {
                        AudioManager.instance.clusterBreak.PlayOneShot(AudioManager.instance.breakOne, 0.5f);
                    }
                    else if (chance == 2)
                    {
                        AudioManager.instance.clusterBreak.PlayOneShot(AudioManager.instance.breakTwo, 0.5f);
                    }
                    else if (chance == 3)
                    {
                        AudioManager.instance.clusterBreak.PlayOneShot(AudioManager.instance.breakThree, 0.5f);
                    }

                    if (tm.currentHint == "left")
                    {
                        tm.ChangeType("slow");
                    }

                    foreach (Transform child in currentCluster.transform)
                    {
                        foreach (Transform childChild in child)
                        {
                            if (childChild.gameObject.GetComponent<CubeDestroy>() != null)
                            {
                                childChild.gameObject.GetComponent<CubeDestroy>().pushMode = 1;
                                childChild.gameObject.GetComponent<CubeDestroy>().Explode();
                            }
                        }
                        if (child.gameObject.GetComponent<OrbitForRubble>() != null)
                        {
                            child.gameObject.GetComponent<OrbitForRubble>().RandomizeRotation();
                        }
                        if (child.gameObject.GetComponent<RubbleExplosion>() != null)
                        {
                            child.gameObject.GetComponent<RubbleExplosion>().Explode();
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position + cam.gameObject.transform.forward * rayForwardMultiply, cam.gameObject.transform.forward);


            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                if (tm.currentHint == "right")
                {
                    tm.ChangeType("float");
                }

                if (objectHit.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.gameModeAllClusters == false && frozenCluster == null)
                {
                    int chance = Random.Range(1, 3);
                    if (chance == 1)
                    {
                        AudioManager.instance.clusterFreeze.PlayOneShot(AudioManager.instance.freezeOne, 0.6f);
                    }
                    else if (chance == 2)
                    {
                        AudioManager.instance.clusterFreeze.PlayOneShot(AudioManager.instance.freezeTwo, 0.6f);
                    }
                    else if (chance == 3)
                    {
                        AudioManager.instance.clusterFreeze.PlayOneShot(AudioManager.instance.freezeThree, 0.6f);
                    }

                    frozenCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                    foreach (Transform child in frozenCluster.transform)
                    {
                        foreach (Transform childChild in child)
                        {
                            if (childChild.gameObject.GetComponent<CubeDestroy>() != null)
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
                        int chance = Random.Range(1, 3);
                        if (chance == 1)
                        {
                            AudioManager.instance.clusterFreeze.PlayOneShot(AudioManager.instance.freezeOne, 0.6f);
                        }
                        else if (chance == 2)
                        {
                            AudioManager.instance.clusterFreeze.PlayOneShot(AudioManager.instance.freezeTwo, 0.6f);
                        }
                        else if (chance == 3)
                        {
                            AudioManager.instance.clusterFreeze.PlayOneShot(AudioManager.instance.freezeThree, 0.6f);
                        }

                        secondFrozenCluster = objectHit.gameObject.transform.parent.gameObject.transform.parent.gameObject; //The parent's parent
                        foreach (Transform child in secondFrozenCluster.transform)
                        {
                            foreach (Transform childChild in child)
                            {
                                if (childChild.gameObject.GetComponent<CubeDestroy>() != null)
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
            if (Input.GetKeyDown(KeyCode.Q) && cubeSpawnCharges > 0)
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
            if (Input.GetKey(KeyCode.Q) && cubeSpawnCharges > 0)
            {
                if (spawnedCube != null)
                {

                    spawnedCube.transform.position = Vector3.Lerp(spawnedCube.transform.position, cam.transform.position + cam.transform.forward * spawnDistance, 0.05f);
                }
            }
            if (Input.GetKeyUp(KeyCode.Q) && cubeSpawnCharges > 0)
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
                    cubeSpawnCharges -= 1;
                }
            }
        }

    }

    GameObject lastPowerUp;
    void HoverCursor()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position + cam.gameObject.transform.forward * rayForwardMultiply, cam.gameObject.transform.forward);


        if (Physics.Raycast(ray, out hit))
        {
            Transform clusterHit = hit.transform;
            if (cubeManager.clusterHasShader && clusterHit.gameObject.GetComponent<CubeDestroy>() != null)
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


            if (lastPowerUp != null)
            {
                Debug.Log("notHovered");

                lastPowerUp.transform.GetChild(0).gameObject.GetComponent<RandomPowerUp>().isHovered = false;
                lastPowerUp = null;
            }

            if (clusterHit.gameObject.GetComponent<_PowerUpCollider>() != null)
            {
                Debug.Log("hoveredParent");
                lastPowerUp = clusterHit.gameObject;
                clusterHit.GetChild(0).gameObject.GetComponent<RandomPowerUp>().isHovered = true;
            }

            if (clusterHit.gameObject.GetComponent<RandomPowerUp>() != null)
            {
                Debug.Log("hoveredChild");
                lastPowerUp = clusterHit.gameObject;
                clusterHit.gameObject.GetComponent<RandomPowerUp>().isHovered = true;
            }
            /*
            else if (lastPowerUp != null)
            {
                //Debug.Log("notHovered");

                lastPowerUp.transform.GetChild(0).gameObject.GetComponent<RandomPowerUp>().isHovered = false;
                lastPowerUp = null;
            }
            */
        }
    }

    public IEnumerator Defreeze(GameObject cluster)
    {
        yield return new WaitForSeconds(meltingTime);
        foreach (Transform child in cluster.transform)
        {
            if (child.GetChild(0).GetComponent<CubeDestroy>() != null)
                child.GetChild(0).GetComponent<CubeDestroy>().freezeThisCluster = false;
        }
        frozenCluster = null;
    }
    public IEnumerator DefreezeClusterTwo(GameObject cluster)
    {
        yield return new WaitForSeconds(meltingTime);
        foreach (Transform child in cluster.transform)
        {
            if (child.GetChild(0).GetComponent<CubeDestroy>() != null)
                child.GetChild(0).GetComponent<CubeDestroy>().freezeThisCluster = false;
        }
        secondFrozenCluster = null;
    }
}
