﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    //[HideInInspector] public GameObject playerInstance;
    [HideInInspector] public GameObject player;
    public GameObject powerUp, consumable;
    [SerializeField] public Material glowMat;
    [SerializeField] public ParticleSystem barbieParticle;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
