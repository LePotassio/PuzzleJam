using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private LevelWarp levelWarp;

    private SpriteRenderer sr;

    [SerializeField]
    private Sprite newLevelSprite;

    [SerializeField]
    private Sprite startedLevelSprite;

    [SerializeField]
    private Sprite completedLevelSprite;

    public LevelWarp LevelWarp
    {
        get { return levelWarp; }
    }

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        LevelStatus status = GameManager.Instance.SaveFileProgress.GetLevelStatus(levelWarp.SceneToLoad);

        switch (status)
        {
            case LevelStatus.New:
                sr.sprite = newLevelSprite;
                break;
            case LevelStatus.Started:
                sr.sprite = startedLevelSprite;
                break;
            case LevelStatus.Completed:
                sr.sprite = completedLevelSprite;
                break;
        }
    }

    public void ActivateLevelSelect()
    {
        if (!levelWarp.OverrideStartingPosition)
            GameManager.Instance.SwitchLevel(levelWarp.SceneToLoad);
        else
            GameManager.Instance.SwitchLevel(levelWarp.SceneToLoad, levelWarp.SinglePositionOverride);
    }
}
