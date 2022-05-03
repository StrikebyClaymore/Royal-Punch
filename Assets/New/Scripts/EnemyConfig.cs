using System.Collections;
using System.Collections.Generic;
using System.Linq;
using New;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", order = 51)]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private SuperAttackConfig[] attacks;

    public SuperAttackConfig GetSuper(int id) => attacks.First(a => a.id == id);
}
