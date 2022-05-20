using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play (string name)
    {
        Sound s = FindSound(name);

        s.source.Play();
    }

    public void Play(string name, float speed)
    {
        Sound s = FindSound(name);
        s.source.pitch = speed;

        s.source.Play();
    }

    public void PlayOnLoop(string name, float speed) {
        Sound s = FindSound(name);

        if (!s.source.isPlaying)
        {
            s.source.loop = true;
            s.source.pitch = speed;

            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = FindSound(name);

        s.source.Stop();
        s.source.pitch = s.pitch;
    }

    public Sound FindSound(string name) {
        switch (name)
        {
            case "Swing":
                int dice = UnityEngine.Random.Range(0, 2);
                if (dice == 0)
                    return Array.Find(sounds, sound => sound.name == "Swing1");
                else
                    return Array.Find(sounds, sound => sound.name == "Swing2");

            default:
                return Array.Find(sounds, sound => sound.name == name);
        }
    }
}
