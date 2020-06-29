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

    /*static Wing[] GenerateWings(ClusterSettings settings)
    {
        return new Wing[] 
        {
            settings.wingStrategy != null ?
                settings.wingStrategy.GenerateWing(
                    settings,
                    new RectInt(0, 0, settings.Size.x, settings.Size.y),
                    1) :
                ((WingStrategy)ScriptableObject.CreateInstance<DefaultWingStrategy>()).GenerateWing(
                    settings,
                    new RectInt(0, 0, settings.Size.x, settings.Size.y),
                    1
                    )
                
        };
    }

   /* static Wing GenerateWing(ClusterSettings settings, RectInt bounds, int numberOfStories)
    {
        return new Wing(
                    bounds,
                    settings.storiesStrategy != null ?
                        settings.storiesStrategy.GenerateStories(settings, bounds, numberOfStories) :
                        ((StoriesStrategy)ScriptableObject.CreateInstance<DefaultStoriesStrategy>()).GenerateStories(settings, bounds, numberOfStories),
                    settings.roofStrategy != null ?
                        settings.roofStrategy.GenerateRoof(settings, bounds) :
                        ((RoofStrategy)ScriptableObject.CreateInstance<DefaultRoofStrategy>()).GenerateRoof(settings, bounds)
                    );
    }

   /* static Story[] GenerateStories(ClusterSettings settings, RectInt bounds, int numberOfStories)
    {
        return new Story[] { settings.storiesStrategy != null ?
            settings.storyStrategy.GenerateStory(settings, bounds, 1) :
            ((StoryStrategy)ScriptableObject.CreateInstance<DefaultStoryStrategy>()).GenerateStory(settings, bounds, 1)
        };
    }

    /*static Story GenerateStory(ClusterSettings settings, RectInt bounds, int level)
    {
        return new Story(0, settings.wallsStrategy != null ?
            settings.wallsStrategy.GenerateWalls(settings, bounds, level) :
            ((WallsStrategy)ScriptableObject.CreateInstance<DefaultWallsStrategy>()).GenerateWalls(settings, bounds, level)
        );
    }

    /*static Wall[] GenerateWalls(ClusterSettings settings, RectInt bounds, int level)
    {
        return new Wall[(bounds.size.x + bounds.size.y) * 2];
    }*/

    /* static Roof GenerateRoof(ClusterSettings settings, RectInt bounds)
     {
         return new Roof();
     }*/
}
