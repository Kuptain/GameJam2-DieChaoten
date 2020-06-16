using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour
{
    [SerializeField] Color colorOne;
    [SerializeField] Color colorTwo;
    [SerializeField] Color colorThree;
    public Color colorHover;
    float speed;
    float slowmoValue;
    float gravityValue; //The value of the gravity
    float maxGravity; //gravityChange can not be higher than this
    float sendBackManual;
    float sendBackAuto;
    float returnDelay;

    [HideInInspector] public int pushMode;
    [HideInInspector] public bool sendingBack, freezeThis;

    CubeManager cubeManager;
    Rigidbody rigid;
    GameObject orbitObj;


    float finalSpeed;
    public float currentSlowmo = 1;
    float gravityAutoAdjust = 1;
    float gravityChange; //This will be changed and added to the object

    bool colliding;
    bool returnTimer;
    bool isFrozen;

    Vector3 startPosition;
    public Vector3 moveVelocity;
    Quaternion startRotation;



    void Start()
    {
        

        //Set Random color (auskommentiert)
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
        }
        
        
        //Copy from Cubemanager//
        {
            cubeManager = CubeManager.instance.GetComponent<CubeManager>();

            speed = cubeManager.speed;
            finalSpeed = speed;

            slowmoValue = cubeManager.slowmoValue;
            gravityValue = cubeManager.gravityValue;
            maxGravity = cubeManager.maxGravity;
            sendBackManual = cubeManager.sendBackManual;
            sendBackAuto = cubeManager.sendBackAuto;
            //pushForce = cubeManager.pushForce;
            returnDelay = cubeManager.returnDelay;
        }

        //Start Setup
        {
            rigid = gameObject.GetComponent<Rigidbody>();
            startPosition = transform.position;
            startRotation = transform.rotation;
            

            StartCoroutine(Force());
        }

        gravityAutoAdjust = 1;

    }

    void Update()
    {
        InputPushNew();
        SetColor();

        // Freeze Plattform player is standing on
        if (freezeThis)
        {
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
        }

        //Return Cooldown
        if (returnTimer == true)
        {
            ReturnTimer();
        }

        //Add own velocity and gravity - Push Cubes away
        {
            moveVelocity *= 0.99f;

            if (colliding == false && (pushMode == 1 || pushMode == 2) && freezeThis == false)
            {

                //Change velocity
                if (moveVelocity.y > -maxGravity)
                {
                    moveVelocity.y -= gravityValue * Time.deltaTime * currentSlowmo * gravityAutoAdjust;

                }
                else if (moveVelocity.y <= -maxGravity)
                {
                    moveVelocity.y = -maxGravity * gravityAutoAdjust;
                }

            }
            else
            {
                gravityChange = 0;
            }
            transform.position += moveVelocity * finalSpeed * Time.deltaTime * currentSlowmo;

        }


        //Check ground below for Collision/Raycast
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 1.5f))
            {
                Debug.DrawRay(transform.position, new Vector3(0, -1, 0) * hit.distance, Color.yellow);
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


        //Send Cubes back to the roots
        {
            if (cubeManager.testMode == 1)
            {
                sendingBack = true;
            }

            if (sendingBack)
            {
                SendBack();
            }
        }
                     
    }

    //Intervall Push mode, every x second makes the cubes push away again
    IEnumerator Force()
    {
        while (true )
        {
          
            yield return new WaitForSeconds(Random.Range(-0.75f, 0.75f));

            if (currentSlowmo == 1)
            {
                if (pushMode == 2)
                {
                    Explode();

                }
            }

        }
      
    }


    //Push out effect/Explode
    public void Explode()
    {
        colliding = false;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        returnTimer = true;

        moveVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
        if( cubeManager.testMode == 0)
        {
            sendingBack = false;

        }

    }

    
    //Return cubes after this timer runs out
    float timer = 0;
    void ReturnTimer()
    {    

        if(timer < returnDelay)
        {
            timer += Time.deltaTime * currentSlowmo;
        }
        else
        {
            sendingBack = true;
            timer = 0;
            returnTimer = false;
        }
    }

    //Send cubes back to the roots while this is running
    public void SendBack()
    {
      
        if(freezeThis == false)
        {
            pushMode = 0;
            transform.position = Vector3.Slerp(transform.position, startPosition, sendBackManual * currentSlowmo);
            //transform.rotation = startRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, 0.02f);
            moveVelocity = new Vector3(0, 0, 0);

            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;

        }

       /* if (cubeManager.testMode == 1)
        {
            transform.position = Vector3.Slerp(transform.position, startPosition, sendBackAuto * currentSlowmo);
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, 0.002f);


        }*/


    
    }

    void InputPushNew()
    {     
      

        //Slowmotion
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSlowmo = slowmoValue;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currentSlowmo = 1;
            }
        }

        //Control all cubes
        {
            //Force/Push out 
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                pushMode = 1;
                Explode();
            }

            //Intervall Force/Push
            if (Input.GetKey(KeyCode.Alpha2))
            {
                pushMode = 2;
                Explode();
            }

            //Send all cubes back to the roots
            if (Input.GetKey(KeyCode.Alpha3))
            {
                if (cubeManager.testMode == 0)
                {
                    sendingBack = true;
                }
            }

          
        }

  
    }

    //Color Management
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


}
