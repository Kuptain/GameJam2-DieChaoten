using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    [SerializeField] float speed;
    //[SerializeField] float mouseSensitivity;
    [SerializeField] float jumpForce;
    [SerializeField] float floatForce;

    PowerUpManager powerUp;

    float camSmoothingFactor = 1;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    bool isOnCube;

    Rigidbody rigid;
    Camera cam;
    PlayerManager pm;
    CamCheckSideCollision camRightCollCheck;
    CamCheckLeftCollision camLeftCollCheck;

    Vector3 lastMousePosition;
    //public List<GameObject> currentPlatforms = new List<GameObject>();

    private Quaternion camRotation, savedCamRot;

    void Start()
    {
        pm = PlayerManager.instance.GetComponent<PlayerManager>();
        cam = Camera.main;
        camRightCollCheck = cam.transform.parent.GetChild(1).GetComponent<CamCheckSideCollision>(); 
        camLeftCollCheck = cam.transform.parent.GetChild(2).GetComponent<CamCheckLeftCollision>();
        rigid = GetComponent<Rigidbody>();
        Cursor.visible = false;

        powerUp = PowerUpManager.instance;
        
    }

    void Update()
    {
        RotatePlayer();
    }
    private void FixedUpdate()
    {
        Jump();
        Floating();
        JumpSmoothing();
        Move();
    }

    private void Move()
    {
        rigid.MovePosition(transform.position + (transform.forward * (Input.GetAxis("Vertical") * speed * Time.deltaTime) + transform.right * (Input.GetAxis("Horizontal") * speed * Time.deltaTime)));
    }

    private void RotatePlayer()
    {
        savedCamRot.y = camRotation.y;
        camRotation.y += Input.GetAxis("Mouse X") * camSmoothingFactor;

        if (camRightCollCheck.collidingToRight && Input.GetAxis("Mouse X") > 0)
        {
            camRotation.y = savedCamRot.y;
        }
        else if (camLeftCollCheck.collidingToLeft && Input.GetAxis("Mouse X") < 0)
        {
            camRotation.y = savedCamRot.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.x, camRotation.y, transform.rotation.z);        
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && pm.isGrounded)
        {
            pm.isGrounded = false;
            rigid.velocity = new Vector3(0, 0, 0);
            rigid.AddForce(Vector3.up * jumpForce * powerUp.higherJumpFactor);
        }
    }

    void Floating()
    {
        if (Input.GetKey(KeyCode.Space) && pm.isGrounded == false && pm.floatFuel > 0)
        {
            if (rigid.velocity.y < 0)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y * 0.75f, rigid.velocity.z);
            }

            if (true)
            {
                rigid.velocity = rigid.velocity + Vector3.up * floatForce;
                pm.floatFuel -= 1;
            }
        }
    }

    void JumpSmoothing()
    {
        if (rigid.velocity.y < 0)
        {
            rigid.velocity += Vector3.up * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigid.velocity.y > 0)
        {
            rigid.velocity += Vector3.up * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            if (rigid.velocity.y <= 0)
            {
                pm.isGrounded = true;
                pm.floatFuel = pm.maxFloatFuel;

            }

        }
    }
    */



    IEnumerator Defreeze(GameObject cube)
    {
        yield return new WaitForSeconds(0.15f);
        if(pm.isGrounded == false)
        {
            cube.GetComponent<CubeDestroy>().freezeThisCluster = false;

        }

    }

    
    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("terrain") && other.transform.parent.gameObject.GetComponent<CheckPointBehavior>() != null)
        {
            if (other.transform.parent.gameObject.GetComponent<CheckPointBehavior>().isStart == false)
            {
                LevelGeneration.instance.MoveCheckpoint();
                Debug.Log("Move Checkpoint");
            }

        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        //StartCoroutine(ChangeGrounded());

        /*if (other.CompareTag("terrain"))
        {
            if(other.gameObject.GetComponent<CubeDestroy>() != null)
            {
                if (other.name != "Ground" && currentPlatforms != null && other.gameObject.GetComponent<CubeDestroy>().freezeThis == true)
                {
                    foreach (GameObject platform in currentPlatforms)
                    {
                        if (other.gameObject == platform.gameObject)
                        {
                            platform.GetComponent<CubeDestroy>().freezeThis = false;
                            currentPlatforms.Remove(platform);
                        }
                    }
                }
            }
        }*/

    }


    IEnumerator ChangeGrounded()
    {
        yield return new WaitForSeconds(0.5f);
        pm.isGrounded = false;

    }

}
