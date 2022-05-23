using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionModeUI : MonoBehaviour
{
    [SerializeField]
    private Image interactionModeImg;

    [SerializeField]
    private Text interactionModeText;

    [SerializeField]
    private Image interactionSwapIndicator;

    public void SetInteractionUI(InteractionMode mode, bool swapAvailable)
    {
        interactionSwapIndicator.gameObject.SetActive(swapAvailable);


        InteractionDetails details = InteractionsDB.interactionModes[mode];
        interactionModeImg.sprite = details.interactionImage;
        interactionModeText.text = details.interactionName;
    }
}
