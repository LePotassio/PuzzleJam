using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionWarp : MonoBehaviour
{
    [SerializeField]
    private bool startingPlayerOnly;

    [SerializeField]
    private bool byOffset = true;

    [SerializeField]
    private Vector2 warpPos;

    // issue: make sure warp cannot be "plugged" -> Could make it unpluggable or make sure never a case it is sealed after warping (but area cycles...)
    // could also just reset pos of pushed every screen change...
    public void DoWarp(GameObject objectToWarp)
    {
        PlayerMovement m = objectToWarp.GetComponent<PlayerMovement>();
        if ((!m || !m.StartingPlayer) && startingPlayerOnly)
            return;

        // Change the player's pos and change tile content lists
        m.Movable.CurrentTile.Contents.Remove(m.Movable.PuzzleElement);

        // also detach all connections

        List<Puzzle_Element> attachedElements = m.Movable.PuzzleElement.AttachedElements;

        for (int i = 0; i < attachedElements.Count; i++)
        {
            attachedElements[i].AttachedElements.Remove(m.Movable.PuzzleElement);
        }

        m.Movable.PuzzleElement.AttachedElements = new List<Puzzle_Element>();
        m.Movable.PuzzleElement.AttachedElements.Add(m.Movable.PuzzleElement);

        if (!byOffset)
            objectToWarp.transform.position = warpPos;
        else
            objectToWarp.transform.position = new Vector3(objectToWarp.transform.position.x + warpPos.x, objectToWarp.transform.position.y + warpPos.y, 0);

        m.GetComponent<MovableSnapAssign>().SnapToCenter();
    }
}
