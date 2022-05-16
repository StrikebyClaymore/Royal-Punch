using UnityEngine;

public interface IHitable
{
    void GetHit(Vector3 hitPoint, int damage);

    void KnockOut(float force, int damage);

    bool IsLastHit(int damage);
}