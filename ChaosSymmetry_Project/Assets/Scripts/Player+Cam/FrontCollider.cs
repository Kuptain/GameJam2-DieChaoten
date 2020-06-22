using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCollider : MonoBehaviour
{
    float timer;
    [SerializeField] float cooldown = 0.1f;
    bool canJump = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Cooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(cooldown);
        canJump = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("terrain"))
        {
            Rigidbody rigid = transform.parent.GetComponent<Rigidbody>();

            //
            if (rigid.velocity.y < 0 && canJump)
            {
            
                //rigid.velocity = new Vector3(0, 0, 0);
                //rigid.AddForce(Vector3.up * PlayerManager.instance.collideJumpForce, ForceMode.Impulse);

                StartCoroutine( Cooldown() );
            }
   
        }
    }
}
