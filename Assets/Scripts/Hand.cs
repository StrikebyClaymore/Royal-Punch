using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private BaseAttack _attack;
    [SerializeField] private BaseAttack.HandType _type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HitArea>(out var hitArea))
        {
            //Debug.Log(name + " COLLIDE");
            _attack.Attack(hitArea, _type);
        }
    }
}
