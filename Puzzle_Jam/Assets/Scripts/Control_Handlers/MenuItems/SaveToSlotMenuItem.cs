using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveToSlotMenuItem : MonoBehaviour, IMenuItem
{
    [SerializeField]
    private int saveSlot;
    // Eventually, want are you sure prompt
    public void MenuSelect()
    {
        GameManager.Instance.SaveGame(saveSlot);
    }
}
