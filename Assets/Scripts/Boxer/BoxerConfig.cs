using UnityEngine;

[CreateAssetMenu(fileName = "BoxerConfig", order = 51)]
public class BoxerConfig : ScriptableObject
{
    public int defaultHealth = 300;
    public int startAddHealth = 60;
    public int upAddHealth = 30;
    
    public int defaultDamage = 10;
    public int startAddDamage = 5;
    public int upAddDamage = 2;

    public float comboDamageMultiplier = 1f;
}