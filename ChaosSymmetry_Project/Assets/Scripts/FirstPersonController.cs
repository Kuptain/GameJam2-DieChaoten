using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

    [SerializeField] float speed;
    //[SerializeField] float mouseSensitivity;
    [SerializeField] float jumpForce;
    [SerializeField] float lookUpMax = 60;
    [SerializeField] float lookUpMin = -80;

    float camSmoothingFactor = 1;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [SerializeField] bool isGrounded;

    Rigidbody rigid;
    Camera cam;

    Vector3 lastMousePosition;
    public List<GameObject> currentPlatforms = new List<GameObject>();

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
        JumpSmoothing();
        Move();
        RotateCamera();

    }

    private void Move()
    {
        rigid.MovePosition(transform.position + (transform.forward * (Input.GetAxis("Vertical") * speed * Time.deltaTime) + transform.right * (Input.GetAxis("Horizontal") * speed * Time.deltaTime)));
    }

    private void RotateCamera()
    {
        camRotation.x += Input.GetAxis("Mouse Y") * camSmoothingFactor * (-1);
        camRotation.y += Input.GetAxis("Mouse X") * camSmoothingFactor;

        camRotation.x = Mathf.Clamp(camRotation.x, lookUpMin, lookUpMax);

        transform.localRotation = Quaternion.Euler(camRotation.x, camRotation.y, camRotation.z);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigid.AddForce(Vector3.up * jumpForce);
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

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            if (other.name != "Ground")
            {
                currentPlatforms.Add(other.gameObject);
                other.gameObject.GetComponent<CubeDestroy>().freezeThis = true;
                other.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            isGrounded = false;
            if (other.name != "Ground" && currentPlatforms != null)
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
    }

}
