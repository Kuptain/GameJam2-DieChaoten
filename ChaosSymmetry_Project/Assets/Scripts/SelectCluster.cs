using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCluster : MonoBehaviour
{
    float range;
    ClusterManager clusterManager;

    // Start is called before the first frame update
    void Start()
    {
        range = 100f;
        clusterManager = GameObject.Find("Manager").GetComponent<ClusterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Select();
      
    }

    void Select()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            clusterManager.OnTheWayToHeaven();
        }
    }
}
