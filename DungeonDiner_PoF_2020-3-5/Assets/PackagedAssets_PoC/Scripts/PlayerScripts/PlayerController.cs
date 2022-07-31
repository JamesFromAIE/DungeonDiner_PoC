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
    public bool _developmentTurnAttack = false;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _invulnerableTime = 2f;
    [SerializeField] WeaponHit _weapon;
    [SerializeField] Material _hitMat;
    private Material _origMat;
    [SerializeField] int _blinkRate = 4;
    private MeshRenderer[] meshes;

    private PlayerInventory _pInventory;
    private PlayerAnimator _pAnimator;

    private float _currentHealth;
    private bool _canDash = true;
    private bool _canAttack = true;
    private bool _canMove = true;
    public bool _isInvulnerable { get; private set; } = false;
    public bool _canPickUp { get; private set; } = true;

    private Vector3 _input;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pAnimator = GetComponent<PlayerAnimator>();
        _pInventory = GetComponent<PlayerInventory>();
        _currentHealth = _maxHealth;

        meshes = GetComponentsInChildren<MeshRenderer>();
        _origMat = meshes[0].material;
    }

    void Update()
    {
        if (_canDash && Input.GetKeyDown(KeyCode.Space))
        {
            Look();
            if (_input != Vector3.zero)
            {
                Vector3 relative = (transform.position + _input.ToIso()) - transform.position;
                StartCoroutine(Dash(relative));
            }
            else StartCoroutine(Dash(transform.forward));
        }

        GatherInput();

        if (Input.GetMouseButtonDown(0))
        {
            if (_developmentTurnAttack)
                MouseLook();
            else Look();

            _pAnimator.IncrementAttackCount();
        }

        if (_developmentTurnAttack)
        {
            if (_pAnimator._currentAttack == 0)
                Look();
        }
        else Look();
            
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
        //if (_developmentTurnAttack) value = true;

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

    Vector3 MouseLook()
    {
        RaycastHit hit;
        Vector3 lookDirection = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, 1 << 10))
        {
            lookDirection = hit.point.FloorV3() - transform.position;

            // Turn direction into a Quaternion
            Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);

            // Set Rotation to movement direction
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * 10);
        }

        return lookDirection;
    }

    void Move()
    {
        if (!_canMove) return;

        if (_input.magnitude > 1) _rb.MovePosition(transform.position + (transform.forward * 1) * _moveSpeed * Time.deltaTime);
        else _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _moveSpeed * Time.deltaTime);
    }

    void ClampDash()
    {
        //if (_isInvulnerable) return;

        if (_pAnimator._currentAttack == 3) _rb.velocity = _rb.velocity * 0.8f;
        else if (_pAnimator._currentAttack > 0) _rb.velocity = _rb.velocity * 0.6f;


        else if (!_canDash) _rb.velocity = _rb.velocity * 0.80f;
        else if (!_canMove) _rb.velocity = _rb.velocity * 0.80f;
        
    }

    IEnumerator Dash(Vector3 dashDirection)
    {
        _canDash = false;

        _pAnimator.ResetAttackCount();

        _rb.AddForce(dashDirection.normalized * _dashSpeed, ForceMode.VelocityChange);

        yield return Helper.GetWait(_dashCooldown);

        _canDash = true;
    }
    #endregion

    #region Combat

    public void HitByEnemy(float damage)
    {
        _currentHealth -= damage;

        UIManager.Instance.UpdateHealth(_currentHealth);

        _pInventory.DropEntireInventory();

        if (_currentHealth <= 0)
        {
            UIManager.Instance.DisplayDeadUI();
            this.enabled = false;
        }

        StartCoroutine(BlinkInvulnerability());
        StartCoroutine(CountdownInvulnerability());
    }

    public void IsThirdSwing(bool value)
    {
        _weapon._isThirdSwing = value;
    }

    IEnumerator BlinkInvulnerability()
    {
        float normalisedTime = 0;

        _canPickUp = false;

        float blinkIntervals = 1f / _blinkRate; // 4 => 0.25f

        Threshold[] thresholdArray = SetThresholdArray(blinkIntervals, _blinkRate);
        bool isBlinkOffset = false;

        while (normalisedTime <= 1f)
        {
            if (normalisedTime < 0.25f) _canPickUp = false;
            else _canPickUp = true;

            bool isOffset = IsThisOffsetEven(thresholdArray, normalisedTime);

            normalisedTime += Time.deltaTime / _invulnerableTime;

            if (isOffset != isBlinkOffset) yield return null;

            isBlinkOffset = isOffset;

            if (isBlinkOffset) SetMaterialInMeshRenderers(_hitMat);
            else SetMaterialInMeshRenderers(_origMat);

            yield return null;
        }

        SetMaterialInMeshRenderers(_origMat);
    }

    bool IsThisOffsetEven(Threshold[] threshArray, float time)
    {
        for (int i = threshArray.Length - 1; i >= 0; i--)
        {
            Threshold thresh = threshArray[i];
            if (time > thresh.Capacity) return thresh.IsOffset; 
        }

        return true;
    }

    Threshold[] SetThresholdArray(float capacity, int rate)
    {
        Threshold[] thresholds = new Threshold[rate]; // 4 => [0, 0, 0, 0]

        for (int i = 0; i < rate; i++)
        {
            Threshold threshold = new Threshold();

            threshold.Init(capacity * i, i % 2 == 0);  // 4 => [0, 0.25, 0.50, 0.75]

            thresholds[i] = threshold;
        }

        return thresholds;
    }

    void SetMaterialInMeshRenderers(Material newMat)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            MeshRenderer mesh = meshes[i];

            mesh.material = newMat;
        }
    }

    IEnumerator CountdownInvulnerability()
    {
        _isInvulnerable = true;

        yield return Helper.GetWait(_invulnerableTime);

        _isInvulnerable = false;
    }

    #endregion


}

public class Threshold
{
    public bool IsOffset { get; private set; }
    public float Capacity { get; private set; }

    public Threshold Init(float capacity, bool offset)
    {
        Capacity = capacity;
        IsOffset = offset;

        return new Threshold();
    }
}
