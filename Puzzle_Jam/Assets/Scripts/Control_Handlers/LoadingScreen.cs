using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public IEnumerator OpenLoadingScreen()
    {
        gameObject.SetActive(true);
        yield return null;
    }

    public IEnumerator CloseLoadingScreen()
    {
        gameObject.SetActive(false);
        yield return null;
    }
}
