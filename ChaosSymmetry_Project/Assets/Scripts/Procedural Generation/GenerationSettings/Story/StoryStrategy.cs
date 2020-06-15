using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public abstract class StoryStrategy : ScriptableObject
{
    public abstract Story GenerateStory(ClusterSettings settings, RectInt bounds, int level);
}
