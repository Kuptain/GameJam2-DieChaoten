using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTerrainCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {

        if (GetComponent<Collider>().GetType() == typeof(SphereCollider) && other.gameObject.CompareTag("terrain") && other.gameObject.GetComponent<_IsTerrain>() != null)
        {
            transform.parent.GetComponentInParent<CameraController>().colliding = true;
            GetComponent<CapsuleCollider>().radius = 5f;
            GetComponent<CapsuleCollider>().height = 15f;


        }

        if (GetComponent<Collider>().GetType() != typeof(CapsuleCollider) && other.gameObject.GetComponent<FreezeBubble>() != null)
        {
            transform.parent.GetComponentInParent<CameraController>().playerCollide = true;

        }

       
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (GetComponent<Collider>().GetType() == typeof(SphereCollider) && other.gameObject.CompareTag("terrain") && other.gameObject.GetComponent<_IsTerrain>() != null)
        {
            transform.parent.GetComponentInParent<CameraController>().colliding = false;
            GetComponent<CapsuleCollider>().radius = 0.5f;
            GetComponent<CapsuleCollider>().height = 7f;

        }
        if (GetComponent<Collider>().GetType() == typeof(CapsuleCollider) && other.gameObject.GetComponent<FreezeBubble>() != null)
        {
            transform.parent.GetComponentInParent<CameraController>().playerCollide = false;

        }
        

        
    }

}
