using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsDB : MonoBehaviour
{
    public static Dictionary<InteractionMode, InteractionDetails> interactionModes = new Dictionary<InteractionMode, InteractionDetails>()
    {
        { InteractionMode.None, new InteractionDetails() { interactionName = "Push/Interact" } },
        { InteractionMode.Pull, new InteractionDetails() { interactionName = "Grab" } },
        { InteractionMode.Release, new InteractionDetails() { interactionName = "Release" } }
    };
}

public class InteractionDetails
{
    public string interactionName;
    public Sprite interactionImage;
}
