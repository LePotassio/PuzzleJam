using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { PlayerMove, MoveStandby, MoveResolution, PuzzleSolved, PauseMenu, TitleMenu, SaveMenu, LoadMenu };

public enum CameraState { PuzzleLocked, Follow };

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

    // Could potential be put into the essentail objects to allow for main menu access everywhere...
    [SerializeField]
    private MainMenu mainMenu;

    [SerializeField]
    private MainMenu loadMenu;

    [SerializeField]
    private PauseMenu pauseMenu;

    [SerializeField]
    private PauseMenu saveMenu;

    [SerializeField]
    private PuzzleUI puzzleUI;

    [SerializeField]
    private InteractionModeUI interactionModeUI;

    // Need a class for an action
    private List<GameObject> queuedMoves;

    [SerializeField]
    private List<WinCondition> currentWinConditions;

    [SerializeField]
    private FullPuzzle currentPuzzle;

    [SerializeField]
    private Camera mainCamera;

    private CameraState cameraMode = CameraState.PuzzleLocked;

    //[SerializeField]
    private SaveFileProgress saveFileProgress;

    // private Vector2 onCompletionReturnPosition;
    [SerializeField]
    PlayerMovement startingPlayerRef;

    private bool winDelay;

    private GameState pausedStateCache;

    private int buildSceneCount;

    public List<PlayerMovement> PlayerMovements
    {
        get { return player_movements; }
    }

    public MainMenu MainMenu
    {
        get { return mainMenu; }
        set { mainMenu = value; }
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

    public SaveFileProgress SaveFileProgress
    {
        get { return saveFileProgress; }
        set { saveFileProgress = value; }
    }

    public PlayerMovement StartingPlayerRef
    {
        get { return startingPlayerRef; }
        set { startingPlayerRef = value; }
    }

    public GameState State
    {
        get { return state; }
        set {
            state = value;
        }
    }

    public int BuildSceneCount
    {
        get { return buildSceneCount; }
    }

    public CameraState CameraMode
    {
        get { return cameraMode; }
        set
        {
            cameraMode = value;
            RecenterCameraToBox();
        }
    }

    private void Awake()
    {
        buildSceneCount = SceneManager.sceneCountInBuildSettings;

        DontDestroyOnLoad(this);
        Instance = this;
        // default to no progress, then load a save from menu handler...
        saveFileProgress = new SaveFileProgress();
        queuedMoves = new List<GameObject>();
    }

    private void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        saveMenu.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            State = GameState.TitleMenu;
            MainMenu.OpenMenu();
            puzzleUI.gameObject.SetActive(false);
        } else
        {
            StartCoroutine(UpdateCurrentPuzzleUI());
        }
    }

    private void Update()
    {
        // This is where all the gameplay handlers will go, certain ones will be called depending on what the gamestate is
        if ((state != GameState.TitleMenu && state != GameState.LoadMenu) && GameSettings.Instance.GetKeyBindingDown(KeyButtons.PauseResume))
        {
            if (state == GameState.PauseMenu)
                ResumeGame();
            else
                PauseGame();
        }
        else if (state == GameState.PlayerMove)
        {
            interactionHandler.DoUpdate();
            foreach (PlayerMovement pm in player_movements)
                pm.DoUpdate();
        }
        else if (state == GameState.PuzzleSolved)
        {
            Debug.Log("A winner is you!");
            // You win goes here

            SaveFileProgress.LevelCompletions[SceneManager.GetActiveScene().name] = LevelStatus.Completed;

            // Then swap back to level select
            LevelWarp onCompletionWarp = currentPuzzle.OnCompletionWarp;
            if (!onCompletionWarp.OverrideStartingPosition)
                SwitchLevel(onCompletionWarp.SceneToLoad);
            else
                SwitchLevel(onCompletionWarp.SceneToLoad, onCompletionWarp.SinglePositionOverride);
        }
        else if (state == GameState.TitleMenu)
        {
            mainMenu.DoUpdate();
        }
        else if (state == GameState.PauseMenu)
        {
            pauseMenu.DoUpdate();
        }
        else if (state == GameState.SaveMenu)
        {
            saveMenu.DoUpdate();
        }
        else if (state == GameState.LoadMenu)
        {
            loadMenu.DoUpdate();
        }
    }

    private void LateUpdate()
    {
        if (cameraMode == CameraState.Follow && startingPlayerRef)
        {
            Vector3 targ = startingPlayerRef.transform.position;
            mainCamera.transform.position = new Vector3(targ.x, targ.y, -10);
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

    public void SwitchLevel(string sceneName, PlayerPositionSave startingPlayerPosOverride = null)
    {
        Scene curScene = SceneManager.GetActiveScene();
        // Unload old scene
        //SceneManager.UnloadSceneAsync(curScene);
        // Transition and camera enabling/disabling

        ClearGameManager();// Keep in mind, this could lead to suspended time for handlers... would need to wait for scene to load...
        
        // Wait for load issue: Need to wait for all references to be setup, and cant load before unload

        // Load new scene
        SceneManager.LoadScene(sceneName);

        StartCoroutine(UpdateCurrentPuzzleUI());

        if (startingPlayerPosOverride != null)
            StartCoroutine(OverrideStartingPlayerPos(startingPlayerPosOverride));
    }

    public void ClearGameManager()
    {
        queuedMoves = new List<GameObject>();
        player_movements = new List<PlayerMovement>();
        currentWinConditions = new List<WinCondition>();
        interactionHandler.ClearInteractionHandler();
        currentPuzzle = null;
        winDelay = true;
        startingPlayerRef = null;

        State = GameState.PlayerMove;
    }

    public IEnumerator OverrideStartingPlayerPos(PlayerPositionSave startingPlayerPos)
    {
        // Assume we always have a starting player ref if we are overriding its position...
        yield return new WaitUntil(() => startingPlayerRef != null);

        startingPlayerRef.transform.position = startingPlayerPos.SinglePositionOverride;

        // Snap to center deals with tile content management (but would need delay for starting interactions... That would be good in general considering awake order)
        startingPlayerRef.GetComponent<MovableSnapAssign>().SnapToCenter();
    }

    // Will need to be changed to find nearest grid center and then center to that...
    public void RecenterCamera(GameObject center)
    {
        if (cameraMode == CameraState.PuzzleLocked) // Can change this later for camera size and smooth cahnging (smoothly lerp to terget position in update to avoid coroutine cancellation hassle)
            mainCamera.transform.position = new Vector3(center.transform.position.x, center.transform.position.y, -10);
    }

    public void RecenterCameraToBox()
    {
        var cb = Physics2D.OverlapCircle(startingPlayerRef.transform.position, 0.15f, GameLayers.Instance.CamBoxLayer);
        if (cb)
            RecenterCamera(cb.gameObject);
    }

    public void StartMainMenu()
    {
        pauseMenu.gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
        MainMenu.OpenMenu();
        State = GameState.TitleMenu;
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Puzzle_Lobby_1");
        puzzleUI.gameObject.SetActive(true);
        StartCoroutine(UpdateCurrentPuzzleUI());
        MainMenu.CloseMenu();
        saveFileProgress = new SaveFileProgress();
        State = GameState.PlayerMove;
    }

    public void SaveGame(int saveSlot)
    {
        saveFileProgress.SaveProgress(saveSlot);

        Debug.Log($"Saved to save slot {saveSlot}");

    }

    public void LoadGame(int saveSlot)
    {
        saveFileProgress = SaveSystem.LoadProgress(saveSlot);

        //Temporary until checkpoints
        SceneManager.LoadScene("Puzzle_Lobby_1");

        puzzleUI.gameObject.SetActive(true);
        StartCoroutine(UpdateCurrentPuzzleUI());
        MainMenu.CloseMenu();
        loadMenu.CloseMenu();
        State = GameState.PlayerMove;

        Debug.Log($"Loaded from save slot {saveSlot}");
    }

    private IEnumerator UpdateCurrentPuzzleUI()
    {
        yield return new WaitUntil(() => currentPuzzle);
        puzzleUI.SetSidebarPuzzleInfo(currentPuzzle.LevelData);
    }

    public void OpenLoadMenu()
    {
        loadMenu.OpenMenu();
        pausedStateCache = State;
        State = GameState.LoadMenu;
    }

    public void CloseLoadMenu()
    {
        State = pausedStateCache;
        loadMenu.gameObject.SetActive(false);
    }

    // Will we run into movement cancellation problem? Pause the moment the waituntil changes the state in movable...
    public void PauseGame()
    {
        pauseMenu.OpenMenu();
        pausedStateCache = State;
        State = GameState.PauseMenu;
    }

    public void ResumeGame()
    {
        State = pausedStateCache;
        pauseMenu.gameObject.SetActive(false);
    }

    public void OpenSaveMenu()
    {
        pauseMenu.GetComponent<Canvas>().enabled = false;
        saveMenu.OpenMenu();
        State = GameState.SaveMenu;
    }

    public void CloseSaveMenu()
    {
        pauseMenu.GetComponent<Canvas>().enabled = true;
        State = GameState.PauseMenu;
        saveMenu.gameObject.SetActive(false);
    }

    public void UpdateInteractionModeUI()
    {
        UpdateInteractionModeUI(startingPlayerRef.GetComponentInChildren<InteractionReticle>());
    }

    public void UpdateInteractionModeUI(InteractionReticle originalPlayerReticle)
    {
        interactionModeUI.SetInteractionUI(originalPlayerReticle.InteractionMode, originalPlayerReticle.AvailableInteractions.Count > 1);
    }

    /*public void SwapToNextInteractionMode()
    {
        InteractionReticle originalPlayerReticle = startingPlayerRef.GetComponentInChildren<InteractionReticle>();
        if (originalPlayerReticle.InteractionMode == InteractionMode.Release)
            return;

        foreach (InteractionReticle r in interactionHandler.Reticles)
        {
            if (r.InteractionMode == InteractionMode.Release)
                continue;
            r.CurrentInteraction++;
            if (r.CurrentInteraction > r.AvailableInteractions.Count - 1)
                r.CurrentInteraction = 0;
            r.InteractionMode = r.AvailableInteractions[r.CurrentInteraction];

            //if (r.InteractionMode == InteractionMode.Pull && r)
        }
        // Conundrum, UI shows original player's interactrion mode but for multiple? (maybe just drop the idea of differing interactions, Compromise your idea!)
        UpdateInteractionModeUI(originalPlayerReticle);
    }*/

    public void SetInteractionLists(List<InteractionMode> modeList)
    {
        foreach (InteractionReticle r in interactionHandler.Reticles)
        {
            Debug.Log(r);
            r.CurrentInteraction = 0;
            r.AvailableInteractions = modeList;
            r.InteractionMode = r.AvailableInteractions[0];
        }
    }
}
