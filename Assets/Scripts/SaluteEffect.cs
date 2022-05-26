using UnityEngine;

public class SaluteEffect : MonoBehaviour
{
    private ParticleSystem[] _salutes;
    private Timer _timer;
    [SerializeField] private float saluteCooldown = 1.0f;

    private void Awake()
    {
        GameManager.SaluteEffect = this;
        _salutes = GetComponentsInChildren<ParticleSystem>();

        _timer = gameObject.AddComponent<Timer>();
        _timer.Init(transform, saluteCooldown, PlaySalute, true);
    }

    public void StartSalute()
    {
        _timer.Enable();
        PlaySalute();
    }

    private void PlaySalute()
    {
        foreach (var salute in _salutes)
        {
            salute.Play();
        }
    }
}
