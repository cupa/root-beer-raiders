using UnityEngine;
using System.Collections;
using System;

public class AudioFade
{
    public static IEnumerator FadeOut(Sound sound, float fadingTime, Func<float, float, float, float> Interpolate, Action Callback = null)
    {
        float startVolume = sound.Source.volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            sound.Source.volume = Interpolate(startVolume, 0, t);
            yield return null;
        }

        sound.Source.volume = 0;
        sound.Source.Stop();
        sound.Source.volume = sound.Volume;
        if (Callback != null)
        {
            Callback();
        }
    }
    public static IEnumerator FadeIn(Sound sound, float fadingTime, Func<float, float, float, float> Interpolate, Action Callback = null)
    {
        sound.Source.Play();
        sound.Source.volume = 0;

        float resultVolume = sound.Volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            sound.Source.volume = Interpolate(0, resultVolume, t);
            yield return null;
        }

        sound.Source.volume = resultVolume;
        if (Callback != null)
        {
            Callback();
        }
    }
}