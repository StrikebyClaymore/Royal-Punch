using UnityEngine;

namespace New
{
    public interface IHitable
    {
        void GetHit(Vector3 hitPoint, int damage);

        void KnockOut(float force);

        bool IsLastHit(int damage);
    }
}