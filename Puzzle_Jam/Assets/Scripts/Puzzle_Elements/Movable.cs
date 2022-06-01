using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    private GameObject movableObj;

    private GridTile currentTile;

    private Rigidbody2D rb;

    private Puzzle_Element puzzleElement;

    [SerializeField]
    private float moveSpeed = 3f;

    private LevelSelector ls;

    [SerializeField]
    private List<(PositionWarp, GameObject)> queuedWarps;

    public GameObject Player_Obj
    {
        get { return movableObj; }
    }

    public GridTile CurrentTile
    {
        get { return currentTile; }
        set { currentTile = value; }
    }

    public Puzzle_Element PuzzleElement
    {
        get { return puzzleElement; }
    }

    private void Awake()
    {
        queuedWarps = new List<(PositionWarp, GameObject)> ();
        movableObj = gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        puzzleElement = GetComponent<Puzzle_Element>();
        if (!puzzleElement)
            Debug.Log("WARNING: Movable was initialized on an object without a puzzle element!");
    }

    public IEnumerator Move(string dir, int tileDistance = 1)
    {
        GameManager.Instance.WinDelay = false;

        dir = dir.ToLower();

        if (!(dir == "up" || dir == "down" || dir == "left" || dir == "right"))
        {
            yield break;
        }

        Vector3 dirVec = Vector3.zero;

        if (dir == "up")
        {
            dirVec = new Vector3(0, tileDistance, 0);
        }
        else if (dir == "down")
        {
            dirVec = new Vector3(0, -tileDistance, 0);
        }
        else if (dir == "left")
        {
            yield return dirVec = new Vector3(-tileDistance, 0, 0);
        }
        else if (dir == "right")
        {
            dirVec = new Vector3(tileDistance, 0, 0);
        }

        // Get the destination tile and also check if the move is valid
        //GridTile destTile = CheckPathClear(dirVec);

        // The destination was not clear or did not exist in the grid
        bool isClear = true;

        foreach (Puzzle_Element attachedElem in puzzleElement.AttachedElements)
        {
            Movable attachedMov = attachedElem.GetComponent<Movable>();
            if (!attachedMov || !attachedMov.CheckClearRecursive(new List<Puzzle_Element>(), new List<GridTile>(), dirVec))
            {
                isClear = false;
                break;
            }
        }

        if (!isClear)
        {
            // Could show invalid move here
            GameManager.Instance.State = GameState.PlayerMove;
            yield break;
        }

        // Move is a go...
        GameManager.Instance.State = GameState.MoveResolution;

        //currentTile.Contents.Remove(gameObject);
        //destTile.Contents.Add(gameObject);

        // around here, will need to check and do any actions caused by the player's movement

        //yield return MoveByAmount(dirVec);
        //ProcAllMovements(dirVec);

        foreach(Puzzle_Element attachedElem in puzzleElement.AttachedElements)
        {
            Movable attachedMov = attachedElem.GetComponent<Movable>();
            attachedMov.ProcAllMovements(dirVec);
        }

        yield return new WaitUntil(() => GameManager.Instance.QueuedMoves.Count == 0 && !GameSettings.Instance.TimePausedState());

        GameState s = GameManager.Instance.State;
        if (s == GameState.MoveResolution || s == GameState.PlayerMove || s == GameState.MoveStandby)
            ls?.ActivateLevelSelect();

        foreach (var warp in queuedWarps)
        {
            warp.Item1.DoWarp(warp.Item2);
        }
        queuedWarps = new List<(PositionWarp, GameObject)>();

        foreach (InteractionReticle r in GameManager.Instance.InteractionHandler.Reticles)
        {
            r.UpdateCurrentTile();
        }

        // Could just directly move to win in Gamemanager... I think this way is more flexible though
        if (GameManager.Instance.State == GameState.Cutscene || GameManager.Instance.State == GameState.LoadingScreen)
            yield break;
        if (!GameManager.Instance.CheckAllWinConditions())
            GameManager.Instance.State = GameState.PlayerMove;
        else
            GameManager.Instance.State = GameState.PuzzleSolved;
    }

    private GridTile CheckPathClear(Vector3 dirVec)
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position + dirVec, 0.15f, GameLayers.Instance.TileLayer);

        if (colliders.Length == 0)
            return null;

        GridTile tileToCheck = colliders[0].GetComponent<GridTile>();

        // Go through contents and search for a player block
        foreach (Puzzle_Element elem in tileToCheck.Contents)
        {
            if (ElementDB.Instance.GetPushID(puzzleElement.ElementID, elem.ElementID) == (int)ElementDB.ActionIDs.BlockMovement)
            {
                return null;
            }
        }

        return colliders[0].GetComponent<GridTile>();
    }

    // Deprocated
    private IEnumerator MoveByAmount (Vector3 dirVec)
    {
        // Move x or y amount over time by lerping

        Vector3 target = new Vector3(gameObject.transform.position.x + dirVec.x, gameObject.transform.position.y + dirVec.y, 0);

        while ((target - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            //transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * moveSpeed);
            if (GameManager.Instance.State != GameState.PauseMenu)
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            // May need to add yield to wait for resuming from pause (waituntil state == moveresolution)

            yield return null;
        }

        transform.position = target;
        GameManager.Instance.QueuedMoves.Remove(gameObject);
    }

    private bool CheckClearRecursive(List<Puzzle_Element> temps, List<GridTile> recursedTiles, Vector3 dirVec)
    {
        foreach (Puzzle_Element attachedElem in puzzleElement.AttachedElements)
        {
            var colliders = Physics2D.OverlapCircleAll(attachedElem.transform.position + dirVec, 0.15f, GameLayers.Instance.TileLayer);

            // Base case: The tile element is moving into an out of grid tile
            if (colliders.Length == 0)
            {
                RemoveTemps(temps);
                return false;
            }

            GridTile tileToCheck = colliders[0].GetComponent<GridTile>();

            bool movingPiece = false;
            int movableIndex = -1;

            // Go through contents and search for a player block
            foreach (Puzzle_Element targetElem in tileToCheck.Contents)
            {
                movableIndex++;
                // Base case: The tile element is moving into an immovable tile
                if (attachedElem.ElementID >= 0 && targetElem.ElementID >= 0 && ElementDB.Instance.GetPushID(attachedElem.ElementID, targetElem.ElementID) == (int)ElementDB.ActionIDs.BlockMovement)
                {
                    RemoveTemps(temps);
                    return false;
                }
                if (attachedElem.ElementID >= 0 && targetElem.ElementID >= 0 && ElementDB.Instance.GetPushID(attachedElem.ElementID, targetElem.ElementID) == (int)ElementDB.ActionIDs.PushNormal)
                {
                    movingPiece = true;
                    break;
                }
            }

            Movable frontMovable = null;
            if (movingPiece)
                frontMovable = tileToCheck.Contents[movableIndex].GetComponent<Movable>();

            // Create a temporary element in the space in front
            GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/TempElement"), tileToCheck.transform);
            Puzzle_Element tempElem = temp.GetComponent<Puzzle_Element>();
            temps.Add(tempElem);
            tileToCheck.Contents.Add(tempElem);
            recursedTiles.Add(currentTile);

            if (movingPiece && !recursedTiles.Contains(tileToCheck))
            {
                // CheckAndMove the element before it
                return frontMovable.CheckClearRecursive(temps, recursedTiles, dirVec);
            }
        }
        // Base case: The tile element in question is pushing into an available or space occupied by a moving piece

        // Check no temps overlap
        foreach(GridTile recursedTile in recursedTiles)
        {
            bool hasTemp = false;
            foreach (Puzzle_Element elem in recursedTile.Contents)
            {
                if (elem.ElementID == -1)
                {
                    if (!hasTemp)
                        hasTemp = false;
                    else
                    {
                        RemoveTemps(temps);
                        return false;
                    }
                }
            }
        }

        // Clear the temps
        RemoveTemps(temps);
        return true;
    }

    private void ProcAllMovements(Vector3 dirVec)
    {
        List<(Puzzle_Element, GridTile)> removeList = new List<(Puzzle_Element, GridTile)>();
        List<(Puzzle_Element, GridTile)> addList = new List<(Puzzle_Element, GridTile)>();
        // Some type of issue here is causing an infinite loop...
        Movable m = puzzleElement.GetComponent<Movable>();
        m.ProcAllMovements(removeList, addList, dirVec);

        foreach ((Puzzle_Element, GridTile) removePair in removeList)
        {
            removePair.Item2.Contents.Remove(removePair.Item1);
        }

        foreach ((Puzzle_Element, GridTile) addPair in addList)
        {
            // Potentially better way would be to have Puzzle Element have a wincondition member...
            WinCondition wc = addPair.Item2.GetComponent<WinCondition>();
            if (wc)
                wc.CheckOneTimeList(addPair.Item1.ElementID);
            addPair.Item2.Contents.Add(addPair.Item1);
            addPair.Item1.CurrentTile = addPair.Item2;
            
            PlayerMovement mv = addPair.Item1.GetComponent<PlayerMovement>();
            if (addPair.Item1.ElementID == 0 && mv)
            {
                ls = addPair.Item2.GetFirstLevelSelector();
            }

            // Assume we never have a win condition on warp or a level selector on warp at the same time...

            for (int a = 0; a < addPair.Item2.Contents.Count; a++)
            {
                PositionWarp posWarp = addPair.Item2.Contents[a].GetComponent<PositionWarp>();
                if (posWarp)
                {
                    queuedWarps.Add((posWarp, addPair.Item1.gameObject));
                    break;
                }
            }
        }
    }

    private void ProcAllMovements(List<(Puzzle_Element, GridTile)> removeList, List<(Puzzle_Element, GridTile)> addList, Vector3 dirVec)
    {
        // Assumption that we have the green light, just move everything on this second pass
        var colliders = Physics2D.OverlapCircleAll(transform.position + dirVec, 0.15f, GameLayers.Instance.TileLayer);

        GridTile tileToCheck = colliders[0].GetComponent<GridTile>();

        //if (recursedTiles.Contains(tileToCheck))
        //    return;

        foreach (Puzzle_Element objElement in tileToCheck.Contents)
        {
            // may need loop protection here
            if (puzzleElement.ElementID >= 0 && objElement.ElementID >= 0 && ElementDB.Instance.GetPushID(puzzleElement.ElementID, objElement.ElementID) == (int)ElementDB.ActionIDs.PushNormal)
            {
                if (puzzleElement.AttachedElements.Contains(objElement))
                    continue;
                objElement.GetComponent<Movable>().ProcAllMovements(removeList, addList, dirVec);
            }
        }

        removeList.Add((puzzleElement, currentTile));
        //currentTile.Contents.Remove(gameObject);
        addList.Add((puzzleElement, tileToCheck));
        //tileToCheck.Contents.Add(gameObject);
        GameManager.Instance.QueuedMoves.Add(gameObject);
        currentTile = tileToCheck;
        StartCoroutine(MoveByAmount(dirVec));

        //Movable frontMovable = front.GetComponent<Movable>();

        /*if (frontMovable)
            frontMovable.ProcAllMovements(recursedTiles, dirVec);*/
    }

    private void RemoveTemps(List<Puzzle_Element> temps)
    {
        for (int i = 0; i < temps.Count; i++)
        {
            //Debug.Log("B " + temps[i].transform.parent.GetComponent<GridTile>().Contents.Count);
            temps[i].transform.parent.GetComponent<GridTile>().Contents.Remove(temps[i]);
            //Debug.Log("A " + temps[i].transform.parent.GetComponent<GridTile>().Contents.Count);
            Destroy(temps[i].gameObject);
        }
    }

    private void OnDestroy()
    {
        // Account for in list?
        GameManager.Instance.QueuedMoves.Remove(gameObject);
    }
}
