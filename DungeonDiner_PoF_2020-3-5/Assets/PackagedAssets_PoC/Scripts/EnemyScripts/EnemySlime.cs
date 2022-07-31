using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyController
{
    [Header("SlimeStats")]
    [SerializeField] float _jumpForce = 2;
    [SerializeField] float _jumpChargeTime = 0.5f;

    [SerializeField] MeshRenderer _mesh;
    private Vector3 _meshLocation;

    void Start()
    {
        _meshLocation = _mesh.transform.localPosition;
    }

    protected override void AttackingPlayer()
    {
        if (!_isAttacking)
            StartCoroutine(JumpingAtPlayer());
    }

    IEnumerator JumpingAtPlayer()
    {
        _isAttacking = true;

        Vector3 playerPos = Player.transform.position;

        float normalisedTime = 0;

        int iterations = 0;
        bool isOffset = false;

        while (normalisedTime < 1f && State == EnemyState.Attacking)
        {
            iterations++;

            if (iterations % 3 == 0) isOffset = !isOffset;

            if (isOffset) _mesh.transform.localPosition = _meshLocation + new Vector3(0.03f, 0, 0);
            else _mesh.transform.localPosition = _meshLocation + new Vector3(-0.03f, 0, 0);

            transform.localScale = new Vector3(1, 1 - (normalisedTime / 2), 1);

            normalisedTime += Time.deltaTime / _jumpChargeTime;
            yield return null;
        }

        transform.localScale = Vector3.one;
        _mesh.transform.localPosition = _meshLocation;

        Vector3 offset = new Vector3(0, 1.5f, 0);
        Vector3 jumpDirection = ((playerPos - transform.position) + offset).normalized;

        _rb.AddForce(jumpDirection * _jumpForce, ForceMode.VelocityChange);


        normalisedTime = 0;
        while (normalisedTime < 1f && State == EnemyState.Attacking)
        {
            normalisedTime += Time.deltaTime / _attackCooldown;
            yield return null;
        }

        transform.localScale = Vector3.one;
        _isAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player._isInvulnerable || State == EnemyState.Hit || _isDead) return;

            player.HitByEnemy(_attackDamage);
        }
    }
}
