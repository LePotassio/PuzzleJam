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
    private string sceneToLoad;

    // WARNING: Do not use this if multiple objects are the player! (Or make one a dominant "Player using a tag?")
    [Header("Starting Player Object Override")]
    [SerializeField]
    private bool overrideStartingPosition = false;

    [SerializeField]
    private PlayerPositionSave singlePositionOverride;

    //[SerializeField]
    //InteractionMode startingInteractionMode;

    // List of interactions you can swap to here

    public string SceneToLoad
    {
        get { return sceneToLoad; }
    }

    public bool OverrideStartingPosition
    {
        get { return overrideStartingPosition; }
    }

    public PlayerPositionSave SinglePositionOverride
    {
        get { return singlePositionOverride; }
    }
}

[System.Serializable]
public class PlayerPositionSave
{
    [SerializeField]
    private Vector2 singlePositionOverride;

    public Vector2 SinglePositionOverride
    {
        get { return singlePositionOverride; }
    }

    public PlayerPositionSave(Vector2 vec)
    {
        singlePositionOverride = vec;
    }
}
