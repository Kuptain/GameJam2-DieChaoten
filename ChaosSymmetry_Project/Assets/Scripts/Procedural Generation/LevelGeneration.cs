using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] GameObject clusterObj;
    public GameObject[] checkPoints;
    public float clusterAmountCalculator = 5;
    public float checkPointDistance = 50;
    public float difficulty = 0; //subtracts clusters

    [HideInInspector] public GameObject checkPointOne;
    [HideInInspector] public GameObject checkPointTwo;
    
    float clusterDistance;
    float clusterAmount;

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


        }
        else if (checkPoints[1].GetComponent<CheckPointBehavior>().isStart)
        {
            checkPointOne = checkPoints[1];
            checkPointTwo = checkPoints[0];


        }
        else
        {
            Debug.LogError("No Checkpoint");
        }

        //Move second checkpoint up by the variable "checkPointDistance" and give it X and Z variation
        float minVariationZX = 25f;
        float maxVariationZX = 55f;
        float VariationX = 0;
        float VariationZ = 0;

        //Choose X variation
        int choose = Random.Range(0,2); //choose 0 or 1
        if (choose == 0)
        {
            VariationX = Random.Range(minVariationZX, maxVariationZX);
        }
        if (choose == 1)
        {
            VariationX = Random.Range(-minVariationZX, -maxVariationZX);

        }
        
        //Choose Z variation
        choose = Random.Range(0, 2); //choose 0 or 1
        if (choose == 0)
        {
            VariationZ = Random.Range(minVariationZX, maxVariationZX);
        }
        if (choose == 1)
        {
            VariationZ = Random.Range(-minVariationZX, -maxVariationZX);

        }

        checkPointTwo.transform.position = new Vector3(checkPointOne.transform.position.x + VariationX,
                                                       checkPointOne.transform.position.y + checkPointDistance,
                                                       checkPointOne.transform.position.z + VariationZ);

        clusterAmount = Mathf.RoundToInt( checkPointDistance / clusterAmountCalculator - difficulty );
        clusterDistance = checkPointDistance / clusterAmount;


        if (checkPointOne != null && checkPointTwo != null)
        {
            TriggerGeneration();
        }

        //checkPointOne.isStart = true;
        //checkPointOne.canBuild = true;

    }

    void Update()
    {
      
    }
     public void TriggerGeneration()
    {
        GenerateClusters(checkPointOne.transform.position, checkPointTwo.transform.position);

    }

    public void MoveCheckpoint()
    {     
        
        GameObject tempCheckPoint = checkPointOne;
        checkPointOne = checkPointTwo;
        checkPointTwo = tempCheckPoint;
        checkPointOne.GetComponent<CheckPointBehavior>().isStart = true;
        checkPointTwo.GetComponent<CheckPointBehavior>().isStart = false;

        checkPointOne.transform.position = new Vector3(checkPointOne.transform.position.x,
                                                       clusterDistance / clusterAmount,
                                                       checkPointOne.transform.position.z);

    }

    public void GenerateClusters(Vector3 startPos, Vector3 endPos)
    {
        Vector3 spawnPos = startPos - new Vector3(0, 5, 0);
        randomX = spawnPos.x;
        randomZ = spawnPos.z;
        int currentCluster = -1; //-1 or 0  Random.Range(-1, 1);


        for (int i = 0; i < clusterAmount; i++)
        {
            float maxVariationZX = 5f;
            randomX = Random.Range(-maxVariationZX, maxVariationZX) + clusterDistance * currentCluster;
            randomZ = Random.Range(-maxVariationZX, maxVariationZX) + clusterDistance * currentCluster;

            spawnPos.y += clusterDistance;    

            if (endPos.x > startPos.x)
            {
                spawnPos.x += randomX;
            }
            else
            {
                spawnPos.x -= randomX;

            }
            if (endPos.z > startPos.z)
            {
                spawnPos.z += randomZ;
            }
            else
            {
                spawnPos.z -= randomZ;

            }
         
            Instantiate(clusterObj, spawnPos, Quaternion.identity);
            currentCluster += 1;
            
        }
    }
}
