using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : Singleton<SoundPool>
{
    [SerializeField] int _poolSizeAtAwake = 10; // NUMBER of SoundObjects Created on Awake

    List<SoundObject> _pooledObjects = new List<SoundObject>(); // POOL of Inactive SoundObjects

    new void Awake()
    {
        base.Awake(); // RUN Awake function on Object for this Script

        // INITIALISE new SoundObjects on Awake
        for (int i = 0; i < _poolSizeAtAwake; i++)
            CreatePooledObject();
    }

    // ADD this SoundObject to the SoundPool List
    public void AddToPool(SoundObject soundObject)
    {
        _pooledObjects.Add(soundObject);
    }

    // GET Inactive SoundObject from SoundPool
    public SoundObject GetSoundObject()
    {
        // GET first SoundObject in Pool,
        // IF NO SoundObjects are available, CREATE new SoundObject and GET it instead
        SoundObject soundPoolObject = _pooledObjects.Count == 0
            ? CreatePooledObject() : _pooledObjects[0];

        _pooledObjects.RemoveAt(0); // REMOVE gotten SoundObject from SoundPool

        soundPoolObject.gameObject.SetActive(true); // ENABLE SoundObject to play Sound

        return soundPoolObject; // RETURN gotten SoundObject
    }

    // CREATE NEW SoundObject and ADD it to the SoundPool
    SoundObject CreatePooledObject()
    {
        // CREATE a new GameObject and set its parent to THIS SoundPool Object
        GameObject go = new GameObject("Sound Pool Object");
        go.transform.parent = transform;

        SoundObject poolObject = go.AddComponent<SoundObject>(); // ADD SoundObject component to new GameObject
        _pooledObjects.Add(poolObject); // ADD SoundObject to SoundPool
        poolObject.gameObject.SetActive(false); // DISABLE SoundObject 

        return poolObject; // RETURN created SoundObject
    }
}


