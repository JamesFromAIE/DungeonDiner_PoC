using UnityEngine;

[CreateAssetMenu(menuName = "Sound/New Sound Clip", fileName = "SoundClip", order = 1)] // CREATE new Sound Clip in Unity's "Create" menu
public class SoundClip : ScriptableObject
{
    public AudioClip Clip; // Sound to Play when used

    public ClipReference ClipReference; // Enum Ref to sound set to in AudioManager

    [Range(0f, 1f)]
    public float Volume = 1f; // The volume of the AudioSource when used

    [Range(0f, 1f), Tooltip("0 - 2D\n1 - 3D")]
    public float SpatialBlend = 1f; // The conversion from 2D to 3D sound on AudioSource

    public bool Loop = false; // Tick if this sound should loop
}

