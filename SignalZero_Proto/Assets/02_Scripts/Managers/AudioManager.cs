using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } //테스트용

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource; // BGM 전용
    [SerializeField] private AudioSource sfxSource; // SFX 전용
    [SerializeField] private AudioSource loopSource; // Loop 전용

    [Serializable]
    public class SoundEntry
    {
        public string key;
        public AudioClip clip;
    }

    [SerializeField] private List<SoundEntry> soundList = new List<SoundEntry>();

    private Dictionary<string, AudioClip> soundDict;

    void Awake()
    {
        Instance = this;  // 테스트용

        soundDict = new Dictionary<string, AudioClip>();

        foreach (var s in soundList)
            soundDict[s.key] = s.clip;
    }

    // -----------------------------------------
    //  SFX 재생
    // -----------------------------------------

    public void PlaySFX(string key)
    {
       if(soundDict.TryGetValue(key, out var clip))
            sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // -----------------------------------------
    //  BGM 재생
    // -----------------------------------------

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (bgmSource.clip == clip)
        {
            if (!bgmSource.isPlaying)
                bgmSource.Play();
            return;
        }

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }


    // -----------------------------------------
    //  Loop 사운드 관리
    // -----------------------------------------

    public void PlayLoop(AudioClip clip)
    {
        if (clip == null) return;
        loopSource.clip = clip;
        if (!loopSource.isPlaying) loopSource.Play();
    }

    public void StopLoop()
    {
        loopSource.Stop();
    }
}
