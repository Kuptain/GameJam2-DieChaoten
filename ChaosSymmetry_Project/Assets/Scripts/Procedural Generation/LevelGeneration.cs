using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] GameObject clusterObj;
    [HideInInspector] public GameObject checkPointOne;
    [HideInInspector] public GameObject checkPointTwo;
    [HideInInspector] public GameObject[] checkPoints;





    //---------------------------------------------------\\
    [Header("Generation values")]

        [Tooltip("Y Distance between checkpoints")]
        public float checkPointDistance = 50;

        [Tooltip("Subtract this from the cluster amount")]
        public float difficulty = 0; //subtracts clusters

        [Tooltip("Every x units, a new cluster is spawned")]
        [Range(1, 100)] public float clusterAmountCalc;

        [Tooltip("Variation in X and Z of spawned cluster")]
        public float maxVariationZX = 5f;



    //---------------------------------------------------\\
    [Header("Value change after reaching checkpoint")]

        public float incrCheckPointDistance = 5;
        public float incrClusterAmountCalc = 1;
        public float maxClusterAmountCalcDiff = 5;
        public float incrDifficulty = 0.5f;

    //---------------------------------------------------\\


    float clusterDistance;
    float clusterAmount;

    float randomX;
    float randomZ;

    //Singleton
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
                      
  
        checkPointTwo.transform.position = new Vector3(checkPointOne.transform.position.x + GenerateVariation(25f, 55f),
                                                       checkPointOne.transform.position.y + checkPointDistance,
                                                       checkPointOne.transform.position.z + GenerateVariation(25f, 55f));

  
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
  
    float GenerateVariation(float minVariation, float maxVariation)
    {
        //Choose X variation
        float variation = 0;   

        int choose = Random.Range(0, 2); //choose 0 or 1
        if (choose == 0)
        {
            variation = Random.Range(minVariation, maxVariation);
        }
        if (choose == 1)
        {
            variation = Random.Range(-minVariation, -maxVariation);

        }

        return variation;
    }

    public void MoveCheckpoint()
    {
        
        //Increase difficulty
        {
            difficulty += incrDifficulty;
            checkPointDistance += incrCheckPointDistance;
            clusterAmountCalc += incrClusterAmountCalc;
        } 


        //Check if inside bounds
        {
            if (clusterAmountCalc > (clusterAmountCalc + maxClusterAmountCalcDiff))
            {
                clusterAmountCalc = clusterAmountCalc + maxClusterAmountCalcDiff;
            }
            if (clusterAmountCalc < (clusterAmountCalc - maxClusterAmountCalcDiff))
            {
                clusterAmountCalc = clusterAmountCalc - maxClusterAmountCalcDiff;
            }
        }

        //Set the lower checkpoint higher
        checkPointOne.transform.position = new Vector3(checkPointTwo.transform.position.x + GenerateVariation(25f, 55f),
                                                       checkPointTwo.transform.position.y + checkPointDistance,
                                                       checkPointTwo.transform.position.z + GenerateVariation(25f, 55f));
        //Swap Checkpoint 1 with 2
        {
            GameObject tempCheckPoint = checkPointOne;
            checkPointOne = checkPointTwo;
            checkPointTwo = tempCheckPoint;
            checkPointOne.GetComponent<CheckPointBehavior>().isStart = true;
            checkPointTwo.GetComponent<CheckPointBehavior>().isStart = false;
        }

        TriggerGeneration();
    }
    public void TriggerGeneration()
    {

        clusterAmount = Mathf.RoundToInt(
                        Vector3.Distance(checkPointOne.transform.position, checkPointTwo.transform.position) / clusterAmountCalc - difficulty);
        clusterDistance = checkPointDistance / clusterAmount;
        GenerateClusters(checkPointOne.transform.position, checkPointTwo.transform.position);

    }

    public void GenerateClusters(Vector3 startPos, Vector3 endPos)
    {
        Vector3 spawnPos = startPos - new Vector3(0, 0, 0);
        //randomX = spawnPos.x;
        //randomZ = spawnPos.z;
        int currentCluster = 0; //-1 or 0  Random.Range(-1, 1);


        for (int i = 0; i < clusterAmount; i++)
        {
            spawnPos.x = startPos.x;
            spawnPos.z = startPos.z;
            randomX = Random.Range(-maxVariationZX, maxVariationZX) + clusterAmountCalc * currentCluster;
            randomZ = Random.Range(-maxVariationZX, maxVariationZX) + clusterAmountCalc * currentCluster;

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
