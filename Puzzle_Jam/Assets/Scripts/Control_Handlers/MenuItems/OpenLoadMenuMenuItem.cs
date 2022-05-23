using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLoadMenuMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        GameManager.Instance.OpenLoadMenu();
    }
}
