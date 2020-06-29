using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitForRubble : MonoBehaviour
{
    [HideInInspector] public bool canRotate = false;

    CubeDestroy childScript;
    CubeManager cm;
    GameObject childObj;

    Vector3 randomRotate;
    float maxRotation;


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent.GetChild(0).GetChild(0) != null)
        {
            childObj = gameObject.transform.parent.GetChild(0).GetChild(0).gameObject;
        }
        cm = CubeManager.instance.GetComponent<CubeManager>();

        maxRotation = CubeManager.instance.orbitMaxRotation;

        if (childObj.GetComponent<CubeDestroy>() != null)
        {
            childScript = childObj.GetComponent<CubeDestroy>();
        }
    }

    // Update is called once per frame
    void Update()
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


    void RotateElements()
    {
        if (childScript.pushMode == 1)
        {
            transform.Rotate(randomRotate.x * Time.deltaTime * cm.currentSlowmo,
                             randomRotate.y * Time.deltaTime * cm.currentSlowmo,
                             randomRotate.z * Time.deltaTime * cm.currentSlowmo);
        }
        if (childScript.pushMode == 0)
        {
            randomRotate = Vector3.Lerp(randomRotate, Vector3.zero, 0.005f);
            transform.Rotate(randomRotate.x * Time.deltaTime * cm.currentSlowmo,
                       randomRotate.y * Time.deltaTime * cm.currentSlowmo,
                       randomRotate.z * Time.deltaTime * cm.currentSlowmo);
            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 0, 0, 0), CubeManager.instance.sendBackManual * cm.currentSlowmo);
        }
    }
    public void RandomizeRotation()
    {
        randomRotate = new Vector3(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation));

    }
}
