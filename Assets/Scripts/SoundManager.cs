using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

/// <summary>
/// My own sound manager for games (Canvas UI)
/// </summary>
public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public List<AudioSource> soundSources;

    public Slider musicSlider;

    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

    void Start()
    {
        // Load volume settings from file
        LoadVolumeSettings();
    }

    // set background music
    public void PlayBackgroundMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void PlayBackgroundMusic(AudioClip clip, float delay)
    {
        musicSource.clip = clip;
        musicSource.PlayDelayed(delay);
    }

    // play single sound
    public void PlaySingle(AudioClip clip)
    {
        foreach(AudioSource source in soundSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                break;
            }
        } // foreach
    }
    // playing single sound with decreasing bg sound for N secs
    public void PlaySingle(AudioClip clip, bool effectSound, float decreaseTime)
    {
        bool state = false;
        foreach (AudioSource source in soundSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                state = true;
                break;
            }
        }

        // check for done action
        if (state)
        {
            musicSource.volume /= 2;
            StartCoroutine(TweenAudioVolume(musicSource, musicSlider.value, decreaseTime));
        }
    }
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randIndex = Random.Range(0, clips.Length);
        float randPitch = Random.Range(lowPitch, highPitch);

        foreach (AudioSource source in soundSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clips[randIndex];
                source.pitch = randPitch;
                source.Play();
                break;
            }
        } // for i
    }
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);

        musicSource.volume = volume;
        soundSources.ForEach((t) => t.volume = volume);
    }
    public void SetMusicVolume(Slider slider)
    {
        float volume = slider.value;
        PlayerPrefs.SetFloat("MusicVolume", volume);

        musicSource.volume = volume;
        soundSources.ForEach((t) => t.volume = volume);
    }
    public void SetBackgroundVolume(float from, float to, float duration, AnimationCurve curve, bool autoStop)
    {
        Tween.Volume(musicSource, from, to, duration, 0, curve, completeCallback: () =>
        {
            if (autoStop)
                musicSource.clip = null;
        });
    }

    private void LoadVolumeSettings()
    {
        float musicVolume;
        if (PlayerPrefs.HasKey("MusicVolume"))
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        else
        {
            musicVolume = 1f;
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }

        soundSources.ForEach((t) => t.volume = musicVolume);
        musicSource.volume = musicVolume;
        if(musicSlider)
            musicSlider.value = musicVolume;
    }
    private IEnumerator TweenAudioVolume(AudioSource source, float volumeTo, float delay)
    {
        yield return new WaitForSeconds(delay);
        iTween.AudioTo(gameObject, iTween.Hash(
            "audiosource", source,
            "volume", volumeTo,
            "time", 1f,
            "easetype", iTween.EaseType.easeInOutQuad
        ));
    }
}
