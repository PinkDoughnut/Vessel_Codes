using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioHighPassFilter bgmEffect;

    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    public AudioSource bgmSource;
    public List<AudioSource> sfxSources;

    private void Awake()
    {
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        ApplyVolumes();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        ApplyVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        ApplyVolumes();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource sfx = GetAvailableSFXSource();
        sfx.volume = masterVolume * sfxVolume;
        sfx.PlayOneShot(clip);
    }

    AudioSource GetAvailableSFXSource()
    {
        foreach (var sfx in sfxSources)
        {
            if (!sfx.isPlaying)
                return sfx;
        }
        return sfxSources[0];
    }

    private void ApplyVolumes()
    {
        if (bgmSource != null)
            bgmSource.volume = masterVolume * bgmVolume;

        foreach (var sfx in sfxSources)
        {
            if (sfx != null)
                sfx.volume = masterVolume * sfxVolume;
        }
    }

    public void SetHighPassFilter(bool enabled, float cutoff = 3000f)
    {
        if (bgmEffect != null)
        {
            bgmEffect.enabled = enabled;
            bgmEffect.cutoffFrequency = cutoff;
        }
    }


}
