using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseSaveMenuMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        GameManager.Instance.CloseSaveMenu();
    }
}
