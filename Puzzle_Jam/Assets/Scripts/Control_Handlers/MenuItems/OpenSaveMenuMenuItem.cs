using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSaveMenuMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        GameManager.Instance.OpenSaveMenu();
    }
}
