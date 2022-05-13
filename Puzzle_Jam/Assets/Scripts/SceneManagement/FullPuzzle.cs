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
        GameManager.Instance.RecenterCamera(gameObject);

        // Set Sidebar UI

        GameManager.Instance.CurrentPuzzle = this;
    }
}
