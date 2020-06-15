using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    [SerializeField] Button buttonA;
    [SerializeField] Button buttonB;

    [HideInInspector] public int testMode;

    public float pushForce = 10;

    public static CubeManager instance;

    public GameObject player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    void Start()
    {
        
    }

    public void ChangeToA()
    {
        testMode = 0;
    }
    public void ChangeToB()
    {
        testMode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
