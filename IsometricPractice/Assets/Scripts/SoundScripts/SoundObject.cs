using System.Collections;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    AudioSource AudSource; // REFERENCE to THIS SoundObject's AudioSource

    void Awake()
    {
        AudSource = gameObject.AddComponent<AudioSource>(); // Create AudioSource component on SoundObject <-- (GameObject)

        AudSource.playOnAwake = false; // DONT play any sounds when created
    }

    void PlayClip(SoundClip sound)
    {
        AudSource.clip = sound.Clip; // Set AudioSource clip

        AudSource.volume = sound.Volume; // Set AudioSource volume
        AudSource.spatialBlend = sound.SpatialBlend; // Set AudioSource spatial blend from 2D to 3D
        AudSource.loop = sound.Loop; // Set AudioSource loop bool

        AudSource.Play(); // Play AudioClip on this AudioSource

        // ------------ COME NO FURTHER IF SOUND IS BEING LOOPED!!! -----------------------

        if (sound.Loop)
            return;

        // Time when sound has concluded, then send SoundObject back into the SoundPool
        StartCoroutine(SendToPool(sound.Clip.length));
    }

    public void Stop()
    {
        AudSource.Stop(); // STOP AudioSource from playing 

        // IF SoundObject isn't already disabled, IMMEDIATELY send back to SoundPool
        if (gameObject.activeSelf)
            StartCoroutine(SendToPool());
    }

    public void PlayClipAtPosition(SoundClip sound, Vector3 position)
    {
        transform.localPosition = position; // SET this SoundObject's position in LocalSpace
        PlayClip(sound); // PLAY Sound on this AudioSource
    }

    // SEND Gameobject back to Pool after time --> (AudioClip duration) has passed
    IEnumerator SendToPool(float time = 0)
    {
        yield return new WaitForSeconds(time); // CONTINUE once enough time has passed

        // RESET SoundObject's parent and position 
        transform.SetParent(SoundPool.Instance.transform);
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        SoundPool.Instance.AddToPool(this); // ADD this SoundObject back into SoundPool to get used later

        gameObject.SetActive(false); // DISABLE this SoundObject (GameObject)
    }
}

