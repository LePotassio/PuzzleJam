using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromSlotMenuItem : MonoBehaviour, IMenuItem
{
    [SerializeField]
    private int saveSlot;
    public void MenuSelect()
    {
        GameManager.Instance.LoadGame(saveSlot);
    }
}
