using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private Movable movable;

    private Puzzle_Element puzzleElement;

    private bool isChangingDir;

    public Movable Movable
    {
        get { return movable; }
    }

    private void Awake()
    {
        isChangingDir = false;
    }

    private void Start()
    {
        puzzleElement = GetComponent<Puzzle_Element>();
        if (!puzzleElement)
            Debug.Log("WARNING: PlayerMovement was initialized on an object without a puzzle element!");
    }

    /// <summary>
    /// Update loop for player movement controls
    /// </summary>
    public void DoUpdate()
    {
        if (isChangingDir)
            return;

        if (GameSettings.Instance.GetKeyBinding(KeyButtons.MoveUp) || GameSettings.Instance.GetKeyBinding(KeyButtons.MoveDown) || GameSettings.Instance.GetKeyBinding(KeyButtons.MoveLeft) || GameSettings.Instance.GetKeyBinding(KeyButtons.MoveRight))
        {
            GameManager.Instance.State = GameState.MoveStandby;


            string dir = "";
            if (GameSettings.Instance.GetKeyBinding(KeyButtons.MoveUp))
                dir = "up";
            else if (GameSettings.Instance.GetKeyBinding(KeyButtons.MoveDown))
                dir = "down";
            else if (GameSettings.Instance.GetKeyBinding(KeyButtons.MoveLeft))
            {
                dir = "left";
            }
            else if (GameSettings.Instance.GetKeyBinding(KeyButtons.MoveRight))
            {
                dir = "right";
            }

            if (!puzzleElement.ChangeFacingDirection(dir))
                PlayerMove(dir);
            else
            {
                StartCoroutine(WaitForDirectionChange());
                GameManager.Instance.State = GameState.PlayerMove;
            }
        }
    }

    public void PlayerMove(string dir)
    {   
        StartCoroutine(movable.Move(dir));
    }

    private IEnumerator WaitForDirectionChange()
    {
        isChangingDir = true;
        yield return new WaitForSeconds(GameSettings.ChangeDirectionDelay);
        isChangingDir = false;
    }
}
