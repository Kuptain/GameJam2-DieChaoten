﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{

    public Material freezeMat;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color colorHover, colorHoverExploded;

    public float speed;
    public float slowmoValue = 0.25f;
    public float gravityValue = 0.1f; //The value of the gravity
    public float currentSlowmo = 1;

    public float maxGravity = 0.1f; //gravityChange can not be higher than this
    public float sendBackManual = 0.02f;
    public float sendBackAuto = 0.001f;
    public float returnDelay = 3f;
    public float orbitMaxRotation = 45;
    public float clusterMaxRotation = 20;



    [HideInInspector] public int testMode;
    [HideInInspector] public bool gameModeAllClusters;

    public float pushForce = 10;

    public static CubeManager instance;
    public bool slowMode = false;

    GameObject player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    void Start()
    {
        player = ObjectManager.instance.player;

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
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameModeAllClusters = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameModeAllClusters = false;
        }*/

        //Slowmotion
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if(slowMode == false)
                {
                    slowMode = true;

                }
                else if(slowMode == true)
                {
                    slowMode = false;

                }
            }
         
        }
    }
}
