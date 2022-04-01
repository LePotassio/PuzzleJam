using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Depreocated, We don't need a strict gridlike structure with out architecture!
public class PuzzleGrid : MonoBehaviour
{
    [SerializeField]
    private string gridID;
    [SerializeField]
    private List<GridRow> tileGrid;

    public string GridID
    {
        get { return gridID; }
    }
    public List<GridRow> TileGrid
    {
        get { return tileGrid; }
    }

    public GridTile GetTile(int x, int y)
    {
        GridTile tile;
        try
        {
            tile = tileGrid[y].RowContents[x];
        }
        catch (System.IndexOutOfRangeException)
        {
            throw new System.IndexOutOfRangeException($"GetTile was provided with ({x}, {y}) but was out of range");
        }
        return tile;
    }
}
