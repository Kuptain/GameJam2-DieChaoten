using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClusterGenerator 
{
    public static Cluster Generate()
    {
        return new Cluster(4, 4,
            new Wing[] {
                new Wing(
                    new RectInt(0,0,4,4),
                    new Story[]
                    {
                        new Story(0, new Wall[((4+4)*2)])
                    },
                    new Roof()
                    )
            });
    }
}
