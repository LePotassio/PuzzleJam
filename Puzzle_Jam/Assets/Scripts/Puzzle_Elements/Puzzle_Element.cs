using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Element : MonoBehaviour
{
    // element descriptor list might be good, like Ice, etc... or could use interface...

    [SerializeField]
    private int elementID;

    public int ElementID
    {
        get { return elementID; }
    }
}
