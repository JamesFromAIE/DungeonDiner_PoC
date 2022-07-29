using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : EnemyController
{
    [Header("TurretStats")]
    [SerializeField] Vector3 _targetOffset;
    [SerializeField] Transform _barrelTip;
    [SerializeField] EnemyProjectile _projectile;
    [SerializeField] float _bulletSpeed;

    protected override void AttackingPlayer()
    {
        Vector3 targetPosition = (Player.transform.position - _barrelTip.position) + _targetOffset;

        if (Vector3.Distance(targetPosition, transform.position) < 1) return;

        Quaternion lookRotation = Quaternion.LookRotation(targetPosition, Vector3.up);

        transform.rotation = lookRotation;

        //transform.rotation = Quaternion.Euler(lookRotation.x, lookRotation.y, lookRotation.z);

        

        if (!_isAttacking)

            StartCoroutine(ShootingAtPlayer());
    }

    IEnumerator ShootingAtPlayer()
    {
        _isAttacking = true;

        
        float normalisedTime = 0;

        while (normalisedTime < 1f && State == EnemyState.Attacking)
        {
            normalisedTime += Time.deltaTime / _attackCooldown;
            yield return null;
        }
        
        if (normalisedTime >= 1f)
        {
            EnemyProjectile bullet = Instantiate(_projectile, _barrelTip.position, _barrelTip.rotation, null);

            bullet.Init(_attackDamage, _bulletSpeed);
        }

        

        _isAttacking = false;
    }
}
