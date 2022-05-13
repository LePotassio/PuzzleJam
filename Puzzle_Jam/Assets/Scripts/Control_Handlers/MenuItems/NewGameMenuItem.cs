using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        // Start a new game

        GameManager.Instance.StartNewGame();
    }
}
