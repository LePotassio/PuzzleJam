using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemJumpStart : MonoBehaviour
{
    [SerializeField]
    private float initialTime;

    [SerializeField]
    private ParticleSystem ps;

    private void Start()
    {
        ps.Simulate(initialTime);
        ps.Play();
    }
}
