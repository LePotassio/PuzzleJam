using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutOpener : MonoBehaviour
{
    [SerializeField]
    private ProgressionGate gateToUnlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement m = collision.GetComponent<PlayerMovement>();
        if (m && m.StartingPlayer)
        {
            gateToUnlock.DoGateCheck();
        }
    }
}
