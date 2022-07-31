using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] Animator _animator;

    private PlayerController _player;

    private float _lockedTill;
    public int _currentAttack { get; private set; } = 0;

    void Start()
    {
        _player = GetComponent<PlayerController>();
    }

    void Update()
    {
        int state = GetState();

        if (state == _currentState) return;

        _animator.CrossFade(state, 0, 0);
        _currentState = state;
    }

    public void IncrementAttackCount()
    {
        _player.IsThirdSwing(_currentAttack == 2);

        if (_currentAttack >= 3) return;

        _currentAttack++;
        
        
        _player.SetMoveBool(false);
        _player.AttackMove();
    }

    public void ResetAttackCount()
    {
        _currentAttack = 0;
        _player.SetMoveBool(true);
    }

    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        if (_currentAttack == 3) return LockState(Attack3, 0.4f);
        if (_currentAttack == 2) return LockState(Attack2, 0.2f);
        if (_currentAttack == 1) return LockState(Attack1, 0.2f);

        return Idle;



        // Set minimum time for next animation
        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }

        /*
        // Priorities
        if (_attacked) return LockState(Attack, _attackAnimTime);
        if (_player.Crouching) return Crouch;
        if (_landed) return LockState(Land, _landAnimDuration);
        if (_jumpTriggered) return Jump;

        if (_grounded) return _player.Input.x == 0 ? Idle : Walk;
        return _player.Speed.y > 0 ? Jump : Fall;

        */


    }

    #region Cached Properties (Add more as seen fit)

    private int _currentState;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack1 = Animator.StringToHash("Attack1");
    private static readonly int Attack2 = Animator.StringToHash("Attack2");
    private static readonly int Attack3 = Animator.StringToHash("Attack3");

    #endregion 
}
