using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RubbleAroundPlayer : MonoBehaviour
{
    float maxRotation;
    Vector3 randomRotate;
    CubeManager cm;
    PlayerManager pm;
    public bool exploding;

    public Transform target;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    public float radius = 0.5f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;

    bool doOnce;
    float speed = 10.0f; //how fast it shakes
    float amount = 10f; //how much it shakes
    Vector3 shakeStartPos, randomPos;
    [Range(0f, 2f)]
    public float distance = 0.2f;

    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay = 0.002f;
    public float temp_shake_intensity = .3f;

    // Start is called before the first frame update
    void Start()
    {
        cm = CubeManager.instance;
        pm = PlayerManager.instance;
        radius = 0.5f;

        RandomizeRotation();

        if (SceneManager.GetActiveScene().name == "Character")
        {
            target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
            radius = 0.8f;
        }
        else
        {
            maxRotation = cm.orbitMaxRotation;
            target = ObjectManager.instance.player.transform;

        }

        transform.position = (transform.position - target.position).normalized * radius + target.position;
        doOnce = true;

        speed = 100;
        amount = 0.01f;
        rotationSpeed = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (cm.slowMode)
        {
            radius = 0.9f;
        }
        else
        {
            radius = 0.5f;
        }

        if (SceneManager.GetActiveScene().name == "Character")
        {
            if (doOnce == false)
            {
                rotationSpeed = 30f;
                doOnce = true;

            }

            transform.RotateAround(target.position, axis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - target.position).normalized * radius + target.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
            transform.Rotate(randomRotate.x * Time.deltaTime, randomRotate.y * Time.deltaTime, randomRotate.z * Time.deltaTime);
        }
        else
        {
            if (!pm.isGrounded)
            {
                if (doOnce)
                {
                    rotationSpeed = 400f;
                    doOnce = false;
                }
                transform.RotateAround(target.position, axis, rotationSpeed * Time.deltaTime);
                desiredPosition = (transform.position - target.position).normalized * radius + target.position;
                transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
            }
            else
            {
                if (doOnce == false)
                {
                    rotationSpeed = 30f;
                    doOnce = true;
                }

                transform.RotateAround(target.position, axis, rotationSpeed * Time.deltaTime);
                desiredPosition = (transform.position - target.position).normalized * radius + target.position;
                transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
                transform.Rotate(randomRotate.x * Time.deltaTime, randomRotate.y * Time.deltaTime, randomRotate.z * Time.deltaTime);

            }
        }
    }

    public void RandomizeRotation()
    {
        randomRotate = new Vector3(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation));

    }
}
