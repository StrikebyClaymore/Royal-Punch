using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperAttackChargeEffect : MonoBehaviour
{
    [SerializeField] private Transform _image;
    private bool _isCharging = false;
    private readonly Vector3 _defaultScale = new Vector3(0.05f, 0.05f, 1f);
    private const float MAXChargingScale = 0.32f;
    private float _chargingSpeed = 1.0f;
    [HideInInspector]
    public Player target;

    public Action OnCharged;

    private void Update()
    {
        if(_isCharging == false)
            return;
        Charging();
    }

    public void StartCharging()
    {
        gameObject.SetActive(true);
        _image.localScale = _defaultScale;
        _isCharging = true;
    }

    private void StopCharging()
    {
        _isCharging = false;
        OnCharged?.Invoke();
    }

    private void Charging()
    {
        //Debug.Log($"{transform.localPosition} {transform.localScale * (1f + _chargingSpeed * Time.deltaTime)}");
        _image.localScale *= (1f + _chargingSpeed * Time.deltaTime);
        if (_image.localScale.x >= MAXChargingScale)
            StopCharging();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var _target))
        {
            target = _target;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var _target))
        {
            if (_target == target)
                target = null;
        }
    }
}
