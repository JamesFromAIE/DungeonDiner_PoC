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

    [Header("Combat")]
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _invulnerableTime = 2f;
    [SerializeField] Transform _weapon;
    [SerializeField] float _weaponSpeed = 1f;
    [SerializeField] float _weaponSwingOffset = -45;

    private PlayerAnimator _pAnimator;

    private Quaternion _weaponRotation;
    private float _currentHealth;
    private bool _canDash = true;
    private bool _canAttack = true;
    public bool _isInvulnerable { get; private set; } = false;

    private Vector3 _input;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pAnimator = GetComponent<PlayerAnimator>();
        _currentHealth = _maxHealth;
        _weaponRotation = _weapon.localRotation;
        //_weapon.gameObject.SetActive(false);
    }

    void Update()
    {
        if (/*_canAttack && */Input.GetMouseButtonDown(0))
        {
            //StartCoroutine(SwingWeapon());
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
        if (_input.magnitude > 1) _rb.MovePosition(transform.position + (transform.forward * 1) * _moveSpeed * Time.deltaTime);
        else _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _moveSpeed * Time.deltaTime);
    }

    void ClampDash()
    {
        if (_canDash) return;

        _rb.velocity = _rb.velocity * 0.9f;

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

    IEnumerator SwingWeapon()
    {
        _canAttack = false;
        _weapon.gameObject.SetActive(true);

        Vector3 initialRotation = _weapon.localEulerAngles;

        float normalisedTime = 0;
        while (normalisedTime < 1f)
        {
            _weapon.localRotation = Quaternion.Euler(0, (normalisedTime * 180) + _weaponSwingOffset, initialRotation.z);

            normalisedTime += Time.deltaTime / _weaponSpeed;

            yield return null;
        }

        _weapon.rotation = _weaponRotation;
        _weapon.gameObject.SetActive(false);


        _canAttack = true;
    }
    #endregion


}
