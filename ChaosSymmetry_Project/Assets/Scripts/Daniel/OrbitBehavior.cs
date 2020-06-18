using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBehavior : MonoBehaviour
{
    CubeDestroy childScript;
    GameObject childObj;

    Vector3 randomRotate;
    [SerializeField] float maxRotation = 10;

    // Start is called before the first frame update
    void Start()
    {
        childObj = gameObject.transform.GetChild(0).gameObject;

        if (childObj.GetComponent<CubeDestroy>() != null)
        {
            childScript = childObj.GetComponent<CubeDestroy>();

        }

        StartCoroutine(ChangeRotationValue());
    }

    // Update is called once per frame
    void Update()
    {
        if (childScript.pushMode == 1 && childScript.bubbleFreeze == false && childScript.freezeThisCluster == false)
        {
            transform.rotation = Quaternion.Euler(randomRotate.x, randomRotate.y, randomRotate.z);
            //transform.Rotate(randomRotate.x, randomRotate.y, randomRotate.z);
        }
        if (childScript.pushMode == 0 || childScript.bubbleFreeze == true || childScript.freezeThisCluster == true)
        {
            //transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }

    IEnumerator ChangeRotationValue()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            randomRotate = new Vector3(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation));

        }

    }
}
