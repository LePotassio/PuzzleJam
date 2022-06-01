using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionGate : MonoBehaviour
{
    [SerializeField]
    private List<string> levelsCompletionsRequired;

    [SerializeField]
    private int requirementLax;

    [SerializeField]
    Puzzle_Element pe;

    private void Start()
    {
        // Check for completions
        int completed = CountRequiredCompletions();
        if (completed == levelsCompletionsRequired.Count - requirementLax)
        {
            // Get rid of the barrier (could do first time animation of getting rid of it... adda a cam box and set temporarily?)
            pe.RemoveElementFromCurrentTile();
            gameObject.SetActive(false);
        }
    }

    private int CountRequiredCompletions()
    {
        SaveFileProgress sf = GameManager.Instance.SaveFileProgress;
        int res = 0;
        foreach (var level in levelsCompletionsRequired)
        {
            if (sf.GetLevelStatus(level) == LevelStatus.Completed)
                res++;
        }
        return res;
    }
}
