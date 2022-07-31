using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private ParticleSystem _ps;


    void Start()
    {
        _ps = GetComponent<ParticleSystem>();  
    }

    public void FixedUpdate()
    {
        if (_ps && !_ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
