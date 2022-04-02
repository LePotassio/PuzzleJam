using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FullPuzzle : MonoBehaviour
{
    [SerializeField]
    private LevelWarp onCompleteWarp;

    public LevelWarp OnCompletionWarp
    {
        get { return onCompleteWarp; }
    }

    void Start()
    {
        GameManager.Instance.RecenterCamera(gameObject);
        GameManager.Instance.CurrentPuzzle = this;
    }
}
