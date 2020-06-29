using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster
{

    Vector2Int size;
    Wing[] wings;

    public Vector2Int Size { get { return size; } }
    public Wing[] Wings { get { return wings; } }

    public Cluster(int sizeX, int sizeY, Wing[] wings)
    {
        size = new Vector2Int(sizeX, sizeY);
        this.wings = wings;
    }

    public override string ToString()
    {
        string clst = "Cluster:(" + size.ToString() + ";" + wings.Length + ")\n";
        foreach(Wing w in wings)
        {
            clst += "\t" + w.ToString() + "\n";
        }
        return clst;
    }

}
