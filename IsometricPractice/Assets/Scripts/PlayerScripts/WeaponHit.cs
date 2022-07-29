using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponHit : MonoBehaviour
{
    [SerializeField] int hitDamage = 20;
    [SerializeField] float hitForce = 1f;
    [SerializeField] float stunDuration = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyController enemy))
        {
            enemy.HitByWeapon(hitDamage, transform.position + new Vector3(0, 0.25f, 0), hitForce, stunDuration);
        }
    }
}
