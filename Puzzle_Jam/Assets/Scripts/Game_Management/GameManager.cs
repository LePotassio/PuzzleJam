using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { PlayerMove, MoveStandby, MoveResolution, PuzzleSolved, PauseMenu, TitleMenu };

/// <summary>
/// The game manager is a singleton class that keeps track of the update loop
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameState state;

    [SerializeField]
    private List<PlayerMovement> player_movements;

    [SerializeField]
    private InteractionHandler interactionHandler;

    // Need a class for an action
    private List<GameObject> queuedMoves;

    [SerializeField]
    private List<WinCondition> currentWinConditions;

    [SerializeField]
    private FullPuzzle currentPuzzle;

    [SerializeField]
    private Camera mainCamera;

    private bool winDelay;

    public List<PlayerMovement> PlayerMovements
    {
        get { return player_movements; }
    }

    public InteractionHandler InteractionHandler
    {
        get { return interactionHandler; }
    }

    public List<GameObject> QueuedMoves
    {
        get { return queuedMoves; }
    }

    public List<WinCondition> CurrentWinConditions
    {
        get { return currentWinConditions; }
    }

    public FullPuzzle CurrentPuzzle
    {
        get { return currentPuzzle; }
        set { currentPuzzle = value; }
    }
    
    public bool WinDelay
    {
        get { return winDelay; }
        set { winDelay = value; }
    }

    public GameState State
    {
        get { return state; }
        set { state = value; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        queuedMoves = new List<GameObject>();
    }

    private void Update()
    {
        // This is where all the gameplay handlers will go, certain ones will be called depending on what the gamestate is
        if (state == GameState.PlayerMove)
        {
            interactionHandler.DoUpdate();
            foreach (PlayerMovement pm in player_movements)
                pm.DoUpdate();
        }
        else if (state == GameState.PuzzleSolved)
        {
            Debug.Log("A winner is you!");
            // You win goes here

            // Then swap back to level select
            SwitchLevel(currentPuzzle.OnCompletionWarp.SceneToLoad);
        }
    }

    // Call this after ANY Movement or interaction that could result in a win...
    public bool CheckAllWinConditions()
    {
        if (winDelay)
            return false;

        foreach (WinCondition winCondition in currentWinConditions)
        {
            if (!winCondition.CheckFullfilled())
                return false;
        }
        return true;
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

    public void SwitchLevel(int sceneIndex)
    {
        Scene curScene = SceneManager.GetActiveScene();
        // Unload old scene
        //SceneManager.UnloadSceneAsync(curScene);
        // Transition and camera enabling/disabling

        ClearGameManager();// Keep in mind, this could lead to suspended time for handlers... would need to wait for scene to load...
        
        // Wait for load issue: Need to wait for all references to be setup, and cant load before unload

        // Load new scene
        SceneManager.LoadScene(sceneIndex);
    }

    public void ClearGameManager()
    {
        queuedMoves = new List<GameObject>();
        player_movements = new List<PlayerMovement>();
        currentWinConditions = new List<WinCondition>();
        interactionHandler.ClearInteractionHandler();
        currentPuzzle = null;
        winDelay = true;
        state = GameState.PlayerMove;
    }

    public void RecenterCamera(GameObject center)
    {
        mainCamera.transform.position = new Vector3(center.transform.position.x, center.transform.position.y, -10);
    }
}
