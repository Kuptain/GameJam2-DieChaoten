using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClusterGenerator
{
    public static Cluster Generate(ClusterSettings settings)
    {
        return new Cluster(settings.Size.x,
            settings.Size.y,
            settings.wingsStrategy != null ?
                settings.wingsStrategy.GenerateWings(settings) :
                ((WingsStrategy)ScriptableObject.CreateInstance<DefaultWingsStrategy>()).GenerateWings(settings)
                );
    }

 
}
