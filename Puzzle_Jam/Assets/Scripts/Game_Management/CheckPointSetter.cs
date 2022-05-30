using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointSetter : MonoBehaviour
{
    [SerializeField]
    private Vector2 checkPointSpawnLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
        if (playerMovement && playerMovement == GameManager.Instance.StartingPlayerRef)
        {
            SaveFileProgress p = GameManager.Instance.SaveFileProgress;

            p.SetCheckpoint(SceneManager.GetActiveScene().name, checkPointSpawnLocation);
            // could put auto save here plus on level completion...
        }
    }
}
