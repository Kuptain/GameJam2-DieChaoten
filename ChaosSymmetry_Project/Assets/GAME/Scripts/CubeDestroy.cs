using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour
{
    Color colorOne;
    Color colorTwo;
    Color colorThree;


    public Color colorHover, colorHoverExploded;
    float speed;
    float slowmoValue;
    float gravityValue; //The value of the gravity
    float maxGravity; //gravityChange can not be higher than this
    float sendBackManual;
    float sendBackAuto;
    float returnDelay;

    [HideInInspector] public int pushMode;
    public bool sendingBack, freezeThisCluster, bubbleFreeze, notsendingback;

    CubeManager cm;
    Rigidbody rigid;
    Material baseMat;

    float finalSpeed;
    float gravityAutoAdjust = 1;

    bool colliding;
    bool returnTimer;
    bool doOnce;
    public bool isHovered;

    Vector3 startPosition;
    public Vector3 moveVelocity;
    Quaternion startRotation;



    void Start()
    {
        //baseMat = GetComponent<MeshRenderer>().material;
        
        //Copy from Cubemanager//
        {
            cm = CubeManager.instance.GetComponent<CubeManager>();

            speed = cm.speed;
            finalSpeed = speed;

            slowmoValue = cm.slowmoValue;
            gravityValue = cm.gravityValue;
            maxGravity = cm.maxGravity;
            sendBackManual = cm.sendBackManual;
            sendBackAuto = cm.sendBackAuto;
            //pushForce = cubeManager.pushForce;
            returnDelay = cm.returnDelay;

            colorOne = cm.color1;
            colorTwo = cm.color2;
            colorThree = cm.color3;
            colorHover = cm.colorHover;
            colorHoverExploded = cm.colorHoverExploded;
        }

        //Start Setup
        {
            rigid = gameObject.GetComponent<Rigidbody>();
            startPosition = transform.position;
            startRotation = transform.rotation;

            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
            StartCoroutine(Force());
            //TriggerRandomColor();
        }

        gravityAutoAdjust = 1;
    }

    void TriggerRandomColor()
    {
        //Set Random color (auskommentiert)
        {

            if (gameObject.GetComponent<Renderer>() != null)
            {
                int randomNumber = Random.Range(1, 4);
                if (randomNumber == 1)
                {
                    colorOne = cm.color1;
                    //gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorOne);
                }
                if (randomNumber == 2)
                {
                    colorOne = cm.color2;
                    //gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorOne);

                }
                if (randomNumber == 3)
                {
                    colorOne = cm.color3;
                    //gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorOne);

                }
            }

        }
    }
    private void FixedUpdate()
    {
        //Send Cubes back to the roots
        {
            if (cm.testMode == 1)
            {
                sendingBack = true;
            }

            if (sendingBack)
            {
                SendBack();
            }
        }


        //Add own velocity and gravity - Push Cubes away
        {
            moveVelocity *= 0.99f;

            if (colliding == false && (pushMode == 1 || pushMode == 2) && freezeThisCluster == false && bubbleFreeze == false)
            {

                //Change velocity
                if (moveVelocity.y > -maxGravity)
                {
                    moveVelocity.y -= gravityValue * Time.deltaTime * cm.currentSlowmo * gravityAutoAdjust;

                }
                else if (moveVelocity.y <= -maxGravity)
                {
                    moveVelocity.y = -maxGravity * gravityAutoAdjust;
                }

            }

            transform.position += moveVelocity * finalSpeed * Time.deltaTime * cm.currentSlowmo;
        }
    }

    void Update()
    {
        InputPushNew();
        SetColor();
        StartCoroutine(ChangeHoveredState());

        // Freeze Plattform player is standing on
        if (freezeThisCluster || bubbleFreeze)
        {
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
        }
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        rigid.constraints = RigidbodyConstraints.FreezePosition;

        if (transform.childCount > 0 && cm.clusterHasShader)
        {
            if (freezeThisCluster)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(1).gameObject.SetActive(false);
            }
        }       

        //Return Cooldown
        if (returnTimer == true)
        {
            ReturnTimer();
        }


        /*
        //Check ground below for Collision/Raycast
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 1.5f))
            {
                Debug.DrawRay(transform.position, new Vector3(0, -1, 0) * hit.distance, Color.yellow);
                if (hit.collider.gameObject.CompareTag("terrain") && hit.collider.gameObject.GetComponent<CubeDestroy>() == null)
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
        */    
                     
    }

    //Intervall Push mode, every x second makes the cubes push away again
    IEnumerator Force()
    {
        while (true )
        {
          
            yield return new WaitForSeconds(Random.Range(-0.75f, 0.75f));

            if (cm.currentSlowmo == 1)
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
        transform.parent.parent.GetComponent<OrbitPoint>().rotateElements = true;

        if (freezeThisCluster == false && bubbleFreeze == false)
        {
            colliding = false;
            rigid.constraints = RigidbodyConstraints.None;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            returnTimer = true;

            if (transform.parent.GetComponent<OrbitPoint>() != null)
            {
                transform.parent.GetComponent<OrbitPoint>().RandomizeRotation();

            }

            moveVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
            if (cm.testMode == 0)
            {
                sendingBack = false;

            }
        }       
    }

    
    //Return cubes after this timer runs out
    float timer = 0;
    void ReturnTimer()
    {    

        if(timer < returnDelay)
        {
            timer += Time.deltaTime * cm.currentSlowmo;
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
      
        if(freezeThisCluster == false && bubbleFreeze == false)
        {
            pushMode = 0;
            transform.position = Vector3.Slerp(transform.position, startPosition, sendBackManual * cm.currentSlowmo);
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, sendBackManual * cm.currentSlowmo);
            moveVelocity = new Vector3(0, 0, 0);

            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
            //notsendingback = false;
        }
        else if(freezeThisCluster == true || bubbleFreeze == true)
        {
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
            moveVelocity = Vector3.zero;
            //notsendingback = true;
        }

        if(doOnce == false)
        {
            StartCoroutine(SendingBackFalseTimer());
            doOnce = true; 
        }

        /* if (cubeManager.testMode == 1)
         {
             transform.position = Vector3.Slerp(transform.position, startPosition, sendBackAuto * cm.currentSlowmo);
             transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, 0.002f);
         }*/
    }

    IEnumerator SendingBackFalseTimer()
    {

        yield return new WaitForSeconds(3f);
        sendingBack = false;
        transform.parent.parent.GetComponent<OrbitPoint>().rotateElements = false;
        doOnce = false;
    }


    void InputPushNew()
    {         
        
        //Slowmotion
        {

            if (cm.slowMode)
            {
                cm.currentSlowmo = cm.slowmoValue;
            }
            else if (cm.slowMode == false)
            {
                cm.currentSlowmo = 1;

            }
        }

        //Control all cubes
        /*{
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
                if (cm.testMode == 0)
                {
                    sendingBack = true;
                }
            }

          
        }*/

  
    }
    IEnumerator ChangeHoveredState()
    {
       
        yield return new WaitForEndOfFrame();
        isHovered = false;
    }

    //Color Management
    void SetColor()
    {
        if (isHovered == false && cm.clusterHasShader)
        {
            transform.GetChild(0).gameObject.SetActive(false);

        }
   

    }


}
