using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    [SerializeField]
    List<Text> menuItems;

    int selectedItem;

    public List<Text> MenuItems
    {
        get { return menuItems; }
    }

    private void Awake()
    {
        selectedItem = 0;
        menuItems = new List<Text>();
        menuItems = transform.GetComponentsInChildren<Text>().ToList();
    }

    private void Start()
    {
        //menuItems = transform.GetComponentsInChildren<Text>().ToList();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        foreach (Text slotText in menuItems)
        {
            SaveToSlotMenuItem mi = slotText.GetComponent<SaveToSlotMenuItem>();
            mi?.UpdateSlotUI();
        }
        UpdateItemSelection();
    }

    // Call in gamemanager when state == pausemenu
    public void DoUpdate()
    {
        int prevSelection = selectedItem;

        if (GameSettings.Instance.GetKeyBindingDown(KeyButtons.MenuDown))
        {
            selectedItem++;
        }
        else if (GameSettings.Instance.GetKeyBindingDown(KeyButtons.MenuUp))
        {
            selectedItem--;
        }

        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count - 1);

        if (prevSelection != selectedItem)
        {
            UpdateItemSelection();
        }

        if (GameSettings.Instance.GetKeyBindingDown(KeyButtons.MenuSelect))
        {
            menuItems[selectedItem].GetComponent<IMenuItem>().MenuSelect();
        }
    }

    public void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (i == selectedItem)
            {
                menuItems[i].color = Color.red;
            }
            else
            {
                SaveToSlotMenuItem s = menuItems[i].GetComponent<SaveToSlotMenuItem>(); 
                if (s)
                    menuItems[i].color = s.SetColor;
                else
                    menuItems[i].color = Color.black;
            }
        }
    }
}
