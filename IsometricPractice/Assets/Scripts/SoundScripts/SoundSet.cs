using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/New Sound Set", fileName = "SoundSet", order = 1)] // CREATE new Sound Set in Unity's "Create" Menu
public class SoundSet : ScriptableObject
{
    public Dictionary<ClipReference, SoundClip> Sounds => _sounds; // PUBLIC GETTER for SoundClip dictionary
    Dictionary<ClipReference, SoundClip> _sounds = new Dictionary<ClipReference, SoundClip>(); // A dictionary to store all game sound upon Start

    public List<SoundClip> Clips = new List<SoundClip>(); // List of SoundClips which are set in Editor

    public void Init()
    {
        _sounds.Clear(); // Clear old list of sounds if any exist

        foreach (SoundClip sound in Clips) // For each SoundClip in the list
            _sounds.Add(sound.ClipReference, sound); // Add the Enum ref and Audioclip to the Dictionary
    }
}

