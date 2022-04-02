using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableSnapAssign : MonoBehaviour
{
    private bool ftFlag = true;

    // Could also just physics cirlc on start...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GridTile tile = collision.GetComponent<GridTile>();
        if (ftFlag && tile)
        {
            ftFlag = false;
            SnapToCenter(tile);
        }
    }

    private void SnapToCenter(GridTile tile)
    {
        this.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, 0);
        GetComponent<Movable>().CurrentTile = tile;
        Puzzle_Element pz = gameObject.GetComponent<Puzzle_Element>();
        tile.Contents.Add(pz);
        pz.CurrentTile = tile;
    }
}
