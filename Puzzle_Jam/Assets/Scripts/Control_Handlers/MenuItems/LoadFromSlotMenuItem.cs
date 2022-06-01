using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadFromSlotMenuItem : MonoBehaviour, IMenuItem
{
    [SerializeField]
    private int saveSlot;

    [SerializeField]
    private Text slotText;

    private Color setColor;

    public Color SetColor
    {
        get { return setColor; }
    }
    private void Awake()
    {
        setColor = slotText.color;
    }

    public void MenuSelect()
    {
        if (System.IO.File.Exists(GameSettings.GetSaveFilePath(saveSlot)))
            GameManager.Instance.LoadGame(saveSlot);
    }

    public void UpdateSlotUI()
    {
        if (System.IO.File.Exists(GameSettings.GetSaveFilePath(saveSlot)))
        {
            slotText.text = $"Save Slot {saveSlot}";
        }
        else
        {
            slotText.text = "No Save Data";
            setColor = Color.grey;
        }
    }
}
