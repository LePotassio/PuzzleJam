using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private string cutsceneName;

    [SerializeField]
    private List<CutsceneStep> steps;

    private int currentStep;

    private Action afterCutscene;

    public void DoUpdate()
    {
        if (GameSettings.Instance.GetKeyBindingDown(KeyButtons.AdvanceCutscene))
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

        this.afterCutscene = afterCutscene;

        StartCoroutine(steps[0].DoCutsceneStep(cutsceneSlide));

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
        Debug.Log(currentStep);
        if (currentStep >= steps.Count)
        {
            afterCutscene.Invoke();
            cutsceneCanvas.SetActive(false);
        }
        else
        {
            StartCoroutine(steps[currentStep].DoCutsceneStep(cutsceneSlide));
        }
    }

    public void MarkCutsceneWatched(SaveFileProgress p)
    {
        p.SetCutsceneStatus(cutsceneName, CutsceneStatus.Watched);
    }
}
