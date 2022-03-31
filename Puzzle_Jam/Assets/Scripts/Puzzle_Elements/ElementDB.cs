using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDB : MonoBehaviour
{
    private static ElementDB instance;

    public static ElementDB Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    // Tables here will indicate things like interactions

    // Potential for a table of actions like melt, push, pull, etc

    /*
     * Element IDs
     * 
     * -1 Temp Element
     * 0 Player
     * 1 Immovable Rock
     * 2 Pushable/Pullable Rock
    */

    /*
     * Action IDs
     * 
     * -1 Not Expected
     * 0 No Action
     * 1 Block Movement
     * 2 Push Normal
    */

    public enum ActionIDs { NoAction, BlockMovement, PushNormal };

    // On Push Table
    public List<List<int>> PushTable = new List<List<int>>
    {
        // x axis is pushee
        // y axis is pusher

        new List<int> { -1,  1,  2 }, // Player
        new List<int> { -1, -1, -1 }, // Immovable Rock
        new List<int> {  2,  1,  2 }, // Pushable/Pullable Rock
    };

    public int GetPushID(int pusherID, int pushedID)
    {
        return PushTable[pusherID][pushedID];
    }

    // Can Pull Table?
    public List<List<int>> CanPullTable = new List<List<int>>
    {
        // x axis is pulled
        // y axis is puller

        // -1 -> Unexpected
        // 0  -> Cannot Push
        // 1  -> Can Push
        new List<int> {  1,  0,  1 }, // Player
        new List<int> { -1, -1, -1 }, // Immovable Rock
        new List<int> { -1, -1, -1 }, // Pushable/Pullable Rock
    };

    public bool GetCanPull(int pullerID, int pulledID)
    {
        bool res = false;
        if (CanPullTable[pullerID][pulledID] == 1)
            res = true;
        return res;
    }
}
