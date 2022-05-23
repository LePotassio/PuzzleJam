using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField]
    private LayerMask tileLayer;

    [SerializeField]
    private LayerMask camBoxLayer;

    public static GameLayers Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public LayerMask TileLayer
    {
        get => tileLayer;
    }

    public LayerMask CamBoxLayer
    {
        get => camBoxLayer;
    }
}
