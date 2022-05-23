using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField]
    private List<int> expectedElements;

    
    // Determines if the objective can be one time or must be at the end
    [SerializeField]
    private List<int> oneTimeExpectedElements;

    private Puzzle_Element parentElement;

    private void Start()
    {
        parentElement = GetComponent<Puzzle_Element>();
        GameManager.Instance.CurrentWinConditions.Add(this);
    }

    public void CheckOneTimeList(int elementID)
    {
        if (oneTimeExpectedElements.Contains(elementID))
            oneTimeExpectedElements.Remove(elementID);
    }

    public bool CheckFullfilled()
    {
        if (oneTimeExpectedElements.Count != 0)
            return false;

        foreach (int expectedElement in expectedElements)
        {
            if (!parentElement.CurrentTile.CheckForContentByID(expectedElement))
                return false;
        }

        return true;
    }

    private void OnDestroy()
    {
        GameManager.Instance.CurrentWinConditions.Remove(this);
    }
}
