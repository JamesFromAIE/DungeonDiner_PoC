using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosHelper : MonoBehaviour
{
    [SerializeField] float _centerSize;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _centerSize);
    }

    
}
