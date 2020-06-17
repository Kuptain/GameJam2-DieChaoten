﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

    [SerializeField] float speed;
    //[SerializeField] float mouseSensitivity;
    [SerializeField] float jumpForce;
    [SerializeField] float floatForce;


    float camSmoothingFactor = 1;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool isGrounded;
    bool isOnCube;

    Rigidbody rigid;
    Camera cam;

    Vector3 lastMousePosition;
    //public List<GameObject> currentPlatforms = new List<GameObject>();

    private Quaternion camRotation;

    void Start()
    {
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    void Update()
    {
        Jump();
        Floating();
        JumpSmoothing();
        Move();
        RotatePlayer();

    }

    private void Move()
    {
        rigid.MovePosition(transform.position + (transform.forward * (Input.GetAxis("Vertical") * speed * Time.deltaTime) + transform.right * (Input.GetAxis("Horizontal") * speed * Time.deltaTime)));
    }

    private void RotatePlayer()
    {
        camRotation.y += Input.GetAxis("Mouse X") * camSmoothingFactor;

        transform.rotation = Quaternion.Euler(transform.rotation.x, camRotation.y, transform.rotation.z);
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            rigid.velocity = new Vector3(0, 0, 0);
            rigid.AddForce(Vector3.up * jumpForce);
        }
    }

    void Floating()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded == false)
        {
            if (rigid.velocity.y < 0)
            {
                rigid.AddForce(Vector3.up * floatForce, ForceMode.Force);
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


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            if (rigid.velocity.y <= 0)
            {
                isGrounded = true;

            }

            /*if (other.name != "Ground")
            {

                currentPlatforms.Add(other.gameObject);
                other.gameObject.GetComponent<CubeDestroy>().freezeThis = true;
                other.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;
                StartCoroutine(Defreeze(other.gameObject));


            }*/
        }
    }


    IEnumerator Defreeze(GameObject cube)
    {
        yield return new WaitForSeconds(0.15f);
        if(isGrounded == false)
        {
            cube.GetComponent<CubeDestroy>().freezeThisCluster = false;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(ChangeGrounded());
        /*if (other.CompareTag("terrain"))
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
        }*/
        
    }
    IEnumerator ChangeGrounded()
    {
        yield return new WaitForSeconds(0.5f);
        isGrounded = false;

    }

}
