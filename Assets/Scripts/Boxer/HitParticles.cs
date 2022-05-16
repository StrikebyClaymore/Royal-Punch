using UnityEngine;

public class HitParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _leftParticles;
    [SerializeField] private ParticleSystem _rightParticles;

    public void StartHitParticles(Hand.HandTypes type)
    {
        if (type == Hand.HandTypes.Left)
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