using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuitMenuItem : MonoBehaviour, IMenuItem
{
    public void MenuSelect()
    {
        // For use in editor only
        /*
        if (Application.isEditor)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
        */
        Application.Quit();
    }
}
