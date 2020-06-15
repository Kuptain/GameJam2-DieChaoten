using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

    [SerializeField] float speed;
    //[SerializeField] float mouseSensitivity;
    [SerializeField] float jumpForce;
    [SerializeField] float lookUpMax = 45;
    [SerializeField] float lookUpMin = -45;

    float camSmoothingFactor = 1;

    bool isGrounded;


    Rigidbody rigid;
    Camera cam;

    Vector3 lastMousePosition;

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
            rigid.AddForce(transform.up * jumpForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            isGrounded = false;
        }
    }

}
