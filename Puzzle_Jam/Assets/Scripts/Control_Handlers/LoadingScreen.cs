using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public IEnumerator OpenLoadingScreen()
    {
        // Cover the screen here with anim
        // Disable it
        gameObject.SetActive(true);
        yield return null;
    }

    public IEnumerator CloseLoadingScreen()
    {
        // Cover the screen here with anim
        // Disable it
        gameObject.SetActive(false);
        yield return null;
    }
}
