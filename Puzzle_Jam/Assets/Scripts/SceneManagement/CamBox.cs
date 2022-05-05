using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement m = collision.GetComponent<PlayerMovement>();
        if (m && m.StartingPlayer)
        {
            // Recenter camera
            // Debug.Log("Recentering");
            GameManager.Instance.RecenterCamera(gameObject);
        } 
    }
}
