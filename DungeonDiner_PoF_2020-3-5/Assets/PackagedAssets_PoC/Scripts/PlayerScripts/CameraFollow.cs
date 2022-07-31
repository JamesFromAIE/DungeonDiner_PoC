using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _followSpeed;

    void FixedUpdate()
    {
        Vector3 playerPos = _playerTransform.position;

        if (Vector3.Distance(playerPos, transform.position) < 0.05f) return;

        transform.position = Vector3.Lerp(transform.position, playerPos, _followSpeed * Time.deltaTime);
    }
}
