using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CutsceneStep
{
    [SerializeField]
    private Sprite imageSlide;

    [SerializeField]
    private string slideText;

    /*
    [SerializeField]
    private bool advanceAutomatically = false;
    */

    // FOR NOW: Just do slides...

    // Animation Here (List of sprites)
    // Sprite renderer to animate here -> Make an animation class


    public IEnumerator DoCutsceneStep(Image cutsceneSlide)
    {
        yield return null;
        cutsceneSlide.sprite = imageSlide;
    }
}
