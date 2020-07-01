using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Anims : MonoBehaviour
{
    Rigidbody rigid;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            anim.Play("Idle");
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.Play("Walk");
        }
        if (Input.GetKey(KeyCode.S))
        {
            anim.Play("SlowMo");
        }
        if (Input.GetKey(KeyCode.C))
        {
            anim.Play("Cluster");
        }

        if (Input.GetKey(KeyCode.Space) )
        {
            
            //anim.SetBool("jumping", true);
            rigid.velocity = new Vector3(0, 0, 0);
            rigid.AddForce(Vector3.up * 1000f);
            //StartCoroutine(DisableCollider());
        }
    }
}
