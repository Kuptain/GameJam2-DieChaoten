using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleAroundPlayer : MonoBehaviour
{
    float maxRotation;
    Vector3 randomRotate;
    CubeManager cm;
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
        maxRotation = cm.orbitMaxRotation;
        RandomizeRotation();

        target = ObjectManager.instance.player.transform;
        transform.position = (transform.position - target.position).normalized * radius + target.position;
        radius = 0.5f;
        doOnce = true;

        speed = 100;
        amount = 0.01f;
        rotationSpeed = 30f;
    }

    // Update is called once per frame
    void Update()
    {      
        if(cm.slowMode == false)
        {
            if(doOnce == false)
            {
                rotationSpeed = 30f;
                //transform.position = shakeStartPos;
                doOnce = true;
            }

            transform.RotateAround(target.position, axis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - target.position).normalized * radius + target.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
            transform.Rotate(randomRotate.x * Time.deltaTime, randomRotate.y * Time.deltaTime, randomRotate.z * Time.deltaTime);
        }
        else
        {
            if(doOnce )
            {
                //shakeStartPos = transform.position;
                rotationSpeed = 400f;
                doOnce = false;
            }
            //tranform.position.x = shakeStartPos.x + Mathf.Sin((Time.time * speed) * amount );

            //tranform.position.y = shakeStartPos.y + Mathf.Sin((Time.time * speed) * amount) ;
            //transform.position = new Vector3(shakeStartPos.x + Mathf.Sin((Time.time * speed) * amount) * 0.02f, shakeStartPos.y + Mathf.Sin((Time.time * speed) * amount) * 0.02f, transform.position.z);
            transform.RotateAround(target.position, axis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - target.position).normalized * radius + target.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);

            /*transform.position = transform.position + (Random.insideUnitSphere * 0.4f) * temp_shake_intensity;
            transform.rotation = new Quaternion(
                transform.rotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                transform.rotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                transform.rotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                transform.rotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f);*/
            //temp_shake_intensity -= shake_decay;

            //randomPos = shakeStartPos + (Random.insideUnitSphere * distance);

            //transform.position = randomPos;

            // gameObject.transform.position.x = Mathf.Sin(Time.deltaTime * speed) * amount;
        }     
    }

    public void RandomizeRotation()
    {
        randomRotate = new Vector3(Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation), Random.Range(-maxRotation, maxRotation));

    }
}
