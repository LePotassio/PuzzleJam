using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelWarp
{
    // Two main cases
    // - The level is completed and we want to return to level select
    // - The player steps on a level select in the lobby to start a level

    // The issue is that the first case always returns to the same place while second case goes to different places and two always has one condition while one has potentially many

    // Resolve this by having this implementation only

    [SerializeField]
    private int sceneToLoad;

    //[SerializeField]
    //InteractionMode startingInteractionMode;

    // List of interactions you can swap to here

    public int SceneToLoad
    {
        get { return sceneToLoad; }
    }
}
