using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyDetection : MonoBehaviour
{
    private EnemyController _controller;
    private PlayerController _player;

    [SerializeField] float _detectionRadius = 2;
    [SerializeField] float _attackRadius = 1;

    void Start()
    {
        _controller = GetComponent<EnemyController>();
        _player = _controller.Player;
    }

    void FixedUpdate()
    {
        if (_controller.State == EnemyState.Hit) return;

        Vector3 playerPos = _player.transform.position;
        float playerDistance = Vector3.Distance(transform.position, playerPos);

        if (playerDistance < _attackRadius)
        {
            _controller.ChangeEnemyState(EnemyState.Attacking);
        }
        else if (playerDistance < _detectionRadius)
        {
            _controller.ChangeEnemyState(EnemyState.Alert);
        }
        else
        {
            _controller.ChangeEnemyState(EnemyState.Idle);
        }
    }

    void OnDrawGizmos()
    {
        
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}
