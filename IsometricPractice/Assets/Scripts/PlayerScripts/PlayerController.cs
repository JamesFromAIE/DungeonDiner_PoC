using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _turnSpeed = 15f;
    [SerializeField] float _dashSpeed = 5f;
    [SerializeField] float _dashCooldown = 2f;
    [SerializeField] float _attackMoveSpeed = 2f;

    [Header("Combat")]
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _invulnerableTime = 2f;
    [SerializeField] WeaponHit _weapon;

    private PlayerAnimator _pAnimator;

    private float _currentHealth;
    private bool _canDash = true;
    private bool _canAttack = true;
    private bool _canMove = true;
    public bool _isInvulnerable { get; private set; } = false;

    private Vector3 _input;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pAnimator = GetComponent<PlayerAnimator>();
        _currentHealth = _maxHealth;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _pAnimator.IncrementAttackCount();
        }

        if (_canDash && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }

        GatherInput();
        Look();
    }

    void FixedUpdate()
    {
        Move();
        ClampDash();
    }

    #region Movement
    void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    public void SetMoveBool(bool value)
    {
        _canMove = value;
        _weapon.IsWeaponCollEnabled(!value);
    }

    public void AttackMove()
    {
        _rb.AddForce(transform.forward * _attackMoveSpeed, ForceMode.Impulse);
    }

    void Look()
    {
        if (_input == Vector3.zero) return;

        // Gets the direction towards next movement
        Vector3 relative = (transform.position + _input.ToIso()) - transform.position;
        // Turn direction into a Quaternion
        Quaternion rot = Quaternion.LookRotation(relative, Vector3.up);

        // Set Rotation to movement direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed);
    }

    void Move()
    {
        if (!_canMove) return;

        if (_input.magnitude > 1) _rb.MovePosition(transform.position + (transform.forward * 1) * _moveSpeed * Time.deltaTime);
        else _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _moveSpeed * Time.deltaTime);
    }

    void ClampDash()
    {
        if (!_canDash) _rb.velocity = _rb.velocity * 0.9f; 
        else if (!_canMove) _rb.velocity = _rb.velocity * 0.9f;



        

    }

    IEnumerator Dash()
    {
        _canDash = false;

        _rb.AddForce(transform.forward * _dashSpeed, ForceMode.VelocityChange);

        yield return Helper.GetWait(_dashCooldown);

        /*
        float normalisedTime = 0;
        while (normalisedTime < 1f)
        {
            normalisedTime += Time.deltaTime / _dashCooldown;

            yield return null;
        }
        */

        _canDash = true;
    }
    #endregion

    #region Combat

    public void HitByEnemy(float damage)
    {
        _currentHealth -= damage;

        UIManager.Instance.UpdateHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            UIManager.Instance.DisplayDeadUI();
            this.enabled = false;
        }
        

        StartCoroutine(CountdownInvulnerability());
    }

    public void IsThirdSwing(bool value)
    {
        _weapon._isThirdSwing = value;
    }

    IEnumerator CountdownInvulnerability()
    {
        _isInvulnerable = true;

        yield return Helper.GetWait(_invulnerableTime);

        /*
        float normalisedTime = 0;

        while (normalisedTime < 1f)
        {
            normalisedTime += Time.deltaTime / _invulnerableTime;

            yield return null;
        }
        */

        _isInvulnerable = false;
    }

    #endregion


}
