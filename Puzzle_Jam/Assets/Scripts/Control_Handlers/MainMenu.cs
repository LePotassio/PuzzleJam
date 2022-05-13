using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // New game, load game, options, quit

    [SerializeField]
    GameObject mainMenu;

    // Can be replaced with image or menuitem script later
    List<Text> menuItems;

    int selectedItem;

    private void Awake()
    {
        selectedItem = 0;
        menuItems = new List<Text>();
    }

    private void Start()
    {
        menuItems = mainMenu.GetComponentsInChildren<Text>().ToList();
        OpenMenu();
    }

    public void OpenMenu()
    {
        mainMenu.SetActive(true);
        GameManager.Instance.MainMenu = this;
        UpdateItemSelection();
    }

    public void CloseMenu()
    {
        mainMenu?.SetActive(false);
    }

    public void DoUpdate()
    {
        int prevSelection = selectedItem;

        if (GameSettings.Instance.GetKeyBindingDown(KeyButtons.MenuDown))
        {
            selectedItem++;
        }
        else if(GameSettings.Instance.GetKeyBindingDown(KeyButtons.MenuUp))
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
                menuItems[i].color = Color.black;
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.MainMenu = null;
    }
}
