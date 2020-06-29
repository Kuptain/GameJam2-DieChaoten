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

    void CheckParentScript(GameObject obj, string mode)
    {
        if (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            
            if (mode == "freeze")
            {
                if (obj.GetComponent<CubeDestroy>())
                {
                    FreezeCollider(obj, "freeze");
                }
                else
                {
                    CheckParentScript(obj, "freeze");
                }
            }
         
            else if (mode == "defreeze")
            {
                if (obj.GetComponent<CubeDestroy>() != null)
                {
                    FreezeCollider(obj, "defreeze");
                }
                else
                {
                    CheckParentScript(obj, "defreeze");

                }

            }
        }
    }

    void FreezeCollider(GameObject obj, string mode)
    {
        if (mode == "freeze")
        {
            currentPlatforms.Add(obj);
            obj.GetComponent<CubeDestroy>().bubbleFreeze = true;
            obj.GetComponent<CubeDestroy>().moveVelocity = Vector3.zero;

        }
        else if (mode == "defreeze")
        {
            if (obj.gameObject.GetComponent<CubeDestroy>().bubbleFreeze == true)
            {
                foreach (GameObject platform in currentPlatforms.ToList())
                {
                    if (obj.gameObject == platform.gameObject)
                    {
                        obj.GetComponent<CubeDestroy>().bubbleFreeze = false;
                        currentPlatforms.Remove(platform);
                    }
                }
               
            }

        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            if(other.gameObject.GetComponent<CubeDestroy>() != null)
            {
                FreezeCollider(other.gameObject, "freeze");
            }
            else
            {
                CheckParentScript(other.gameObject, "freeze");
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("terrain") )
        {
            if (currentPlatforms != null )
            {
                if (other.gameObject.GetComponent<CubeDestroy>() != null)
                {
                    FreezeCollider(other.gameObject, "defreeze");

                           
                }
                else
                {
                    CheckParentScript(other.gameObject, "defreeze");

                  
                }
            }

            //Power Up Spawn Cube
            if (other.gameObject.GetComponent<spawnedCubePowerUp>() != null)
            {
                other.gameObject.GetComponent<spawnedCubePowerUp>().StartCoroutine("FadeOut");
            }
        }

      
    }
}
