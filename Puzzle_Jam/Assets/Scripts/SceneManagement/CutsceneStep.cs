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

    [SerializeField]
    private bool slideBackgoundEnabled = true;

    [SerializeField]
    private bool autoAdvance = false;

    /*
    [SerializeField]
    private bool advanceAutomatically = false;
    */

    // FOR NOW: Just do slides...

    // Animation Here (List of sprites)
    // Sprite renderer to animate here -> Make an animation class

    private bool doingSlide = false;
    private bool doingBackgound = false;
    private bool doingText = false;

    public void DoCutsceneStep(Cutscene cutscene)
    {
        cutscene.State = CutsceneState.Busy;
        cutscene.StartCoroutine(DoCutsceneStepAsync(cutscene));
    }

    public IEnumerator DoCutsceneStepAsync(Cutscene cutscene)
    {
        doingSlide = true;
        if ((cutscene.CutsceneSlideBackground.color.a == 0f && slideBackgoundEnabled) || (cutscene.CutsceneSlideBackground.color.a >= 1.0f && !slideBackgoundEnabled))
        {
            doingBackgound = true;
            cutscene.StartCoroutine(ToggleSlideBackground(cutscene));
        }
        if (cutscene.CutsceneText.text != slideText)
        {
            doingText = true;
            cutscene.StartCoroutine(ChangeText(cutscene));
        }

        cutscene.StartCoroutine(DoCutsceneStepSlide(cutscene));

        // wait for all substeps to finish before switching to state for player skip...
        yield return new WaitUntil(() => !doingSlide && !doingBackgound && !doingText);
        if (!autoAdvance)
            cutscene.State = CutsceneState.Waiting;
        else
            cutscene.AdvanceCutscene();
    }

    public IEnumerator DoCutsceneStepSlide(Cutscene cutscene)
    {
        /*if (cutscene.CutsceneSlide.sprite == null && imageSlide == null)
            yield break;
        */

        // Fade out (If there was a sprite)
        if (cutscene.CutsceneSlide.sprite != null)
            yield return AnimationUtils.FadeImgOut(cutscene.CutsceneSlide, GameSettings.CutsceneSlideFadeSpeed);

        cutscene.CutsceneSlide.sprite = imageSlide;

        // Fade in (If there is a sprite)
        if (cutscene.CutsceneSlide.sprite != null)
            yield return AnimationUtils.FadeImgIn(cutscene.CutsceneSlide, GameSettings.CutsceneSlideFadeSpeed);

        doingSlide = false;
    }

    public IEnumerator ToggleSlideBackground(Cutscene cutscene)
    {
        if (slideBackgoundEnabled)
            yield return AnimationUtils.FadeImgIn(cutscene.CutsceneSlideBackground, GameSettings.CutsceneSlideFadeSpeed);
        else
            yield return AnimationUtils.FadeImgOut(cutscene.CutsceneSlideBackground, GameSettings.CutsceneSlideFadeSpeed);
        doingBackgound = false;
    }

    public IEnumerator ChangeText(Cutscene cutscene)
    {
        if (cutscene.CutsceneText.text != "")
        {
            yield return AnimationUtils.FadeTextOut(cutscene.CutsceneText, GameSettings.CutsceneSlideFadeSpeed);
        }

        cutscene.CutsceneText.text = slideText;

        if (cutscene.CutsceneText.text != "")
        {
            yield return AnimationUtils.FadeTextIn(cutscene.CutsceneText, GameSettings.CutsceneSlideFadeSpeed);
        }
        


        doingText = false;
    }
}
