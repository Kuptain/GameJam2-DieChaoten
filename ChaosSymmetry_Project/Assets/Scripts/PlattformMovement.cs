using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlattformMovement : MonoBehaviour
{
    public bool startMoving;

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

    // Start is called before the first frame update
    void Start()
    {
        cubeManager = CubeManager.instance.GetComponent<CubeManager>();

        rigid = gameObject.GetComponent<Rigidbody>();
        pushForce = CubeManager.instance.pushForce;
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
