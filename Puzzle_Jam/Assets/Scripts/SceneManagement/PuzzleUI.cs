using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField]
    private Text puzzleNameText;
    [SerializeField]
    private Text puzzleCodeText;
    [SerializeField]
    private Text journalText;

    public void SetSidebarPuzzleInfo(LevelData data)
    {
        puzzleNameText.text = data.PuzzleName;
        puzzleCodeText.text = data.PuzzleCode;
        journalText.text = data.JournalText;
    }
}
