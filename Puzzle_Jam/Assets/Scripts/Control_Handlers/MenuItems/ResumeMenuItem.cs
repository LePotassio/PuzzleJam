using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        // Exit the pause menu and reset the state
        GameManager.Instance.ResumeGame();
    }
}
