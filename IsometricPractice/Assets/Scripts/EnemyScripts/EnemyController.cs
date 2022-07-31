using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idle = 0,
    Alert = 1,
    Attacking = 2,
    Hit = 3,
}

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    public EnemyState State { get; private set; } = EnemyState.Idle;

    public PlayerController Player { get; private set; }

    protected Rigidbody _rb; // PROTECTED VARIABLE FOR CHILDREN
    protected Vector3 _idlePosition = Vector3.zero;
    protected float _newIdleTimer = 2f;
    protected bool _canGetNewIdle = true;

    [Header("Combat")]
    [SerializeField] protected int _maxHealth;
    protected int _currentHealth;
    [SerializeField] protected int _attackDamage;
    [SerializeField] protected float _attackCooldown; // PROTECTED VARIABLE FOR CHILDREN
    protected bool _isAttacking = false; // PROTECTED VARIABLE FOR CHILDREN
    protected bool _isDead = false;
    private Material _origMat;
    [SerializeField] Material _hitMat;
    [SerializeField] float _hitBlinkTimer = 0.15f;
    [SerializeField] ParticleSystem _deathPEffect;
    [Tooltip("Do NOT ADD more than ONE of the SAME drop in this LIST.")]
    public List<DropProbability> _allDrops = new List<DropProbability>();

    [Header("Movement")]
    [SerializeField] protected float _speed;
    [SerializeField] protected float _turnSpeed;
    public LayerMask _idleAvoidLayers;

    [Header("Sound")]
    [SerializeField] SoundPlayer _hitSound;
    [SerializeField] SoundPlayer _deadSound;

    void Awake()
    {
        Player = FindObjectOfType<PlayerController>();

        _rb = GetComponent<Rigidbody>();
        _idlePosition = transform.position;
        _currentHealth = _maxHealth;
        _origMat = GetComponentInChildren<MeshRenderer>().material;
    }

    void Start()
    {
        
    }

    #region Movement
    void Move(Vector3 destination)
    {
        if (Vector3.Distance(transform.position, destination) < 0.4f) return;

        transform.position = Vector3.Lerp(transform.position, destination, _speed * Time.deltaTime);
    }

    void Look(Vector3 lookDirection)
    {
        // Turn direction into a Quaternion
        Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);

        // Set Rotation to movement direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed);

    }

    void Update()
    {
        if (State == EnemyState.Alert) Look(Player.transform.position);
        else if (State == EnemyState.Idle) Look(_idlePosition);
    }

    void FixedUpdate()
    {
        if (State == EnemyState.Attacking) AttackingPlayer();
        else if (State == EnemyState.Alert) Move(Player.transform.position);
        else if (State == EnemyState.Idle)
        {
            if (_canGetNewIdle && Random.Range(0, 120) == 0)
            {
                _idlePosition = GetNewIdlePosition();
                StartCoroutine(CountdownForNewIdlePosition());
            }

            Move(_idlePosition);
        }
    }

    IEnumerator CountdownForNewIdlePosition()
    {
        _canGetNewIdle = false;

        yield return Helper.GetWait(_newIdleTimer);

        /*
        float normalisedTime = 0;

        while (normalisedTime < 1f)
        {
            normalisedTime += Time.deltaTime / _newIdleTimer;
            yield return null;
        }
        */

        _canGetNewIdle = true;
    }

    Vector3 GetNewIdlePosition()
    {
        Vector3 newPosition = transform.position + (Random.insideUnitCircle * 2f).XYToXZ();

        int iterations = 0;

        while (Vector3.Distance(_idlePosition, newPosition) < 1f || Physics.CheckSphere(newPosition, 0.5f, _idleAvoidLayers))
        {
            newPosition = transform.position + (Random.insideUnitCircle * 2f).XYToXZ();

            iterations++;

            if (iterations > 50) return transform.position;
        }

        return newPosition;
    }
    #endregion

    #region State
    public void ChangeEnemyState(EnemyState newState)
    {
        if (newState == State) return;

        State = newState;

        switch (State)
        {
            case EnemyState.Idle:
                {
                    
                }
                break;
            case EnemyState.Alert:
                {
                    
                }
                break;
            case EnemyState.Attacking:
                {
                    
                }
                break;
            case EnemyState.Hit:
                {
                    
                }
                break;
        }
    }
    #endregion

    #region Combat
    public void HitByWeapon(int damage, Vector3 hitPosition, float force, float stunDuration)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0 && !_isDead)
        {
            KillMe();
        }

        Vector3 hitDirection = transform.position - hitPosition;

        _rb.AddForce(hitDirection * force, ForceMode.Impulse);

        StartCoroutine(CountdownStunFromHit(stunDuration));
        StartCoroutine(FlashHitBlink());
    }

    public void PulledByWeapon(int damage, Vector3 hitPosition, float force, float stunDuration)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0 && !_isDead)
        {
            KillMe();
        }

        Vector3 hitDirection = transform.position - hitPosition;

        _rb.AddForce(hitDirection * force / 5, ForceMode.Impulse);

        StartCoroutine(CountdownStunFromHit(stunDuration));
        StartCoroutine(FlashHitBlink());
        _hitSound?.Play();
    }

    protected virtual void AttackingPlayer()
    {

    }

    IEnumerator CountdownStunFromHit(float stunDuration)
    {
        ChangeEnemyState(EnemyState.Hit);

        yield return Helper.GetWait(stunDuration);

        ChangeEnemyState(EnemyState.Alert);
    }

    IEnumerator FlashHitBlink()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        if (meshes.Length == 0) Debug.LogError("No Mesh Renderers found on " + name);

        SetMaterialInMeshRenderers(meshes, _hitMat);

        yield return Helper.GetWait(_hitBlinkTimer);

        SetMaterialInMeshRenderers(meshes, _origMat);
    }

    void KillMe()
    {
        if (_isDead) return;

        _isDead = true;
        if (_deathPEffect != null) Instantiate(_deathPEffect, transform.position, _deathPEffect.transform.rotation, null);

        DropsManager.Instance.DropLoot(transform.position.FloorV3(), _allDrops);
        UIManager.Instance.UpdateDeadMonsters();
        Destroy(gameObject, 1);

        StopAllCoroutines();
        _rb.constraints = RigidbodyConstraints.None;
        _rb.mass = _rb.mass / 3;
        this.enabled = false;

        _deadSound?.Play();
    }

    #endregion

    #region Visuals

    void SetMaterialsInMeshRenderers(MeshRenderer[] meshes, Material mat)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            MeshRenderer mesh = meshes[i];
            int materialsLength = mesh.materials.Length;
            Material[] matArray = new Material[materialsLength];

            for (int j = 0; j < materialsLength; j++)
            {
                matArray[j] = mat;
            }

            mesh.materials = matArray;
        }
    }

    void SetMaterialInMeshRenderers(MeshRenderer[] meshes, Material mat)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            MeshRenderer mesh = meshes[i];

            mesh.material = mat;
        }
    }

    #endregion


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_idlePosition, 0.5f);
    }

    
}

[System.Serializable]
public class DropProbability
{
    [SerializeField] ScriptableDrops _drop;
    public ScriptableDrops Drop { get { return _drop; } }

    [SerializeField] int _weight;
    public int Weight { get { return _weight; } }


    public DropProbability(ScriptableDrops drop, int weight)
    {
        _drop = drop;
        _weight = weight;
    }
}
