using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch;
    public float startTime;
    public float endTime;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
