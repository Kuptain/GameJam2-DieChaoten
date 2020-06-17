using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] GameObject clusterObj;
    public GameObject[] checkPoints;
    public float newClusterAmount = 5;
    public float clusterDistance;
    CheckPointBehavior checkPointOneScr;
    CheckPointBehavior checkPointTwoScr;
    [HideInInspector] public GameObject checkPointOne;
    [HideInInspector] public GameObject checkPointTwo;

    float randomX;
    float randomZ;

    public static LevelGeneration instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("checkpoint");

        if (checkPoints[0].GetComponent<CheckPointBehavior>().isStart)
        {
            checkPointOne = checkPoints[0];
            checkPointTwo = checkPoints[1];

            checkPointOneScr = checkPointOne.GetComponent<CheckPointBehavior>();
            checkPointTwoScr = checkPointTwo.GetComponent<CheckPointBehavior>();

        }
        else if (checkPoints[1].GetComponent<CheckPointBehavior>().isStart)
        {
            checkPointOne = checkPoints[1];
            checkPointTwo = checkPoints[0];

            checkPointOneScr = checkPointOne.GetComponent<CheckPointBehavior>();
            checkPointTwoScr = checkPointTwo.GetComponent<CheckPointBehavior>();

        }
        else
        {
            Debug.LogError("No Checkpoint");
        }

        if (checkPointOne != null && checkPointTwo != null)
        {
            TriggerGeneration();
        }

        //checkPointOne.isStart = true;
        //checkPointOne.canBuild = true;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GenerateClusters(new Vector3(0,0,0), new Vector3(0, 50, 0));
        }
    }
     public void TriggerGeneration()
    {
        GenerateClusters(checkPointOne.transform.position, checkPointTwo.transform.position);

    }
    public void GenerateClusters(Vector3 startPos, Vector3 endPos)
    {
        Vector3 spawnPos = startPos - new Vector3(0, 1, 0);
        clusterDistance = (endPos.y - startPos.y) / newClusterAmount;
        randomX = spawnPos.x;
        randomZ = spawnPos.z;
        int currentCluster = -1; //-1 or 0  Random.Range(-1, 1);


        for (int i = 0; i < newClusterAmount; i++)
        {
            float maxVariationZX = 5f;
            randomX = Random.Range(-maxVariationZX, maxVariationZX) + ((endPos.x - startPos.x) / newClusterAmount) * currentCluster;

            randomZ = Random.Range(-maxVariationZX, maxVariationZX) + ((endPos.z - startPos.z) / newClusterAmount) * currentCluster;

            spawnPos.y += clusterDistance;    
            spawnPos.x += randomX;
            spawnPos.z -= randomX;
         
            Instantiate(clusterObj, spawnPos, Quaternion.identity);
            currentCluster += 1;
            
        }
    }
}
