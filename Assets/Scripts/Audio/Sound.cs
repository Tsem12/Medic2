using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum SoundType
    {
        Music,
        Sfx
    }
    public string name;
    public SoundType type;
    public AudioClip clip;

    [Range(0f, 5f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector] public AudioSource source;
}