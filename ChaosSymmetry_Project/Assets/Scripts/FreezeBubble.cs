using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBubble : MonoBehaviour
{
    public List<GameObject> currentPlatforms = new List<GameObject>();

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
        if (other.CompareTag("terrain"))
        {
            if (other.name != "Ground")
            {
                currentPlatforms.Add(other.gameObject);
                other.gameObject.GetComponent<CubeDestroy>().bubbleFreeze = true;
                other.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;
                print("inbuble");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            if (other.name != "Ground" && currentPlatforms != null && other.gameObject.GetComponent<CubeDestroy>().bubbleFreeze == true)
            {
                foreach (GameObject platform in currentPlatforms)
                {
                    if (other.gameObject == platform.gameObject)
                    {
                        platform.GetComponent<CubeDestroy>().bubbleFreeze = false;
                        currentPlatforms.Remove(platform);
                        print("buddleDefreeze");
                    }
                }
            }
        }

    }
}
