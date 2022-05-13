using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField]
    private string puzzleName = "Unamed";
    [SerializeField]
    private string puzzleCode = "No Code";
    [SerializeField]
    private string journalText;

    public string PuzzleName
    {
        get { return puzzleName; }
    }

    public string PuzzleCode
    {
        get { return puzzleCode; }
    }

    public string JournalText
    {
        get { return journalText; }
    }
}
