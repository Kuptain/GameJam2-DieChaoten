﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cluster c = ClusterGenerator.Generate();
        Debug.Log(c.ToString());
    }
}