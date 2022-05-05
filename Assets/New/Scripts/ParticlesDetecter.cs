using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesDetecter : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("PC");
    }

    private void OnParticleTrigger()
    {
        Debug.Log("PT");
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("C");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("T");
    }
}
