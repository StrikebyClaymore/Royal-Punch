using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{
    public class HitParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _leftParticles;
        [SerializeField] private ParticleSystem _rightParticles;

        public void StartHitParticles(Hand.NewHandTypes type)
        {
            if (type == Hand.NewHandTypes.Left)
                Play(_rightParticles);
            else
                Play(_leftParticles);
        }

        private void Play(ParticleSystem particles)
        {
            particles.Stop();
            particles.Play();
        }
    }

}

