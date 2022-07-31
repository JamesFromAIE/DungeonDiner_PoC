using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsPool : Singleton<DropsPool>
{
    [SerializeField] EnemyDrop _genericDrop;
    [SerializeField] int _poolSizeAtAwake = 10;
    public Vector3 DropPosition { get ; private set; }

    List<EnemyDrop> _pooledObjects = new List<EnemyDrop>();

    new void Awake()
    {
        base.Awake();

        for (int i = 0; i < _poolSizeAtAwake; i++)
            CreatePooledObject();

        DropPosition = _genericDrop.transform.position;
    }

    public void AddToPool(EnemyDrop drop)
    {
        _pooledObjects.Add(drop);
    }

    public EnemyDrop GetDropObject()
    {
        EnemyDrop dropPoolObject = _pooledObjects.Count == 0
            ? CreatePooledObject() : _pooledObjects[0];

        _pooledObjects.RemoveAt(0);

        dropPoolObject.gameObject.SetActive(true);

        return dropPoolObject;
    }

    EnemyDrop CreatePooledObject()
    {
        EnemyDrop newPoolObject = Instantiate(_genericDrop);
        newPoolObject.transform.parent = transform;

        _pooledObjects.Add(newPoolObject);
        newPoolObject.gameObject.SetActive(false);

        return newPoolObject;
    }
}
