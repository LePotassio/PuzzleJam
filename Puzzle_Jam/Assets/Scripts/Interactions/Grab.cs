using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : Interaction
{
    public override void DoInteraction(Puzzle_Element attacher1, Puzzle_Element attacher2)
    {
        // Special cases would go here...


        attacher1.AttachedElements.Add(attacher2);
        attacher2.AttachedElements.Add(attacher1);
    }
}
