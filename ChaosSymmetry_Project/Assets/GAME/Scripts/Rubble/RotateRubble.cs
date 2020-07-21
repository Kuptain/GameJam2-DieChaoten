using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRubble : MonoBehaviour
{
    CubeManager cm;
    float maxRotation;
    Vector3 randomRotate;
    public bool exploding;
    GameObject platform;
    Vector3 startPosition;
    CubeDestroy platformScript;
    Rigidbody rigid;
    float sendBackManual;
    Quaternion startRotation;
    public bool doOnce, sentback;

    // Start is called before the first frame update
    void Start()
    {
        cm = CubeManager.instance;
        maxRotation = cm.orbitMaxRotation;
        startPosition = transform.position;
        if (transform.parent.GetComponent<RubbleExplosion>() != null)
        {
            startRotation = transform.rotation;
            sendBackManual = cm.sendBackManual;
            platform = gameObject.transform.parent.transform.parent.GetChild(0).GetChild(0).gameObject;
            platformScript = platform.GetComponent<CubeDestroy>();
            rigid = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<RubbleExplosion>() != null)
        {
            if (exploding)
            {
                transform.Rotate(randomRotate.x * Time.deltaTime * cm.currentSlowmo,
                            randomRotate.y * Time.deltaTime * cm.currentSlowmo,
                            randomRotate.z * Time.deltaTime * cm.currentSlowmo);

            }

            if (platformScript.sendingBack == true && sentback == false)
            {
                SendBack();
            }
        }
    }

    public void RandomizeRotation()
    {
        randomRotate = new Vector3(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation));

    }

    public void SendBack()
    {
        if (platformScript.freezeThisCluster == false && platformScript.bubbleFreeze == false)
        {
            //pushMode = 0;
            transform.position = Vector3.Slerp(transform.position, startPosition, sendBackManual * cm.currentSlowmo);
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, sendBackManual * cm.currentSlowmo);

            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
        }
        else if (platformScript.freezeThisCluster == true || platformScript.bubbleFreeze == true)
        {
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
        }
        if (doOnce == false)
        {
            StartCoroutine(SendingBackFalseTimer());
            doOnce = true;
        }
    }

    IEnumerator SendingBackFalseTimer()
    {
        yield return new WaitForSeconds(3f);
        sentback = true;
        exploding = false;
        doOnce = false;
        print("hhhhhhaa");
    }
}
