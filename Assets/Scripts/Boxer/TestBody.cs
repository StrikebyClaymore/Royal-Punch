using UnityEngine;

public class TestBody : MonoBehaviour, IHitable
{
    public void GetHit(Vector3 hitPoint, int damage)
    {
    }

    public void KnockOut(float force, int damage)
    {
    }

    public bool IsLastHit(int damage)
    {
        return true;
    }
}