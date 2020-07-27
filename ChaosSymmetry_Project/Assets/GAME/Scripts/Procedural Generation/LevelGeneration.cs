using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    GameObject clusterObj;
    [SerializeField] GameObject clusterObjOne;
    [SerializeField] GameObject clusterObjTwo;
    [SerializeField] GameObject clusterObjThree;

    [SerializeField] GameObject Island1;
    [SerializeField] GameObject Island2;
    [SerializeField] GameObject Island3;

    [HideInInspector] public GameObject checkPointOne;
    [HideInInspector] public GameObject checkPointTwo;
    [HideInInspector] public GameObject[] checkPoints;



    GameObject powerUp, consumable;

    //---------------------------------------------------\\
    [Header("Generation values")]

        [Tooltip("Y Distance between checkpoints")]
        public float checkPointDistanceY = 50;

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


    float clusterDistanceY;
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
        //checkPoints = GameObject.FindGameObjectsWithTag("checkpoint");
        checkPointOne = GameObject.FindGameObjectWithTag("checkpoint");

        powerUp = ObjectManager.instance.powerUp;
        consumable = ObjectManager.instance.consumable;
      
        if (checkPointTwo != null)
        {
            Destroy(checkPointTwo);
        }

        Vector3 checkpointTransform = new Vector3(checkPointOne.transform.position.x + GenerateVariation(25f, 55f),
                                                       checkPointOne.transform.position.y + checkPointDistanceY,
                                                       checkPointOne.transform.position.z + GenerateVariation(25f, 55f));        

        checkPointTwo = Instantiate(RandomIsland(), checkpointTransform, Quaternion.identity) as GameObject;
        Instantiate(powerUp, checkPointTwo.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);


        if (checkPointOne != null && checkPointTwo != null)
        {
            TriggerGeneration();
        }

     
    }

    GameObject RandomIsland()
    {
        GameObject island = Island1;

        int random = Random.Range(0, 3);
        if (random == 1)
        {
            island = Island2;
        }
        else if (random == 2)
        {
            island = Island3;
        }
        return island;
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

    bool safeZoneDestroyed = false;
    public void MoveCheckpoint()
    {
        float oldClusterAmountCalc = clusterAmountCalc;

        if (safeZoneDestroyed == false)
        {
            GameObject safeZone = GameObject.FindGameObjectWithTag("safeZone");
            if (safeZone != null)
            {
                Destroy(safeZone);
            }
            safeZoneDestroyed = true;
        }
     

        //Increase difficulty
        {
            difficulty += incrDifficulty;
            checkPointDistanceY += incrCheckPointDistance;
            clusterAmountCalc += incrClusterAmountCalc;
        } 


        //Check if inside bounds
        {
            Mathf.Clamp(clusterAmountCalc, oldClusterAmountCalc - maxClusterAmountCalcDiff,
                                           oldClusterAmountCalc + maxClusterAmountCalcDiff);
         
        }

        //Set the lower checkpoint higher
        Vector3 checkpointTransform = new Vector3(checkPointTwo.transform.position.x + GenerateVariation(25f, 55f),
                                                       checkPointTwo.transform.position.y + checkPointDistanceY,
                                                       checkPointTwo.transform.position.z + GenerateVariation(25f, 55f));
        if (checkPointOne != null)
        {
            Destroy(checkPointOne);
        }

        checkPointOne = Instantiate(RandomIsland(), checkpointTransform, Quaternion.identity) as GameObject;
        Instantiate(powerUp, checkPointOne.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        if (PlayerPrefs.GetInt("gameMode") == 1)
        {
            UIManager.instance.endlessScore += 1;
            if(UIManager.instance.endlessScore > PlayerPrefs.GetInt("endlessHighScore"))
            {
                PlayerPrefs.SetInt("endlessHighScore", PlayerPrefs.GetInt("endlessHighScore") + 1);
            }
        }
        else
        {
            UIManager.instance.normalScore += 1;
            if(UIManager.instance.normalScore > PlayerPrefs.GetInt("normalHighScore"))
            {
                PlayerPrefs.SetInt("normalHighScore", PlayerPrefs.GetInt("normalHighScore") + 1);
            }
        }
        
        int consumableChance = Random.Range(0, 100);
        if(consumableChance >= 0)
        {
            Instantiate(consumable, checkPointOne.transform.position + new Vector3(2f, 0.9f, 0), Quaternion.identity);
        }

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
        float pointVaryOne = 6;
        float pointVaryTwo = 6;

        clusterAmount = Mathf.RoundToInt(
                        Vector3.Distance(checkPointOne.transform.position + new Vector3(0, pointVaryOne, 0), checkPointTwo.transform.position - new Vector3(0, pointVaryTwo, 0)) / clusterAmountCalc - difficulty);
        clusterDistanceY = (checkPointDistanceY - pointVaryTwo - pointVaryOne) / clusterAmount;

        GenerateClusters(checkPointOne.transform.position + new Vector3(0, pointVaryOne, 0), checkPointTwo.transform.position - new Vector3(0, pointVaryTwo, 0));

    }

    float RandomAngle()
    {

        float[] numbers = { 0,180};

        int randomIndex = Random.Range(0, numbers.Length);
        return numbers[randomIndex];

    }
    float Choose()
    {
        float[] numbers = { 0,1 };
        int randomIndex = Random.Range(0, numbers.Length);
        return numbers[randomIndex];

    }
    public void GenerateClusters(Vector3 startPos, Vector3 endPos)
    {
        Vector3 spawnPos = startPos;
        //randomX = spawnPos.x;
        //randomZ = spawnPos.z;
        int currentCluster = 0; //-1 or 0  Random.Range(-1, 1);
        float newX = endPos.x - startPos.x;
        float newZ = endPos.z - startPos.z;

        for (int i = 0; i <= clusterAmount; i++)
        {
            spawnPos.x = startPos.x;
            spawnPos.z = startPos.z;   

            randomX = Random.Range(-maxVariationZX, maxVariationZX) + newX / clusterAmount * currentCluster;
            randomZ = Random.Range(-maxVariationZX, maxVariationZX) + newZ / clusterAmount * currentCluster;


            spawnPos.y += clusterDistanceY;    
           
            spawnPos.x += randomX;
            spawnPos.z += randomZ;

            // spwan random Cluster
            int chance = Random.Range(0, 30);
            if(chance < 10)
            {
                clusterObj = clusterObjOne;
            }
            else if( chance >= 10 && chance < 20)
            {
                clusterObj = clusterObjTwo;
            }
            else if( chance >= 20)
            {
                clusterObj = clusterObjThree;
            }


            //Quaternion spawnAngle = new Quaternion(Choose(), Choose(), Choose(), RandomAngle());
            Quaternion spawnAngle = clusterObj.transform.rotation;
            spawnAngle = Quaternion.Euler(clusterObj.transform.eulerAngles.x + RandomAngle(),
                                                   clusterObj.transform.eulerAngles.y + RandomAngle(),
                                                   clusterObj.transform.eulerAngles.z + RandomAngle());

            //spawnAngle.x = clusterObj.transform.rotation.x + RandomAngle();
            //spawnAngle.y = clusterObj.transform.rotation.y + RandomAngle();
            //spawnAngle.z = clusterObj.transform.rotation.z + RandomAngle();

            
            CubeManager.instance.clusterObjects.Add(Instantiate(clusterObj, spawnPos, spawnAngle) as GameObject);
            if (CubeManager.instance.clusterObjects.Count > clusterAmount+1)
            {
                Destroy(CubeManager.instance.clusterObjects[0]);
                CubeManager.instance.clusterObjects.RemoveAt(0);

            }
            currentCluster += 1;
            
        }
    }
}
