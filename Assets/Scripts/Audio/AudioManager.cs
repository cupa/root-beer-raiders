using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    public static AudioManager Instance;
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        //else
        //{
        //    Destroy(gameObject);
        //}
        foreach (var s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
            s.Source.playOnAwake = s.PlayOnAwake;
            if (s.PlayOnAwake)
            {
                s.Source.Play();
            }
        }
    }

    public void Play(string Name)
    {
        var sound = GetSound(Name);
        sound.Source.Play();
    }

    public void Stop(string Name)
    {
        var sound = GetSound(Name);
        sound.Source.Stop();
    }

    public bool IsPlaying(string Name)
    {
        var sound = GetSound(Name);
        return sound.Source.isPlaying;
    }

    public Sound GetSound(string Name)
    {
        var sound = Sounds.FirstOrDefault(s => s.Name == Name);
        if (sound == null)
        {
            Debug.LogWarning("Sound \"" + Name + "\" not found");
            return null;
        }
        return sound;
    }

    public void FadeIn(string Name, float FadeInTime = 3f, Action Callback = null)
    {
        var sound = GetSound(Name);
        StartCoroutine(AudioFade.FadeIn(sound, FadeInTime, Mathf.SmoothStep, Callback));
    }

    public void FadeOut(string Name, float FadeOutTime = 3f, Action Callback = null)
    {
        var sound = GetSound(Name);
        StartCoroutine(AudioFade.FadeOut(sound, FadeOutTime, Mathf.SmoothStep, Callback));
    }
}

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume;
    [Range(.1f, 3f)]
    public float Pitch;
    public bool Loop;
    public bool PlayOnAwake = false;
    [HideInInspector]
    public AudioSource Source;
}
