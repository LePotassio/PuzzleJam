using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpriteSorter : MonoBehaviour
{
    /*
    public float offset;

    public float playerOffset = -.75f;

    public bool showSortPoints = false;

    SpriteRenderer sr;

    SpriteRenderer playerSr;

    GameObject playerObj;


    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        playerObj = GameManager.Instance.PlayerObject;
        playerSr = playerObj.GetComponentInChildren<SpriteRenderer>();
    }


    // Possibly might want to move this to game management update loop, could use observer
    void Update()
    {
        if (showSortPoints)
        {
            float markerLength = .25f;
            Color playerColor = Color.green;
            Color spriteColor = Color.red;

            Debug.DrawLine(new Vector3(playerObj.transform.position.x, playerObj.transform.position.y + playerOffset - markerLength), new Vector3(playerObj.transform.position.x, playerObj.transform.position.y + playerOffset + markerLength), playerColor);
            Debug.DrawLine(new Vector3(playerObj.transform.position.x + markerLength, playerObj.transform.position.y + playerOffset), new Vector3(playerObj.transform.position.x - markerLength, playerObj.transform.position.y + playerOffset), playerColor);

            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + offset - markerLength), new Vector3(transform.position.x, transform.position.y + offset + markerLength), spriteColor);
            Debug.DrawLine(new Vector3(transform.position.x + markerLength, transform.position.y + offset), new Vector3(transform.position.x - markerLength, transform.position.y + offset), spriteColor);
        }

        if (playerObj.transform.position.y + playerOffset > transform.position.y + offset)
        {
            sr.sortingOrder = 10;
        }
        else
        {
            sr.sortingOrder = 0;
        }
    }
    */
}