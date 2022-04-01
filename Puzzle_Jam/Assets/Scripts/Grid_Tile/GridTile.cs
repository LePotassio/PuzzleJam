using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField]
    private List<Puzzle_Element> contents;

    public int x;
    public int y;

    /// <summary>
    /// Represents the things currently in this tile, i.e rock, player (will typically have either 0 or 1 in most cases)
    /// </summary>
    public List<Puzzle_Element> Contents
    {
        get { return contents; }
    }

    public bool CheckForInvalidOverlaps()
    {
        // Checks if there are overlapping elements that are not allowed to share the same spot

        // Temporary, no elements are allowed to share a space

        return contents.Count > 1;
    }

    public bool CheckForContentByID(int id)
    {
        foreach (Puzzle_Element e in contents)
        {
            if (e.ElementID == id)
                return true;
        }

        return false;
    }
}
