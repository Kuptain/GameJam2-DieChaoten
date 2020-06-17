using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehavior : MonoBehaviour
{
    public bool isStart = false;
    public bool canBuild = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CallGenerationFunction();
    }

    void CallGenerationFunction()
    {
        if (isStart && canBuild)
        {
            canBuild = false;
            LevelGeneration.instance.TriggerGeneration();
        }
    }
}
