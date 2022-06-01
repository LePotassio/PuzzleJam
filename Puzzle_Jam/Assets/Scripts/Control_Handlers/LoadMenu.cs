using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonHolder;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private List<Text> menuItems;

    int selectedItem;

    private void Awake()
    {
        selectedItem = 0;
    }

    private void Start()
    {
        menuItems = buttonHolder.GetComponentsInChildren<Text>().ToList();
        OpenMenu();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        foreach (Text slotText in menuItems)
        {
            LoadFromSlotMenuItem mi = slotText.GetComponent<LoadFromSlotMenuItem>();
            mi?.UpdateSlotUI();
        }
        UpdateItemSelection();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

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
            menuItems[selectedItem].GetComponent<IMenuItem>()?.MenuSelect();
        }
    }

    private void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (i == selectedItem)
            {
                menuItems[i].color = Color.red;
            }
            else
            {
                LoadFromSlotMenuItem s = menuItems[i].GetComponent<LoadFromSlotMenuItem>();
                if (s)
                    menuItems[i].color = s.SetColor;
                else
                    menuItems[i].color = Color.black;
            }
        }
    }
}
