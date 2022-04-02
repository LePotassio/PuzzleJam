using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private LevelWarp levelWarp;

    public LevelWarp LevelWarp
    {
        get { return levelWarp; }
    }

    public void ActivateLevelSelect()
    {
        GameManager.Instance.SwitchLevel(levelWarp.SceneToLoad);
    }
}
