using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private Image screenBlock;

    public IEnumerator OpenLoadingScreen()
    {
        // Cover the screen here with anim
        // Disable it
        gameObject.SetActive(true);
        yield return AnimationUtils.FadeImgIn(screenBlock, GameSettings.TransitionFadeSpeed);
    }

    public IEnumerator CloseLoadingScreen()
    {
        // Cover the screen here with anim
        // Disable it
        yield return AnimationUtils.FadeImgOut(screenBlock, GameSettings.TransitionFadeSpeed);
        gameObject.SetActive(false);
    }
}
