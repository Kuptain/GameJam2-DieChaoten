using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructuresManager : MonoBehaviour
{
    [SerializeField] float speed;
    List<GameObject> allStructureParts;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        allStructureParts = new List<GameObject>(); 
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("structurePart"))
        {
            allStructureParts.Add(go);
            startPosition = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
            Quaternion startRotation = go.transform.rotation;

            //Debug.Log(startPosition);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        OnTheWayToHeaven();

        if (Input.GetKey(KeyCode.O))
        {
            BackToOrder();
        }
    }

    void OnTheWayToHeaven()
    {
        /*if (Input.GetKey(KeyCode.Space))
        {
            foreach (GameObject go in allStructureParts)
            {
                go.transform.Translate(Vector3.up * Time.deltaTime * Random.Range(1, 3), Space.World);
                go.transform.Rotate(Vector3.right * Time.deltaTime * Random.Range(1, 10));
            }
        }*/

        foreach (GameObject go in allStructureParts)
        {
            go.transform.Translate(Vector3.up * Time.deltaTime * go.transform.position.y*speed, Space.World);
            go.transform.Rotate(Vector3.right * Time.deltaTime * Random.Range(10, 30));
        }

    }

    void BackToOrder()
    {
        foreach (GameObject go in allStructureParts)
        {
            go.transform.Translate(startPosition * Time.deltaTime * 15, Space.World);
            //go.transform.Rotate(Vector3.right * Time.deltaTime * Random.Range(10, 30));
        }
    }
}
