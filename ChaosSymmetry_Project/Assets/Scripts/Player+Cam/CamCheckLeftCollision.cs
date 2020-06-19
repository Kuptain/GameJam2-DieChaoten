using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCheckLeftCollision : MonoBehaviour
{
    public bool collidingToLeft;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Collider>().GetType() == typeof(CapsuleCollider) && other.gameObject.CompareTag("terrain") && other.gameObject.GetComponent<CubeDestroy>() == false)
        {
            collidingToLeft = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (GetComponent<Collider>().GetType() == typeof(CapsuleCollider) && other.gameObject.CompareTag("terrain") && other.gameObject.GetComponent<CubeDestroy>() == false)
            collidingToLeft = false;
    }
}
