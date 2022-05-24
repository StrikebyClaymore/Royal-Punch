using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaluteEffect : MonoBehaviour
{
    private ParticleSystem[] _salutes;
    
    private void Awake()
    {
        GameManager.SaluteEffect = this;
        _salutes = GetComponentsInChildren<ParticleSystem>();
    }

    public void StartSalute()
    {
        foreach (var salute in _salutes)
        {
            salute.Play();
        }
    }
}
