using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoriesStrategy : ScriptableObject
{
    public abstract Story[] GenerateStories(ClusterSettings settings, RectInt bounds, int numberOfStories);
}
