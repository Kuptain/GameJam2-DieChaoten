using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPoint : MonoBehaviour
{

    public float forceFactor = 5000f;

    List<Rigidbody> rbobjects = new List<Rigidbody>();

    Transform magnetPoint;

    // Start is called before the first frame update
    void Start()
    {
        magnetPoint = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody rbobject in rbobjects)
        {
            rbobject.AddForce((magnetPoint.position - rbobject.position) * (forceFactor * -1) * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnetable"))
        {
            rbobjects.Add(other.GetComponent<Rigidbody>());
        }
    }
}
