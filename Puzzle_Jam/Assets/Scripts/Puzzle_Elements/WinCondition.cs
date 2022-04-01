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
    

    private GridTile currentTile;

    private void Start()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.15f, GameLayers.Instance.TileLayer);
        if (colliders.Length > 0)
            currentTile = colliders[0].GetComponent<GridTile>();
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
            if (!currentTile.CheckForContentByID(expectedElement))
                return false;
        }

        return true;
    }
}
