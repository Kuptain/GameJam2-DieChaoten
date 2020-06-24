using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// ensure class initializer is called whenever scripts recompile
[InitializeOnLoadAttribute]
public static class PlayModeStateChangedExample
{
    // register an event handler when the class is initialized
    static PlayModeStateChangedExample()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        Debug.Log(state);
        if(state == PlayModeStateChange.EnteredPlayMode)
        {
            PlayerPrefs.SetInt("levelRestarted", 0);
        }
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            PlayerPrefs.SetInt("levelRestarted", 0);
        }
    }
}