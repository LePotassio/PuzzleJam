using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequiredLevelCompletions
{
    [SerializeField]
    private List<string> levelList;

    [SerializeField]
    private int requirementLax;

    public bool IsSatisfied()
    {
        int completed = CountRequiredCompletions();

        if (completed >= levelList.Count - requirementLax)
        {
            return true;
        }
        return false;
    }

    private int CountRequiredCompletions()
    {
        SaveFileProgress sf = GameManager.Instance.SaveFileProgress;
        int res = 0;
        foreach (var level in levelList)
        {
            if (sf.GetLevelStatus(level) == LevelStatus.Completed)
                res++;
        }
        return res;
    }
}
