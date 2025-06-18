using UnityEngine;
using UnityEngine.UI;

public class SoundOptionUI : MonoBehaviour
{
    public static SoundOptionUI Instance;

    [Header("Slider UI")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Audio Targets")]
    public AudioSource bgmSource;
    public AudioSource[] sfxSources;
    public AudioSource sfxForSliderMove;

    [Header("Sound Clips")]
    public AudioClip sliderMoveClip;
    public AudioClip levelUpSound;

    [Header("Filter")]
    public AudioHighPassFilter highPassFilter;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadSettings();

        masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    void OnBGMVolumeChanged(float value)
    {
        if (bgmSource != null)
            bgmSource.volume = value;

        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    void OnSFXVolumeChanged(float value)
    {
        foreach (var sfx in sfxSources)
        {
            if (sfx != null)
                sfx.volume = value;
        }

        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void LoadSettings()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterSlider.value = master;
        bgmSlider.value = bgm;
        sfxSlider.value = sfxVolume;

        AudioListener.volume = master;
        if (bgmSource != null) bgmSource.volume = bgm;

        foreach (var source in sfxSources)
        {
            if (source != null)
                source.volume = sfxVolume;
        }
    }

    public void StartSliderSFX()
    {
        if (sfxForSliderMove != null && !sfxForSliderMove.isPlaying)
        {
            sfxForSliderMove.clip = sliderMoveClip;
            sfxForSliderMove.loop = true;
            sfxForSliderMove.Play();
        }
    }

    public void StopSliderSFX()
    {
        if (sfxForSliderMove != null && sfxForSliderMove.isPlaying)
        {
            sfxForSliderMove.Stop();
        }
    }

    // 하이패스 필터 켜기
    public void StartLevelUpEffect()
    {
        if (highPassFilter != null)
            highPassFilter.enabled = true;
    }

    // 하이패스 필터 끄기
    public void StopLevelUpEffect()
    {
        if (highPassFilter != null)
            highPassFilter.enabled = false;
    }

    // 효과음 재생
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        if (sfxForSliderMove != null)
        {
            sfxForSliderMove.PlayOneShot(clip);
        }
        else if (sfxSources.Length > 0 && sfxSources[0] != null)
        {
            sfxSources[0].PlayOneShot(clip);
        }
    }
}
