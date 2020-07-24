using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterDemo : MonoBehaviour
{
    public ClusterSettings settings;
    // Start is called before the first frame update
    void Start()
    {
        Cluster c = ClusterGenerator.Generate(settings);
        GetComponent<ClusterRenderer>().Render(c);
    }
}
