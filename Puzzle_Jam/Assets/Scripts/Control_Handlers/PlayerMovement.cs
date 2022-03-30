using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private Movable movable;

    public Movable Movable
    {
        get { return movable; }
    }

    /// <summary>
    /// Update loop for player movement controls
    /// </summary>
    public void DoUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            GameManager.Instance.State = GameState.MoveStandby;


            string dir = "";
            if (Input.GetKey(KeyCode.UpArrow))
                dir = "up";
            else if (Input.GetKey(KeyCode.DownArrow))
                dir = "down";
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                sr.flipX = true;
                dir = "left";
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                sr.flipX = false;
                dir = "right";
            }

            PlayerMove(dir);
        }
    }

    public void PlayerMove(string dir)
    {   
        StartCoroutine(movable.Move(dir));
    }
}
