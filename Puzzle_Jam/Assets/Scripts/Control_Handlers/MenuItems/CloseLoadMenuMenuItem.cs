using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseLoadMenuMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        GameManager.Instance.CloseLoadMenu();
    }
}
