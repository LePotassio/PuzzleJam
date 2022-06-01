using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CutsceneState { Waiting, Busy }

public class Cutscene : MonoBehaviour
{
    // List of images?
    // Text typing?
    // Puzzle Elements scripted movement?
    // Dialog Boxes?

    // We will need a sequential list of these to progress through while state == cutscene
    // Could do something like start a coroutine of all of these and wait for each to finish (and allow player to skip text typing?)

    [SerializeField]
    private GameObject cutsceneCanvas;

    [SerializeField]
    private Image cutsceneSlide;

    [SerializeField]
    private Text cutsceneText;

    [SerializeField]
    private Image cutsceneSlideBackground;

    [SerializeField]
    private string cutsceneName;

    [SerializeField]
    private List<CutsceneStep> steps;

    [SerializeField]
    private CutsceneState state = CutsceneState.Waiting;

    private int currentStep;

    private Action afterCutscene;

    public Image CutsceneSlideBackground
    {
        get { return cutsceneSlideBackground; }
    }

    public Image CutsceneSlide
    {
        get { return cutsceneSlide; }
    }

    public Text CutsceneText
    {
        get { return cutsceneText; }
    }

    public CutsceneState State
    {
        get { return state;}
        set { state = value; }
    }

    public void DoUpdate()
    {
        if (GameSettings.Instance.GetKeyBindingDown(KeyButtons.AdvanceCutscene) && state == CutsceneState.Waiting)
        {
            AdvanceCutscene();
        }
    }

    public void DoCutscene(Action afterCutscene = null)
    {
        if (steps.Count == 0)
        {
            afterCutscene?.Invoke();
            return;
        }
        // Remember to set state to cutscene before
        currentStep = 0;
        cutsceneCanvas.SetActive(true);
        cutsceneText.text = "";

        this.afterCutscene = afterCutscene;

        steps[0].DoCutsceneStep(this);

        /*foreach (CutsceneStep step in steps)
        {
            yield return step.DoCutsceneStep(cutsceneSlide);
            yield return new WaitUntil(() => (GameSettings.Instance.GetKeyBindingDown(KeyButtons.AdvanceCutscene) && GameManager.Instance.State == GameState.Cutscene));
        }*/

        //cutsceneCanvas.SetActive(false);
        //afterCutscene.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
        if (playerMovement && playerMovement == GameManager.Instance.StartingPlayerRef)
        {
            if (GameManager.Instance.SaveFileProgress.GetCutsceneStatus(cutsceneName) == CutsceneStatus.Unwatched)
            {
                GameManager.Instance.StartCutscene(this);
            }
        }
    }

    public void AdvanceCutscene()
    {
        currentStep++;
        if (currentStep >= steps.Count)
        {
            afterCutscene.Invoke();
            cutsceneSlide.sprite = null;
            //state = CutsceneState.Waiting;
            cutsceneCanvas.SetActive(false);
        }
        else
        {
            steps[currentStep].DoCutsceneStep(this);
        }
    }

    public void MarkCutsceneWatched(SaveFileProgress p)
    {
        p.SetCutsceneStatus(cutsceneName, CutsceneStatus.Watched);
    }

    public IEnumerator FadeImgIn(Image img, float speed)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        while (img.color.a < 1.0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + (Time.deltaTime / speed));
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
    }

    /*public IEnumerator FadeImgOut(Image img, float speed)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        while (img.color.a > 0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime / speed));
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
    }

    public IEnumerator FadeTextIn(Text txt, float speed)
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        while (txt.color.a < 1.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime / speed));
            yield return null;
        }
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1.0f);
    }

    public IEnumerator FadeTextOut(Text txt, float speed)
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
        while (txt.color.a > 0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - (Time.deltaTime / speed));
            yield return null;
        }
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);
    }*/

    /*
    public IEnumerator EndCutscene()
    {
        if (cutsceneSlide.im)

        afterCutscene.Invoke();
        cutsceneSlide.sprite = null;
        //state = CutsceneState.Waiting;
        cutsceneCanvas.SetActive(false);
    }*/
}
