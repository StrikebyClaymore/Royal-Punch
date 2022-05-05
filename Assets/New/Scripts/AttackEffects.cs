using UnityEngine;

namespace New
{
    public class AttackEffects : MonoBehaviour
    {
        [SerializeField] private GameObject _circleArea;
        [SerializeField] private GameObject _conusArea;
        [SerializeField] private ParticleSystem _circleSmoke;
        [SerializeField] private ParticleSystem _conusSmoke;

        public void Play(int type)
        {
            switch (type)
            {
                case 1:
                case 2:
                    _circleSmoke.Play();
                    break;
                case 3:
                    _conusSmoke.Play();
                    break;
            }
        }

        public void AreaSetVisible(int type, bool visible)
        {
            switch (type)
            {
                case 1:
                case 2:
                    _circleArea.SetActive(visible);
                    break;
                case 3:
                    _conusArea.SetActive(visible);
                    break;
            }
        }
    }
}
