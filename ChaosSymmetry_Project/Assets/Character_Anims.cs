using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Anims : MonoBehaviour
{
    // Start is called before the first frame update

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.Play("Idle");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.Play("Walk");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.Play("SlowMo");
        }
    }
}
