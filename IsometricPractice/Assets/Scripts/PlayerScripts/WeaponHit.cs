using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponHit : MonoBehaviour
{
    [SerializeField] int hitDamage = 20;
    [SerializeField] float hitForce = 1f;
    [SerializeField] float stunDuration = 0f;

    public Collider WeaponCollider { get; private set; }

    public bool _isThirdSwing = false;

    void Start()
    {
        WeaponCollider = GetComponent<Collider>();
    }

    public void IsWeaponCollEnabled(bool value)
    {
        WeaponCollider.enabled = value;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyController enemy))
        {
            if (_isThirdSwing)
                enemy.HitByWeapon(hitDamage, transform.position + new Vector3(0, 0.25f, 0), hitForce, stunDuration);
            else
                enemy.PulledByWeapon(hitDamage, transform.position + new Vector3(0, -0.25f, 0), hitForce, stunDuration);
        }
    }
}
