using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterManager : MonoBehaviour
{
    List<GameObject> clusterParts1;
    List<GameObject> clusterParts2;
    List<GameObject> clusterParts3;

    [SerializeField] float speed;

    private Vector3 startPosition;
    
    SelectCluster selectClusterScript;

    // Start is called before the first frame update
    void Start()
    {
        clusterParts1 = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ClusterParts1"))
        {
            clusterParts1.Add(go);
            startPosition = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
            Quaternion startRotation = go.transform.rotation;
        }

        clusterParts2 = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ClusterParts2"))
        {
            clusterParts2.Add(go);
            startPosition = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
            Quaternion startRotation = go.transform.rotation;
        }

        clusterParts3 = new List<GameObject>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ClusterParts3"))
        {
            clusterParts3.Add(go);
            startPosition = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
            Quaternion startRotation = go.transform.rotation;
        }


        selectClusterScript = this.GetComponent<SelectCluster>();

    }

    // Update is called once per frame
    void Update()
    {
        //OnTheWayToHeaven();

        if (Input.GetKey(KeyCode.O))
        {
            BackToOrder();
        }
    }

    public void OnTheWayToHeaven()
    {
        if (Input.GetMouseButton(0))
        {
            if (selectClusterScript.hit.transform.tag == "ClusterParts1")
            {
                foreach (GameObject go in clusterParts1)
                {
                    go.transform.Translate(Vector3.up * Time.deltaTime * go.transform.position.y * speed, Space.World);
                    go.transform.Rotate(Vector3.right * Time.deltaTime * Random.Range(10, 30));
                }
            }

            if (selectClusterScript.hit.transform.tag == "ClusterParts2")
            {
                foreach (GameObject go in clusterParts2)
                {
                    go.transform.Translate(Vector3.up * Time.deltaTime * go.transform.position.y * speed, Space.World);
                    go.transform.Rotate(Vector3.right * Time.deltaTime * Random.Range(10, 30));
                }
            }

            if (selectClusterScript.hit.transform.tag == "ClusterParts3")
            {
                foreach (GameObject go in clusterParts3)
                {
                    go.transform.Translate(Vector3.up * Time.deltaTime * go.transform.position.y * speed, Space.World);
                    go.transform.Rotate(Vector3.right * Time.deltaTime * Random.Range(10, 30));
                }
            }
        }
        

    }

    void BackToOrder()
    {
        /*foreach (GameObject go in allStructureParts)
        {
            go.transform.Translate(startPosition * Time.deltaTime * 15, Space.World);
            //go.transform.Rotate(Vector3.right * Time.deltaTime * Random.Range(10, 30));
        }*/
    }
}
