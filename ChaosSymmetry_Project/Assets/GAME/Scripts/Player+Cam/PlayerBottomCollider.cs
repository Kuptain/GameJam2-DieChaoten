using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomCollider : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }
    private void FixedUpdate()
    {
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            if (rigid.velocity.y <= 0)
            {
                PlayerManager.instance.isGrounded = true;
                pm.floatFuel = pm.maxFloatFuel;

            }

        }
    }
    */


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
                Debug.Log("Move Checkpoint");
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(ChangeGrounded());
        /*if (other.CompareTag("terrain"))
        {
            if(other.gameObject.GetComponent<CubeDestroy>() != null)
            {
                if (other.name != "Ground" && currentPlatforms != null && other.gameObject.GetComponent<CubeDestroy>().freezeThis == true)
                {
                    foreach (GameObject platform in currentPlatforms)
                    {
                        if (other.gameObject == platform.gameObject)
                        {
                            platform.GetComponent<CubeDestroy>().freezeThis = false;
                            currentPlatforms.Remove(platform);
                        }
                    }
                }
            }
        }*/

    }
}
