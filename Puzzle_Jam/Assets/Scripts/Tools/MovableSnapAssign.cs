using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableSnapAssign : MonoBehaviour
{
    //private bool ftFlag = true;

    /*
    // Could also just physics circle on start...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GridTile tile = collision.GetComponent<GridTile>();
        if (ftFlag && tile)
        {
            ftFlag = false;
            SnapToCenter(tile);
        }
    }*/

    private void Start()
    {
        SnapToCenter();
    }

    public void SnapToCenter()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.15f, GameLayers.Instance.TileLayer);
        if (colliders.Length == 0)
            return;
        GridTile tile = colliders[0].GetComponent<GridTile>();

        this.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, 0);

        Movable m = GetComponent<Movable>();
        Puzzle_Element pz = gameObject.GetComponent<Puzzle_Element>();

        if (m.CurrentTile)
            m.CurrentTile.Contents.Remove(pz);

        m.CurrentTile = tile;
        tile.Contents.Add(pz);
        pz.CurrentTile = tile;
    }
}
