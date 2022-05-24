using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyAttack : BaseAttack
{
    private Enemy _boxer;
    private Transform _player;
    private SuperAttack[] _superAttacks;
    private SuperAttack _currentSuperAttack;
    private Timer _superAttackTimer;
    private int _currentSuperAttackIdx = 0;
    [SerializeField] private float _SuperAttackCooldownTime = 4f;
    [SerializeField] private float _SuperAttackChargeTime = 1f;
    [SerializeField] private float _rotationSpeed = 1f;
    private float _animationPercentPassed;
    private const int SuperAttackLayer = 0;
    
    public enum SuperStates
    {
        None = 0,
        Start = 1,
        Charge = 2,
        Continue = 3,
        Attack = 4,
        AttackEnd = 5,
        Tried = 6
    }
    private SuperStates _superState = SuperStates.None;

    protected override void Awake()
    {
        base.Awake();

        _boxer = (boxer as Enemy);
        
        _superAttackTimer = gameObject.AddComponent<Timer>();
        _superAttackTimer.Init(transform, _SuperAttackCooldownTime, SuperAttackTimerTimeOut);

        _superAttacks = GetComponentsInChildren<SuperAttack>();
    }

    private void Start()
    {
        ConnectActions();
        _player = GameManager.Player.transform;
    }

    private void FixedUpdate()
    {
        if (_superState == SuperStates.Charge || _superState == SuperStates.Attack)
        {
            _currentSuperAttack.Process(_superState);
        }
    }

    private void LateUpdate()
    {
        if(_superState != SuperStates.None)
            return;
        Rotate();
    }

    public void CancelSuperAttack()
    {
        ChangeSuperState(SuperStates.None);
        _superAttackTimer.Disable();
        if(_currentSuperAttack)
            _currentSuperAttack.SetActive(false);
    }

    public void StartChargeSuper(float animationPercentPassed)
    {
        ChangeSuperState(SuperStates.Charge);
        boxer.animationSystem.SetSpeed(0);
        boxer.animationSystem.OnAnimationCompleted += SuperAttackEnd;
        _animationPercentPassed = animationPercentPassed;
        _currentSuperAttack.SetActive(true);
        _superAttackTimer.Enable();
    }

    protected override void StartBattle()
    {
        base.StartBattle();
        _superAttackTimer.Enable();
    }

    protected internal override void SetDamage(int value)
    {
        base.SetDamage(value);
        foreach (var super in _superAttacks)
        {
            super.SetDamage(value);
        }
    }

    private void StartSuperAttackPressed(int id)
    {
        _currentSuperAttack = GetSuper(id);
        StartSuperAttack();
    }
    
    private void StartSuperAttack()
    {
        ChangeSuperState(SuperStates.Start);
        boxer.animationSystem.StopPunch();
        _superAttackTimer.Time = _currentSuperAttack.chargingTime;
        _currentSuperAttack.OnAttack += SuperAttack;
        (boxer.animationSystem as EnemyAnimation)?.StartSuper(_currentSuperAttack.id);
    }
    
    private void SuperAttackTimerTimeOut()
    {
        if (_superState == SuperStates.None)
        {
            _currentSuperAttack = _superAttacks[_currentSuperAttackIdx];
            _currentSuperAttackIdx++;
            if (_currentSuperAttackIdx >= _superAttacks.Length)
                _currentSuperAttackIdx = 0;
            StartSuperAttack();
        }
        else if (_superState == SuperStates.Charge)
        {
            StartCoroutine(SuperAttackContinue());
            boxer.animationSystem.SetSpeed(1);
            _superAttackTimer.ResetTime();
        }
    }

    private IEnumerator SuperAttackContinue()
    {
        ChangeSuperState(SuperStates.Continue);
        yield return new WaitForEndOfFrame();
        _boxer.animationRigging.SetWeight(0);
        boxer.animationSystem.AddAnimationCompletedEvent(SuperAttackLayer, _animationPercentPassed);
    }

    private void ApplySuperAttack()
    {
        ChangeSuperState(SuperStates.Attack);
        _currentSuperAttack.SetColliderActive();
    }

    private void SuperAttack()
    {
        ChangeSuperState(SuperStates.AttackEnd);
        _boxer.animationRigging.SetWeight(1);
    }

    private void SuperAttackEnd()
    {
        ChangeSuperState(SuperStates.Tried);
        _boxer.animationRigging.ToggleTried(true);
        _animationPercentPassed = 0;
        boxer.animationSystem.OnAnimationCompleted -= SuperAttackEnd;
        _currentSuperAttack.OnAttack -= SuperAttack;
        StartCoroutine(TriedContinue());
    }

    private IEnumerator TriedContinue()
    {
        yield return new WaitForEndOfFrame();
        boxer.animationSystem.OnAnimationCompleted += TriedEnd;
        boxer.animationSystem.AddAnimationCompletedEvent();
    }
        
    private void TriedEnd()
    {
        ChangeSuperState(SuperStates.None);
        _boxer.animationRigging.ToggleTried(false);
        boxer.animationSystem.OnAnimationCompleted -= TriedEnd;
        _superAttackTimer.Time = _SuperAttackCooldownTime;
        _superAttackTimer.Enable();
    }
    private void PlayerStandUp()
    {
        attackRangeDetector.CastTrigger();
    }


    private void ChangeSuperState(SuperStates newState) => _superState = newState;
    
    private void Rotate()
    {
        var relativePos = _player.position - transform.position;
        var targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        var rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = rotation;
    }

    private SuperAttack GetSuper(int id) => _superAttacks.First(a => a.id == id);

    protected override void ConnectActions()
    {
        base.ConnectActions();
        ((PlayerRagdollSystem) GameManager.Player.ragdollSystem).OnStandUp += PlayerStandUp;
    }

    public void Lock()
    {
        _boxer.Lock();
    }
}