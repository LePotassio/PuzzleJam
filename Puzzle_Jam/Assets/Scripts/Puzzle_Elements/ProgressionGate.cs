using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionGate : MonoBehaviour
{
    [SerializeField]
    string gateName = "default";

    [SerializeField]
    private RequiredLevelCompletions requiredLevelCompletions;

    [SerializeField]
    private bool UnlockOnSceneChange = true;

    [SerializeField]
    private CameraState mode = CameraState.Smooth;

    [SerializeField]
    private float camTimeToGate = 3f;

    [SerializeField]
    private float timeAfterUnlockAnimation = 2f;

    [SerializeField]
    private float camTimeFromGate = 3f;

    [SerializeField]
    Puzzle_Element pe;

    [SerializeField]
    CamBox gateCamBox;

    private void Start()
    {
        if (UnlockOnSceneChange)
            DoGateCheck();
        else if (GameManager.Instance.SaveFileProgress.GetGateStatus(gateName) == GateStatus.Unlocked)
            RemoveGateWithoutAnim();
    }
    
    public void DoGateCheck()
    {
        // Check for completions
        if (requiredLevelCompletions.IsSatisfied())
        {
            // Get rid of the barrier (could do first time animation of getting rid of it... adda a cam box and set temporarily?)
            if (GameManager.Instance.SaveFileProgress.GetGateStatus(gateName) == GateStatus.Locked)
            {
                StartCoroutine(RemoveGateWithAnim());
            }
            else
                RemoveGateWithoutAnim();
        }
    }

    private IEnumerator RemoveGateWithAnim()
    {
        GameManager.Instance.WaitingForGate = true;
        yield return new WaitUntil(() => GameManager.Instance.State == GameState.PlayerMove);
        GameManager.Instance.State = GameState.Cutscene;
        GameManager.Instance.WaitingForGate = false;

        GameManager.Instance.CurrentCamBoxes.Add(gateCamBox);
        if (mode == CameraState.Instant)
            GameManager.Instance.RecenterCamera(CameraState.Instant);
        yield return new WaitForSeconds(camTimeToGate);

        pe.RemoveElementFromCurrentTile();
        //gameObject.SetActive(false);
        gameObject.GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);

        yield return new WaitForSeconds(timeAfterUnlockAnimation);

        GameManager.Instance.CurrentCamBoxes.Remove(gateCamBox);
        
        if (mode == CameraState.Instant)
            GameManager.Instance.RecenterCamera(CameraState.Instant);
        yield return new WaitForSeconds(camTimeFromGate);

        GameManager.Instance.SaveFileProgress.SetGateStatus(gateName, GateStatus.Unlocked);

        GameManager.Instance.State = GameState.PlayerMove;
    }

    private void RemoveGateWithoutAnim()
    {
        pe.RemoveElementFromCurrentTile();
        gameObject.GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
    }
}
