using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitPoint : MonoBehaviour
{
    public bool isCluster = false;
    [HideInInspector] public bool canRotate = false;

    CubeDestroy childScript;
    CubeManager cm;
    GameObject childObj;

    Vector3 randomRotate;
    float maxRotation;


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.GetChild(0) != null)
        {
            childObj = gameObject.transform.GetChild(0).gameObject;

        }
        cm = CubeManager.instance.GetComponent<CubeManager>();

        if (isCluster)
        {
            maxRotation = CubeManager.instance.clusterMaxRotation;
            RandomizeRotation();

        }
        else
        {
            maxRotation = CubeManager.instance.orbitMaxRotation;

            if (childObj.GetComponent<CubeDestroy>() != null)
            {
                childScript = childObj.GetComponent<CubeDestroy>();
            }
        }       

        //StartCoroutine(ChangeRotationValue());
    }
    private void FixedUpdate()
    {
        if (isCluster)
        {
            if (CheckChildrenFreeze())
            {
                RotateCluster();

            }

        }
        else
        {
            if (childScript != null)
            {
                if (childScript.freezeThisCluster == false && childScript.bubbleFreeze == false)
                {
                    canRotate = true;
                    RotateElements();

                }
                else
                {
                    canRotate = false;
                }
            }

        }
    }
    // Update is called once per frame
    void Update()
    {

        

      
    }

    void CheckCheckpointCollision()
    {

    }

    bool CheckChildrenFreeze()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<OrbitPoint>() != null && child.GetComponent<OrbitPoint>().canRotate == false)
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckChildrenNotMoving()
    {
        foreach (Transform child in transform)
        {
            if (child.GetChild(0).GetComponent<CubeDestroy>() != null && child.GetChild(0).GetComponent<CubeDestroy>().pushMode == 1)
            {
                return false;
            }
        }
        return true;
    }
    void RotateElements()
    {
        if (childScript.pushMode == 1)
        {
            transform.Rotate(randomRotate.x * Time.deltaTime * cm.currentSlowmo,
                             randomRotate.y * Time.deltaTime * cm.currentSlowmo,
                             randomRotate.z * Time.deltaTime * cm.currentSlowmo);

        }
        if (childScript.pushMode == 0 )
        {
            randomRotate = Vector3.Lerp(randomRotate, Vector3.zero, 0.005f);
            transform.Rotate(randomRotate.x * Time.deltaTime * cm.currentSlowmo,
                       randomRotate.y * Time.deltaTime * cm.currentSlowmo,
                       randomRotate.z * Time.deltaTime * cm.currentSlowmo);

            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 0, 0, 0), CubeManager.instance.sendBackManual * cm.currentSlowmo);


        }
    }
    void RotateCluster()
    {
        transform.Rotate(randomRotate.x * Time.deltaTime * cm.currentSlowmo,
                             randomRotate.y * Time.deltaTime * cm.currentSlowmo,
                             randomRotate.z * Time.deltaTime * cm.currentSlowmo);
    }
    public void RandomizeRotation()
    {
        randomRotate = new Vector3(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation));

    }



}
