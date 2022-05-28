using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FullPuzzle : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

    [SerializeField]
    private LevelWarp onCompleteWarp;

    [SerializeField]
    private List<InteractionMode> levelStartingInteractionModes = new List<InteractionMode> { InteractionMode.None };

    public LevelWarp OnCompletionWarp
    {
        get { return onCompleteWarp; }
    }

    public LevelData LevelData
    {
        get { return levelData; }
    }

    void Start()
    {
        //GameManager.Instance.RecenterCamera(CameraState.Instant);

        // Set Sidebar UI

        GameManager.Instance.CurrentPuzzle = this;

        // StartCoroutine(GameManager.Instance.CenterCameraAfterLoad());

        GameManager.Instance.SetInteractionLists(levelStartingInteractionModes);
        GameManager.Instance.UpdateInteractionModeUI();
    }
}
