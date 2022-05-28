using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBox : MonoBehaviour
{
    [SerializeField]
    private GameObject camPos;

    [SerializeField]
    private int precedence;

    // Camera Size Adjust
    [SerializeField]
    private float camSize = 5f;

    // Option for overriding follow mode and precedence

    public Vector3 CamPos
    {
        get { return new Vector3(camPos.transform.position.x, camPos.transform.position.y, -10) ; }
    }

    public int Precedence
    {
        get { return precedence; }
    }

    public float CamSize
    {
        get { return camSize; }
    }

    private void Awake()
    {
        if (!camPos)
            camPos = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement m = collision.GetComponent<PlayerMovement>();
        if (m && m.StartingPlayer)
        {
            // Recenter camera
            // Debug.Log("Recentering");
            //GameManager.Instance.RecenterCamera(camPos);
            GameManager.Instance.CurrentCamBoxes.Add(this);
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement m = collision.GetComponent<PlayerMovement>();
        if (m && m.StartingPlayer)
        {
            // Recenter camera
            // Debug.Log("Recentering");
            //GameManager.Instance.RecenterCamera(camPos);
            GameManager.Instance.CurrentCamBoxes.Remove(this);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.CurrentCamBoxes.Remove(this);
    }
}
