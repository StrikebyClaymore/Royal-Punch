using UnityEngine;

namespace New
{
    public class AttackColliders : MonoBehaviour
    {
        [SerializeField] private SphereCollider _circleCollder;
        [SerializeField] private BoxCollider _conusCollder;
        
        public void SetCollider(SuperAttackConfig superAttackConfig)
        {
            switch (superAttackConfig.id)
            {
                case 1:
                case 2:
                    superAttackConfig.collider = _circleCollder;
                    break;
                case 3:
                    superAttackConfig.collider = _conusCollder;
                    break;
            }
        }

        public void SetEnable(int type, bool enable)
        {
            switch (type)
            {
                case 1:
                case 2:
                    _circleCollder.enabled = enable;
                    break;
                case 3:
                    _conusCollder.enabled = enable;
                    break;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent<IHitable>(out var player))
            {
                Debug.Log("TRIGGER");
                _circleCollder.enabled = false;
                _conusCollder.enabled = false;
                player.KnockOut(25000f);
            }
        }
    }
}
