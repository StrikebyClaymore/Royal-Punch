using UnityEngine;

namespace New
{
    public class Body : MonoBehaviour, IHitable
    {
        public virtual void GetHit(Vector3 hitPoint)
        {
            Debug.Log(name + " GetHit");
        }
    }
}