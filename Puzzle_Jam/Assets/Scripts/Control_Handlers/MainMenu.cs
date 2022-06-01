using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // New game, load game, options, quit

    [SerializeField]
    private GameObject buttonHolder;

    // Can be replaced with image or menuitem script later
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
        OpenMenu(true);
    }

    public void OpenMenu(bool setupIntro)
    {
        titleText.gameObject.SetActive(true);
        buttonHolder.SetActive(true);
        //GameManager.Instance.MainMenu = this;
        UpdateItemSelection();
        if (setupIntro)
            SetMenuForStartUp();
    }

    public void CloseMenu()
    {
        //gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        buttonHolder.SetActive(false);
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
                menuItems[i].color = Color.black;
            }
        }
    }

    private void SetMenuForStartUp()
    {
        titleText.color = AnimationUtils.GetZeroAlphaColor(titleText.color);

        foreach(Text text in menuItems)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
    }

    public IEnumerator DoStartUpAnim()
    {
        //SetMenuForStartUp();

        yield return AnimationUtils.FadeTextIn(titleText, 1f);

        yield return new WaitForSeconds(1f);

        foreach (Text text in menuItems)
        {
            yield return AnimationUtils.FadeTextIn(text, .5f);
        }
    }
}
