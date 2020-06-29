using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WingStrategy : ScriptableObject
{
    public abstract Wing GenerateWing(ClusterSettings settings, RectInt bounds, int numberOfStories);
}
