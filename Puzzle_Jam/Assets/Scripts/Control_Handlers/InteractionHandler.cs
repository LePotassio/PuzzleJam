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
        foreach (InteractionReticle reticle in reticles) {
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

    //public enum InteractionIDs { NoInteraction, Pull, Release };
    public Dictionary<InteractionMode, Interaction> Interactions = new Dictionary<InteractionMode, Interaction> {
        { InteractionMode.None, null },
        { InteractionMode.Pull, new Grab() },
        //{ InteractionIDs.Release, new Release() },
    };
}
