using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionMode { None, Pull, Release }

public class InteractionHandler : MonoBehaviour
{
    [SerializeField]
    private List<InteractionReticle> reticles;

    public List<InteractionReticle> Reticles
    {
        get { return reticles; }
    }

    public void DoUpdate()
    {
        foreach (InteractionReticle reticle in reticles)
        {
            if (GameSettings.Instance.GetKeyBindingDown(KeyButtons.SwapInteraction))
            {
                SwapToNextInteractionMode(reticle);
            }

            if (reticle.InteractionMode == InteractionMode.None)
                return;

            if (GameSettings.Instance.GetKeyBindingDown(KeyButtons.Interact))
            {
                foreach (InteractionReticle r in Reticles)
                {
                    if (reticle.InteractionMode == InteractionMode.Release && r.AimerElement.AttachedElements.Count > 1)
                    {
                        // let go!
                        List<(Puzzle_Element, Puzzle_Element)> removeList = new List<(Puzzle_Element, Puzzle_Element)>();
                        foreach (Puzzle_Element e in r.AimerElement.AttachedElements)
                        {
                            if (e != r.AimerElement)
                            {
                                removeList.Add((r.AimerElement, e));
                            }
                        }
                        foreach ((Puzzle_Element, Puzzle_Element) removePair in removeList)
                        {
                            removePair.Item1.AttachedElements.Remove(removePair.Item2);
                            removePair.Item2.AttachedElements.Remove(removePair.Item1);
                        }
                        reticle.InteractionMode = InteractionMode.Pull;
                    }
                    else
                    {
                        GridTile reticleCurrentTile = r.CurrentTile;
                        //Debug.Log(reticleCurrentTile.Contents.Count);
                        foreach (Puzzle_Element elem in reticleCurrentTile.Contents)
                        {
                            if (reticle.InteractionMode == InteractionMode.Pull)
                            {
                                if (ElementDB.Instance.GetCanPull(r.AimerElement.ElementID, elem.ElementID))
                                {
                                    Interactions[InteractionMode.Pull].DoInteraction(r.AimerElement, elem);
                                    reticle.InteractionMode = InteractionMode.Release;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void ClearInteractionHandler()
    {
        reticles = new List<InteractionReticle>();
    }

    public void SwapToNextInteractionMode(InteractionReticle r)
    {
        InteractionReticle originalPlayerReticle = GameManager.Instance.StartingPlayerRef.GetComponentInChildren<InteractionReticle>();
        if (originalPlayerReticle.InteractionMode == InteractionMode.Release)
            return;
        if (r.InteractionMode == InteractionMode.Release)
            return;
        r.CurrentInteraction++;
        if (r.CurrentInteraction > r.AvailableInteractions.Count - 1)
            r.CurrentInteraction = 0;
        r.InteractionMode = r.AvailableInteractions[r.CurrentInteraction];
        // Conundrum, UI shows original player's interactrion mode but for multiple? (maybe just drop the idea of differing interactions, Compromise your idea!)
        if (r == originalPlayerReticle)
            GameManager.Instance.UpdateInteractionModeUI(originalPlayerReticle);
    }

    //public enum InteractionIDs { NoInteraction, Pull, Release }; May want to simplify this as class separation really is not useful here...
    public Dictionary<InteractionMode, Interaction> Interactions = new Dictionary<InteractionMode, Interaction> {
        { InteractionMode.None, null },
        { InteractionMode.Pull, new Grab() },
        //{ InteractionIDs.Release, new Release() },
    };
}
