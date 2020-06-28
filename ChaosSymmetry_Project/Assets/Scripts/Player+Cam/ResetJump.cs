using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetJump : MonoBehaviour
{
    public bool jumped;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(transform.parent.GetComponent<Rigidbody>().velocity.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (jumped)
        {
            AudioManager.instance.landing.Play();
            jumped = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            if (transform.parent.GetComponent<Rigidbody>().velocity.y <= 0)
            {
                //transform.parent.GetComponent<FirstPersonController>().isGrounded = true;
                PlayerManager.instance.isGrounded = true;
                PlayerManager.instance.floatFuel = PlayerManager.instance.maxFloatFuel;

            }

        }
    }
}
