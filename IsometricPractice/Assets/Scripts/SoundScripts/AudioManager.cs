using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CREATE new Enum for each Sound, broken up with a unique index
public enum ClipReference
{
    Slime_Hit = 0,
    Slime_Dead = 1,
    Example_Eleventh = 11,

    Example_NewCategory = 100,
    Example_NewCategorySecond = 101,
}

public class AudioManager : Singleton<AudioManager>
{
    public SoundSet SoundSet; // Full set of Sounds used in game

    new void Awake()
    {
        base.Awake(); // RUN Awake function on Object for this Script

        // STOP EVERYTHING if there is no SoundSet set in Editor
        if (SoundSet == null) throw new System.Exception("Sound Set is null");

        SoundSet.Init(); // INITIALISE Sound Dictionary to grab AudioClips later
    }

    // PLAY SoundObject's AudioSource at position in WorldSpace
    public SoundObject PlaySoundAtPosition(ClipReference clip, Vector3 position)
    {
        SoundClip sound = SoundSet.Sounds[clip]; // GET AudioClip from Enum Reference in SoundObject

        //----- IF there is no AudioClip on SoundObject, PRINT ERROR and DONT play AudioSource
        if (sound == null)
        {
            Debug.LogError("NO SOUND ON " + clip + "REFERENCE!!!");
            return null;
        }
        
        SoundObject poolObj = SoundPool.Instance.GetSoundObject(); // GET SoundObject from SoundPool
        poolObj.transform.SetParent(null); // UNPARENT SoundObject from SoundPool Object
        poolObj.PlayClipAtPosition(sound, position); // PLAY AudioClip at WORLD SPACE position

        return poolObj; // RETURN gotten SoundObject
    }

    // PLAY SoundObject's AudioSource on an Object's Transform
    public SoundObject PlaySoundOnObject(ClipReference clip, Transform target)
    {
        SoundClip sound = SoundSet.Sounds[clip]; // GET AudioClip from Enum Reference in SoundObject

        //----- IF there is no AudioClip on SoundObject, PRINT ERROR and DONT play AudioSource
        if (sound == null)
        {
            Debug.LogError("NO SOUND ON " + clip + "REFERENCE!!!");
            return null;
        }

        SoundObject poolObj = SoundPool.Instance.GetSoundObject(); // GET SoundObject from SoundPool
        poolObj.transform.SetParent(target); // SET SoundObject's Parent to targeted Transform
        poolObj.PlayClipAtPosition(sound, Vector3.zero); // PLAY AudioClip on PARENT TRANSFORM

        return poolObj; // RETURN gotten SoundObject
    }
}


