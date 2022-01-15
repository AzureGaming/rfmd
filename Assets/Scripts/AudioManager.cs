using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            AttachAudioSource(sound);
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound: " + name + " not found.");
            return;
        }

        sound.source.time = sound.startTime;
        sound.source.Play();
        if (sound.endTime != default)
        {
            sound.source.SetScheduledEndTime(AudioSettings.dspTime + (sound.endTime - sound.startTime));
        }
    }

    void AttachAudioSource(Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;

        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.playOnAwake = false;
    }
}
