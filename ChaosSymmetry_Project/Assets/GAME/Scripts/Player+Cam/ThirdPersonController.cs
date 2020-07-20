using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    GameObject character;
    GameObject bodyObject;
    [HideInInspector] public GameObject hornBlue;
    [HideInInspector] public GameObject hornOrange;

    Animator anim;

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



        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Character")
            {
                character = child.gameObject;
                anim = character.GetComponent<Animator>();
            }
            if (child.gameObject.name == "PlayerUpper")
            {
                bodyObject = child.gameObject;
            }

            if (child.gameObject.name == "Character")
            {
                foreach (Transform childChild in child)
                {
                    foreach (Transform childThree in childChild)
                    {
                        if (childThree.gameObject.name == "joint16")
                        {
                            foreach (Transform childFour in childThree)
                            {
                                if (childFour.gameObject.name == "HornBlue")
                                {
                                    hornBlue = childFour.gameObject;
                                }
                                if (childFour.gameObject.name == "HornOrange")
                                {
                                    hornOrange = childFour.gameObject;
                                }
                            }

                        }


                    }
                }
            }


        }


    }

    void Update()
    {
        RotatePlayer();
        //print(Input.GetAxis("Mouse X"));

        if (pm.isRespawning == false)
        {
            if ((rigid.velocity.y < -60 && pm.floatFuel <= 0))
            {
                if (PlayerPrefs.GetInt("gameMode", 0) == 0)
                {
                    if (pm.lives > 1)
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
                        UIManager.instance.overScore.GetComponent<Text>().text = "You reached island " + UIManager.instance.normalScore.ToString() + "!";
                        UIManager.instance.overHighscore.GetComponent<Text>().text = "Your high score is " + PlayerPrefs.GetInt("normalHighScore").ToString() + "!";
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

        if (!pm.isGrounded)
        {
            anim.SetBool("midAir", true);
        }
        else
        {
            anim.SetBool("midAir", false);

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
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);

        }
        float lerpValue = 0.2f;
        //Forwards
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") == 0)
        {
            character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), lerpValue);

        }
        //Forward Right
        if ((Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0))
        {
            character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.Euler(new Vector3(0, 45, 0)), lerpValue);

        }
        //Forward Left
        if ((Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0))
        {
            character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.Euler(new Vector3(0, 315, 0)), lerpValue);

        }

        //Backwards
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") == 0)
        {
            character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.Euler(new Vector3(0, 180, 0)), lerpValue);

        }
        //Backwards Right
        if ((Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0))
        {
            character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.Euler(new Vector3(0, 135, 0)), lerpValue);

        }
        //Backwards Left
        if ((Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0))
        {
            character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.Euler(new Vector3(0, 225, 0)), lerpValue);

        }

        //Right
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") > 0)
        {
            character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.Euler(new Vector3(0, 90, 0)), lerpValue);

        }
        //Left
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") < 0)
        {
            character.transform.localRotation = Quaternion.Lerp(character.transform.localRotation, Quaternion.Euler(new Vector3(0, 270, 0)), lerpValue);

        }

    }

    public void PlaySlowmo()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetTrigger("slowmoTrigger");

        }
    }
    public void PlayLand()
    {
        if (anim != null)
        {
            anim.SetTrigger("land");
            anim.SetBool("jumping", false);
        }

    }
    public void PlayDestroy()
    {
        if (anim != null)
        {
            anim.SetTrigger("cluster");
        }
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
        if (Input.GetKey(KeyCode.Space) && pm.isGrounded && pm.canJump)
        {
            pm.isGrounded = false;
            StartCoroutine(JumpCooldown());

            anim.SetBool("jumping", true);
            rigid.velocity = new Vector3(0, 0, 0);
            rigid.AddForce(Vector3.up * jumpForce * powerUp.higherJumpFactor);
            StartCoroutine(DisableCollider());
            UIManager.instance.jumped = true;
        }
    }

    IEnumerator JumpCooldown()
    {
        pm.canJump = false;
        yield return new WaitForSeconds(0.35f);
        pm.canJump = true;

    }
    IEnumerator DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
        bodyObject.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(0.35f);

        GetComponent<Collider>().enabled = true;
        bodyObject.GetComponent<Collider>().enabled = true;

    }
    void Floating()
    {
        if (Input.GetKey(KeyCode.Space) && pm.isGrounded == false && pm.floatFuel > 0)
        {
            anim.SetBool("floating", true);


            if (rigid.velocity.y < 0)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y * 0.75f, rigid.velocity.z);
            }

            if (true)
            {
                rigid.velocity = rigid.velocity + Vector3.up * floatForce;
                pm.floatFuel -= 1;

                if (TutorialManager.instance.currentHint == "float" && pm.floatFuel < pm.maxFloatFuel * 0.5f)
                {
                    TutorialManager.instance.ChangeType("");
                }
            }
        }
        else
        {
            anim.SetBool("floating", false);


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
        if (pm.isGrounded == false)
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
                if (pm.lives > 1)
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
                    UIManager.instance.overScore.GetComponent<Text>().text = "You reached island " + UIManager.instance.normalScore.ToString() + "!";
                    UIManager.instance.overHighscore.GetComponent<Text>().text = "Your high score is " + PlayerPrefs.GetInt("normalHighScore").ToString() + "!";
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

        if (other.gameObject.GetComponent<_DeathZone>() != null)
        {
            isSafe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<_DeathZone>() != null)
        {
            isSafe = false;
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
