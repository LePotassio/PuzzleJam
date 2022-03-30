using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridRow
{
    [SerializeField]
    private List<GridTile> rowContents;

    public List<GridTile> RowContents
    {
        get { return rowContents; }
    }
}
