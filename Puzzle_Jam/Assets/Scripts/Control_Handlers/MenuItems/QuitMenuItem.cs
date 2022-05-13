using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuitMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        if (Application.isEditor)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}
