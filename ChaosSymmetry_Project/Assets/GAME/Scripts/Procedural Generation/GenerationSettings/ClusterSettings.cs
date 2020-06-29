using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cluster Generation/Cluster Settings")]
public class ClusterSettings : ScriptableObject
{
    public Vector2Int clusterSize;

    // TODO: Add other strategies
    public RoofStrategy roofStrategy;
    public StoriesStrategy storiesStrategy;
    public WallsStrategy wallsStrategy;
    public StoryStrategy storyStrategy;
    public WingsStrategy wingsStrategy;
    public WingStrategy wingStrategy;

    public Vector2Int Size { get { return clusterSize; } }
}
