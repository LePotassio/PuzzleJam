using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMainMenuMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        GameManager.Instance.StartMainMenu();
    }
}
