using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    public Sound FootstepSound;
    public static AudioManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        foreach (var s in Sounds)
        {
            CreateSound(s);
            if (s.PlayOnAwake)
            {
                s.Source.Play();
            }
        }
    }
    public void CreateSound(Sound sound)
    {
        sound.Source = gameObject.AddComponent<AudioSource>();
        sound.Source.clip = sound.Clips[0];
        sound.Source.volume = sound.Volume;
        sound.Source.pitch = sound.Pitch;
        sound.Source.loop = sound.Loop;
        sound.Source.playOnAwake = sound.PlayOnAwake;
    }

    public void PlayOneShot(Sound sound)
    {
        if(sound != null)
        {
            SetSourceProperties(sound);
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.volume = sound.FinalVolume;
            sound.Source.pitch = sound.FinalPitch;
            sound.Source.clip = sound.SelectedClip;
            sound.Source.Play();
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

    private void SetSourceProperties(Sound sound)
    {   
        if (sound != null)
        {
            sound.FinalVolume = UnityEngine.Random.Range(sound.Volume + sound.RandomizeVolume, sound.Volume - sound.RandomizeVolume);
            sound.FinalPitch = UnityEngine.Random.Range(sound.Pitch + sound.RandomizePitch, sound.Pitch - sound.RandomizePitch);
            sound.SelectedClip = sound.Clips[UnityEngine.Random.Range(0, sound.Clips.Length)];
        }


    }

}

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip[] Clips;
    [HideInInspector]
    public AudioClip SelectedClip;
    [Range(0f, 1f)]
    public float Volume = 1;
    [Range(0f, 1f)]
    public float RandomizeVolume = 0;
    [HideInInspector]
    public float FinalVolume;
    [Range(.1f, 3f)]
    public float Pitch = 1;
    [Range(0, 3f)]
    public float RandomizePitch;
    [HideInInspector]
    public float FinalPitch;
    [HideInInspector]
    public bool Loop;
    public bool PlayOnAwake = false;
    [HideInInspector]
    public AudioSource Source;
}
