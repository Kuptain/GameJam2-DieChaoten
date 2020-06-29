using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [HideInInspector] public bool isSafe;

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
    ResetJump jumpSoundScript;

    Vector3 lastMousePosition;
    //public List<GameObject> currentPlatforms = new List<GameObject>();

    // gamemode 0 is iwth lives, 1 is endless
    private Quaternion camRotation, savedCamRot;

    public GameObject ankor;

    private void Awake()
    {
    }
    void Start()
    {
        pm = PlayerManager.instance.GetComponent<PlayerManager>();
        cam = Camera.main;
        camRightCollCheck = cam.transform.parent.GetChild(1).GetComponent<CamCheckSideCollision>(); 
        camLeftCollCheck = cam.transform.parent.GetChild(2).GetComponent<CamCheckLeftCollision>();
        rigid = GetComponent<Rigidbody>();
        jumpSoundScript = GameObject.Find("JumpCollider").GetComponent<ResetJump>();
        powerUp = PowerUpManager.instance;
        ankor = transform.GetChild(0).gameObject;

    }

    void Update()
    {
        RotatePlayer();
        //print(Input.GetAxis("Mouse X"));

        if (pm.isRespawning == false)
        {
            if ((rigid.velocity.y < -60 && pm.floatFuel <= 0))
            {
                if(PlayerPrefs.GetInt("gameMode", 0) == 0)
                {
                    if(pm.lives > 0)
                    {
                        pm.Respawn();
                        pm.lives -= 1;
                    }
                    else
                    {
                        UIManager.instance.ingameCanvas.SetActive(false);
                        UIManager.instance.pauseCanvas.SetActive(false);
                        ObjectManager.instance.player.GetComponent<ThirdPersonController>().enabled = false;
                        ObjectManager.instance.player.GetComponent<PlayerShoot>().enabled = false;
                        UIManager.instance.gameOverCanvas.SetActive(true);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
                else
                {
                    pm.Respawn();
                }
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumpSoundScript.jumped = true;
        }

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
            //camRotation.y = savedCamRot.y;
        }
        else if (camLeftCollCheck.collidingToLeft && Input.GetAxis("Mouse X") < 0)
        {
            //camRotation.y = savedCamRot.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.x, Camera.main.transform.eulerAngles.y, transform.rotation.z);        
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && pm.isGrounded)
        {
            pm.isGrounded = false;
            rigid.velocity = new Vector3(0, 0, 0);
            rigid.AddForce(Vector3.up * jumpForce * powerUp.higherJumpFactor);
            StartCoroutine(DisableCollider());
        }
    }

    IEnumerator DisableCollider()
    {
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(0.25f);

        GetComponent<Collider>().enabled = true;

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

                if (TutorialManager.instance.currentHint == "float" && pm.floatFuel < pm.maxFloatFuel*0.5f)
                {
                    TutorialManager.instance.ChangeType("");
                }
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<TerrainCollider>() != null && !isSafe)
        {
            isSafe = true;
            if (PlayerPrefs.GetInt("gameMode", 0) == 0)
            {
                if (pm.lives > 0)
                {
                    pm.Respawn();
                    pm.lives -= 1;
                }
                else
                {
                    UIManager.instance.ingameCanvas.SetActive(false);
                    UIManager.instance.pauseCanvas.SetActive(false);
                    ObjectManager.instance.player.GetComponent<ThirdPersonController>().enabled = false;
                    ObjectManager.instance.player.GetComponent<PlayerShoot>().enabled = false;
                    UIManager.instance.gameOverCanvas.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            else
            {
                pm.Respawn();
            }

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
       
        if (other.gameObject.GetComponent<_DeathZone>() != null )
        {
            isSafe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<_DeathZone>() != null)
        {
            isSafe = false ;
        }
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
