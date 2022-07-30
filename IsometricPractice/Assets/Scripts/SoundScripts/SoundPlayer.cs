using UnityEngine;

// ATTACH THIS SCRIPT TO OBJECTS TO STORE SOUND AND CONTROL WHEN SOUND IS PLAYED AND STOPPED
public class SoundPlayer : MonoBehaviour
{
    public ClipReference Clip; // ENUM REF to chosen SoundClip to play

    public bool PlayOnAwake = false; // BOOL to set whether this sound should play on Start

    SoundObject _objectReference; // REF to SoundObject activated by this SoundPlayer

    void Start()
    {
        // IF BOOL to playOnAwake is ticked in Editor, PLAY SOUNDOBJECT
        if (PlayOnAwake)
            Play();
    }

    public void Play()
    {
        // GET SoundObject used by AudioManager to play sound
        _objectReference = AudioManager.Instance.PlaySoundOnObject(Clip, transform);
    }

    public void Stop()
    {
        // SET SoundObject to STOP PLAYING
        if (_objectReference != null)
            _objectReference.Stop();
    }
}


