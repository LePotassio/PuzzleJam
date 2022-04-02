using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjectsSpawner : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance != null)
            return;

        Instantiate(Resources.Load("Prefabs/EssentialObjects"));
    }
}
