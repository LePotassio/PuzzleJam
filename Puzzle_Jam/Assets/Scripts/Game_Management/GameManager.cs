using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { PlayerMove, MoveStandby, MoveResolution, PauseMenu, TitleMenu };

/// <summary>
/// The game manager is a singleton class that keeps track of the update loop
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameState state;

    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private PlayerMovement player_movement;

    [SerializeField]
    private InteractionHandler interactionHandler;

    // Need a class for an action
    private List<GameObject> queuedMoves;

    public GameObject PlayerObject
    {
        get { return playerObject; }
    }

    public PlayerMovement PlayerMovement
    {
        get { return player_movement; }
    }

    public InteractionHandler InteractionHandler
    {
        get { return interactionHandler; }
    }

    public List<GameObject> QueuedMoves
    {
        get { return queuedMoves; }
    }

    public GameState State
    {
        get { return state; }
        set { state = value; }
    }

    private void Awake()
    {
        Instance = this;
        queuedMoves = new List<GameObject>();
    }

    private void Update()
    {
        // This is where all the gameplay handlers will go, certain ones will be called depending on what the gamestate is
        if (state == GameState.PlayerMove)
        {
            interactionHandler.DoUpdate();
            player_movement.DoUpdate();
        }
    }

    private void FixedUpdate()
    {
        // Physics handlers here
    }

    public IEnumerator WaitForMoveResolution()
    {
        yield return new WaitUntil(() => queuedMoves.Count == 0);
        state = GameState.PlayerMove;
    }
}
