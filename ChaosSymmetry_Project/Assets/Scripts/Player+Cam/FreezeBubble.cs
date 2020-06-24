using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
            if (other.gameObject.GetComponent<spawnedCubePowerUp>() != null)
            {
                //other.gameObject.GetComponent<spawnedCubePowerUp>().StartCoroutine("FadeOut");
            }
        }
      
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("terrain") && other.gameObject.GetComponent<CubeDestroy>() != null)
        {
            if (other.name != "Ground" )
            {
                currentPlatforms.Add(other.gameObject);
                other.gameObject.GetComponent<CubeDestroy>().bubbleFreeze = true;
                other.gameObject.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;
                //print("inbuble");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("terrain") )
        {
            if (other.name != "Ground" && currentPlatforms != null && other.gameObject.GetComponent<CubeDestroy>() != null && other.gameObject.GetComponent<CubeDestroy>().bubbleFreeze == true)
            {
                foreach (GameObject platform in currentPlatforms.ToList())
                {
                    if (other.gameObject == platform.gameObject)
                    {
                        
                        platform.GetComponent<CubeDestroy>().bubbleFreeze = false;
                        currentPlatforms.Remove(platform);
                        //print("buddleDefreeze");
                        
                    }
                }
            }
            if (other.gameObject.GetComponent<spawnedCubePowerUp>() != null)
            {
                other.gameObject.GetComponent<spawnedCubePowerUp>().StartCoroutine("FadeOut");
            }
        }

      
    }
}
