using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomCollider : MonoBehaviour
{  

    IEnumerator ChangeGrounded()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerManager.instance.isGrounded = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("terrain") && other.transform.parent != null && other.transform.parent.gameObject.GetComponent<CheckPointBehavior>() != null)
        {
            if (other.transform.parent.gameObject.GetComponent<CheckPointBehavior>().isStart == false)
            {
                LevelGeneration.instance.MoveCheckpoint();
                AudioManager.instance.checkpoint.Play();               
                other.transform.parent.GetChild(0).gameObject.GetComponent<LightUp>().lightUp = true;
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(ChangeGrounded());       
    }
}
