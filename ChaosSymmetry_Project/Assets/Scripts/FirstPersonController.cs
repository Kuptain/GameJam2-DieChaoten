﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float jumpForce;




    Rigidbody rigid;
    Camera cam;

    Vector3 lastMousePosition;

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
        Vector3 cameraDelta = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);
       // Vector3 cameraDelta = lastMousePosition - Input.mousePosition;

        transform.Rotate(0, cameraDelta.x * Time.deltaTime * mouseSensitivity, 0);
        cam.transform.Rotate( -cameraDelta.y * Time.deltaTime * mouseSensitivity,0,0);


        //   lastMousePosition = Input.mousePosition;
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(transform.up * jumpForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

}
