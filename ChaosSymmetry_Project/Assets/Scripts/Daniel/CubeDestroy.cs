using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour
{
    [SerializeField] Color colorOne;
    [SerializeField] Color colorTwo;
    [SerializeField] Color colorThree;

    float pushForce;
    Vector3 startPosition;
    Quaternion startRotation;
    Rigidbody rigid;
    int pushMode;



    void Start()
    {

        if (gameObject.GetComponent<Renderer>() != null)
        {
            Debug.Log("Has Mat");
            int randomNumber = Random.Range(1, 4);
            if (randomNumber == 1)
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorOne);
            }
            if (randomNumber == 2)
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorTwo);
            }
            if (randomNumber == 3)
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorThree);
            }
        }

        rigid = gameObject.GetComponent<Rigidbody>();
        pushForce = CubeManager.instance.pushForce;
        startPosition = transform.position;
        startRotation = transform.rotation;

        StartCoroutine(Force());
    }

    IEnumerator Force()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(-1f, 1f));

            
            if (pushMode == 1)
            {
                PushFunction(pushForce);
            }
          
        }
      
    }

    void PushFunction(float force)
    {
        Vector3 newForce = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * force;
        //Vector3 newForce = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * pushForce;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.useGravity = true;
        rigid.AddForce(newForce, ForceMode.Impulse);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pushMode = 0;
            PushFunction(pushForce/2);
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            pushMode = 1;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            pushMode = 0;
            transform.position = Vector3.Lerp(transform.position, startPosition, 0.05f);
            transform.rotation = startRotation;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;

            rigid.useGravity = false;


        }
    }
}
