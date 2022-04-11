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

    public Action Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return null;
        }

        sound.source.time = sound.startTime;
        if (sound.loop)
        {
            Coroutine loopRoutine = StartCoroutine(PlayLoop(sound));
            return () =>
            {
                sound.source.Stop();
                StopCoroutine(loopRoutine);
            };
        }
        else
        {
            sound.source.Play();

            if (sound.endTime != default)
            {
                sound.source.SetScheduledEndTime(AudioSettings.dspTime + (sound.endTime - sound.startTime));
            }
            return null;
        }
    }

    public void Play(string name, Vector3 position)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        sound.source.time = sound.startTime;

        AudioSource.PlayClipAtPoint(sound.clip, position);

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

    IEnumerator PlayLoop(Sound sound)
    {
        for (; ; )
        {
            sound.source.Play();

            if (sound.endTime != default)
            {
                sound.source.SetScheduledEndTime(AudioSettings.dspTime + (sound.endTime - sound.startTime));
            }
            yield return new WaitUntil(() => !sound.source.isPlaying);
        }
    }
}
