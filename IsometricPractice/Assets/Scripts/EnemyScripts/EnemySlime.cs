using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyController
{
    [Header("SlimeStats")]
    [SerializeField] float _jumpForce = 2;
    [SerializeField] float _jumpChargeTime = 0.5f;

    protected override void AttackingPlayer()
    {
        if (!_isAttacking)

        StartCoroutine(JumpingAtPlayer());
    }

    IEnumerator JumpingAtPlayer()
    {
        _isAttacking = true;

        Vector3 playerPos = Player.transform.position;

        yield return Helper.GetWait(_jumpChargeTime);

        Vector3 offset = new Vector3(0, 1.5f, 0);
        Vector3 jumpDirection = (playerPos - transform.position) + offset;

        _rb.AddForce(jumpDirection * _jumpForce, ForceMode.VelocityChange);


        float normalisedTime = 0;
        while (normalisedTime < 1f && State == EnemyState.Attacking)
        {
            normalisedTime += Time.deltaTime / _attackCooldown;
            yield return null;
        }


        _isAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player._isInvulnerable) return;

            player.HitByEnemy(_attackDamage);
        }
    }
}
