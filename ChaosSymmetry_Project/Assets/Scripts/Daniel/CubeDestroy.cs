using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour
{
    [SerializeField] Color colorOne;
    [SerializeField] Color colorTwo;
    [SerializeField] Color colorThree;
    [SerializeField] float speed;
    [SerializeField] float slowmoStrength = 0.25f;
    [SerializeField] float gravityValue = 2f; //The value of the gravity
    [SerializeField] float maxGravity = 2f; //gravityChange can not be higher than this

    [HideInInspector] public int pushMode;
    [HideInInspector] public bool sendingBack;

    CubeManager cubeManager;
    float gravityChange; //This will be changed and added to the object

    float finalSpeed;
    float finalSlowmo = 1;
    bool colliding;

    float pushForce;
    Vector3 startPosition;
    Vector3 moveVelocity;
    Quaternion startRotation;
    Rigidbody rigid;


    


    void Start()
    {
        /*
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
        */
        cubeManager = CubeManager.instance.GetComponent<CubeManager>();

        rigid = gameObject.GetComponent<Rigidbody>();
        pushForce = CubeManager.instance.pushForce;
        startPosition = transform.position;
        startRotation = transform.rotation;
        finalSpeed = speed;

        StartCoroutine(Force());
    }

    IEnumerator Force()
    {
        while (true )
        {
          
            yield return new WaitForSeconds(Random.Range(-0.75f, 0.75f));

            if (finalSlowmo == 1)
            {
                if (pushMode == 2)
                {
                    Explode();

                }
            }

        }
      
    }

    //##################
    //>>>>>>>>>>>>> OLD 
    void PushFunction(float force)
    {
        /*
        Vector3 newForce = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * force;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.useGravity = true;
        rigid.AddForce(newForce, ForceMode.Impulse);
        */
    }

    void InputPush()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            pushMode = 0;
            PushFunction(pushForce / 2);
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
        */
    }
    //OLD <<<<<<<<<<<<<<<
    //###################



    //New "manual" push mode for adding the slowmotion effect
    public void Explode()
    {
        colliding = false;
        rigid.constraints = RigidbodyConstraints.None;
        moveVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
        if( cubeManager.testMode == 0)
        {
            sendingBack = false;

        }

    }

    public void SendBack()
    {
      
        pushMode = 0;
        transform.position = Vector3.Slerp(transform.position, startPosition, 0.02f);

        //transform.position = new Vector3(Mathf.InverseLerp(transform.position.x, startPosition.x, 0.2f),
        //                                 Mathf.InverseLerp(transform.position.y, startPosition.y, 0.2f),
        //                                 Mathf.InverseLerp(transform.position.z, startPosition.z, 0.2f));

        transform.rotation = startRotation;
        moveVelocity = new Vector3(0, 0, 0);

        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        rigid.constraints = RigidbodyConstraints.FreezePosition;
    }
    void InputPushNew()
    {     
        //Force/Push out 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pushMode = 1;
            Explode();
        }

        //Slowmo
        if (Input.GetKey(KeyCode.LeftShift))
        {
            finalSlowmo = slowmoStrength;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            finalSlowmo = 1;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (cubeManager.testMode == 0)
            {
                sendingBack = true;

            }
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            pushMode = 2;

            Explode();

        }
    }

    void SetColor()
    {
        if (gameObject.GetComponent<Renderer>() != null)
        {
            if (pushMode == 0)
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorOne);

            }
            if (pushMode == 1)
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorTwo);

            }
            if (pushMode == 2)
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorThree);

            }
        }
   

    }
    void Update()
    {
        if (colliding == false && (pushMode == 1 || pushMode == 2))
        {
            //Change Gravity modifier
            /*
            if(gravityChange < maxGravity)
            {
                gravityChange += gravityValue * finalSlowmo * Time.deltaTime;

            }
            else if(gravityChange >= maxGravity)
            {
                gravityChange = maxGravity;
            }
            */

            //Change velocity
            if (moveVelocity.y > -maxGravity)
            {
                moveVelocity.y -= gravityValue * Time.deltaTime * finalSlowmo;

            }
            else if(moveVelocity.y <= -maxGravity)
            {
                moveVelocity.y = -maxGravity;
            }

        }
        else
        {
            gravityChange = 0;
        }
        transform.position += moveVelocity * finalSpeed * Time.deltaTime * finalSlowmo;


        //InputPush();
        InputPushNew();
        SetColor();
        if (sendingBack)
        {
            SendBack();

        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 1.5f))
        {
            Debug.DrawRay(transform.position , new Vector3(0, -1, 0) * hit.distance, Color.yellow);
            if (hit.collider.gameObject.CompareTag("terrain"))
            {
                colliding = true;
                //moveVelocity.y = 0;
                moveVelocity *= 0.9f;
                rigid.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                colliding = false;
            }
        }
        else
        {
            colliding = false;

        }
    }


}
