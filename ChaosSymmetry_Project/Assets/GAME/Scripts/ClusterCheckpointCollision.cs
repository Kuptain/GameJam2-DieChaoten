using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterCheckpointCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<CheckPointBehavior>() != null && other.gameObject.GetComponent<CheckPointBehavior>().isStart == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(LevelGeneration.instance.checkPointOne.transform.position.x,
                                                                                        transform.position.y,
                                                                                        LevelGeneration.instance.checkPointOne.transform.position.z), 0.01f);
        }

       

    }


 
}
