using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private Transform _parent;
    private float _time;
    public float Time { 
        get => _time;
        set
        {
            _time = value;
            TimeLeft = _time;
        }
        
    }
    public float TimeLeft;
    public bool AutoReset;
    public event Action Action;
    private bool _enabled;
        
    public void Init(Transform parent, float time, Action action, bool autoReset = false, bool enable = false)
    {
        _parent = parent;
        transform.parent = _parent;
        Time = time;
        TimeLeft = _time;
        Action += action;
        AutoReset = autoReset;
        _enabled = enable;
    }
        
    public void Enable() => _enabled = true;

    public void Disable() => _enabled = false;

    public void ResetTime() => TimeLeft = _time;

    private void Update()
    {
        if(_enabled == false)
            return;
        TimeLeft = Mathf.Max(0, TimeLeft - UnityEngine.Time.deltaTime);
        if (TimeLeft == 0)
        {
            Action?.Invoke();
            if (AutoReset)
                TimeLeft = _time;
            else
                Disable();
        }
    }
}