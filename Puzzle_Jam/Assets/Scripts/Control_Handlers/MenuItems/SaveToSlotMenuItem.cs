using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveToSlotMenuItem : MonoBehaviour, IMenuItem
{
    [SerializeField]
    private int saveSlot;

    [SerializeField]
    private Text slotText;
    // Eventually, want are you sure prompt

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
        GameManager.Instance.SaveGame(saveSlot);
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
