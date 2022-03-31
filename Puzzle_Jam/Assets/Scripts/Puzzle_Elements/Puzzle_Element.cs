using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Element : MonoBehaviour
{
    // element descriptor list might be good, like Ice, etc... or could use interface...

    [SerializeField]
    private int elementID;

    [SerializeField]
    private Vector2 facingDirection;

    [SerializeField]
    private Vector2 startingDirection = new Vector2 (0, -1);

    [SerializeField]
    private List<Puzzle_Element> attachedElements;

    private SpriteRenderer sr;


    public event Action<Vector2> OnChangeDirection;

    private void Awake()
    {
        facingDirection = startingDirection;
        attachedElements.Add(this);
    }

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public int ElementID
    {
        get { return elementID; }
    }

    public Vector2 FacingDirection
    {
        get { return facingDirection; }
    }

    public Vector2 StartingDirection
    {
        get { return startingDirection; }
    }

    public List<Puzzle_Element> AttachedElements
    {
        get { return attachedElements; }
    }

    public bool ChangeFacingDirection(string dir)
    {
        // again, this doesnt account for attached but not holding... But maybe this is how we want it? to block interaction
        if (attachedElements.Count > 1)
            return false;

        dir = dir.ToLower();

        Vector2 oldDirection = facingDirection;

        if (dir == "up")
            facingDirection = new Vector2(0, 1);
        else if (dir == "down")
            facingDirection = new Vector2(0, -1);
        else if (dir == "left")
        {
            sr.flipX = true;
            facingDirection = new Vector2(-1, 0);
        }
        else if (dir == "right")
        {
            sr.flipX = false;
            facingDirection = new Vector2(1, 0);
        }
        if (facingDirection == oldDirection)
        {
            return false;
        }

        OnChangeDirection?.Invoke(facingDirection);

        return true;
    }
}
