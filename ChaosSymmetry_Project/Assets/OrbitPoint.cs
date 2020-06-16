﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitPoint : MonoBehaviour
{
    CubeDestroy childScript;
    GameObject childObj;
    float currentSlowmo;

    Vector3 randomRotate;
    [SerializeField] float maxRotation;

    // Start is called before the first frame update
    void Start()
    {

        maxRotation = CubeManager.instance.orbitMaxRotation;
        randomRotate = new Vector3(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation));

        childObj = gameObject.transform.GetChild(0).gameObject;

        if (childObj.GetComponent<CubeDestroy>() != null)
        {
            childScript = childObj.GetComponent<CubeDestroy>();

        }

        //StartCoroutine(ChangeRotationValue());
    }

    // Update is called once per frame
    void Update()
    {
        currentSlowmo = childScript.currentSlowmo;

        //randomize Rotation
        {
            /*
            float maxRotationAdjust = 1;
            //x
            randomRotate.x += Random.Range(-maxRotationAdjust, maxRotationAdjust);
            if (randomRotate.x > maxRotation)
            {
                randomRotate.x = maxRotation;

            }
            if (randomRotate.x < -maxRotation)
            {
                randomRotate.x = -maxRotation;
            }
            //y
            randomRotate.y += Random.Range(-maxRotationAdjust, maxRotationAdjust);
            if (randomRotate.y > maxRotation)
            {
                randomRotate.y = maxRotation;

            }
            if (randomRotate.y < -maxRotation)
            {
                randomRotate.y = -maxRotation;
            }
            //z
            randomRotate.z += Random.Range(-maxRotationAdjust, maxRotationAdjust);
            if (randomRotate.z > maxRotation)
            {
                randomRotate.z = maxRotation;

            }
            if (randomRotate.z < -maxRotation)
            {
                randomRotate.z = -maxRotation;
            }
            */

        }


        if (childScript.pushMode == 1 && childScript.freezeThis == false)
        {            
            transform.Rotate(randomRotate.x * Time.deltaTime * currentSlowmo,
                             randomRotate.y * Time.deltaTime * currentSlowmo,
                             randomRotate.z * Time.deltaTime * currentSlowmo);

        }
        if (childScript.pushMode == 0)
        {

            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 0, 0, 0), 0.02f);

        }
    }

    IEnumerator ChangeRotationValue()
    {
        while (true)
        {
            yield return new WaitForSeconds(0);
            randomRotate = new Vector3(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation));

        }

    }
}
