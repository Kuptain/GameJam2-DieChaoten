using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStoriesStrategy : StoriesStrategy
{
    public override Story[] GenerateStories(ClusterSettings settings, RectInt bounds, int numberOfStories)
    {
        return new Story[] { settings.storiesStrategy != null ?
            settings.storyStrategy.GenerateStory(settings, bounds, 1) :
            ((StoryStrategy)ScriptableObject.CreateInstance<DefaultStoryStrategy>()).GenerateStory(settings, bounds, 1)
        };
    }
}
