using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float _damage;
    private float _speed;

    void Start() => Destroy(gameObject, 3f);


    void FixedUpdate()
    {
        transform.Translate(transform.forward * _speed, Space.World);
    }

    public EnemyProjectile Init(float newDamage, float moveSpeed)
    {
        _damage = newDamage;
        _speed = moveSpeed / 50;

        return this;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player._isInvulnerable) return;

            player.HitByEnemy(_damage);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

    }

}
