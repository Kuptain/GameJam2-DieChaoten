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

    }

    private void OnTriggerEnter(Collider other)
    {
        if (jumped && other.CompareTag("terrain"))
        {
            jumped = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("terrain") && PlayerManager.instance.isGrounded == false)
        {
            if (transform.parent.GetComponent<Rigidbody>().velocity.y <= 0)
            {
                //transform.parent.GetComponent<FirstPersonController>().isGrounded = true;
                AudioManager.instance.landing.Play();

                PlayerManager.instance.isGrounded = true;
                PlayerManager.instance.floatFuel = PlayerManager.instance.maxFloatFuel;
                ObjectManager.instance.player.GetComponent<ThirdPersonController>().PlayLand();

            }

        }
    }
}
