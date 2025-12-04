using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] public AudioSource bgmSource; // BGM 전용
    [SerializeField] public AudioSource sfxSource; // SFX 전용
    [SerializeField] public AudioSource loopSource; // Loop 전용

    [Serializable]
    public class SoundEntry
    {
        public string key;
        public AudioClip clip;
    }

    [Header("SFX 사운드 모음집(전역 SFX)")]
    [SerializeField] private List<SoundEntry> soundList = new List<SoundEntry>();
    private Dictionary<string, AudioClip> soundDict;

    [Header("동일 SFX 재생간격 설정")]

    private Dictionary<AudioClip, float> limitedSfxLastTime = new();
    private float limitedCooldown = 0.05f;   // 동일 SFX 최소 재생 간격

    void Awake()
    {
        soundDict = new Dictionary<string, AudioClip>();

        foreach (var s in soundList)
            soundDict[s.key] = s.clip;
    }

    // -----------------------------------------
    //  SFX 재생
    // -----------------------------------------

    // -----------------------------------------
    // 1) 즉시 출력 SFX (발사, 부스터 시작/끝)
    // -----------------------------------------
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    // -----------------------------------------
    // 2) 제한 SFX (몬스터 피격처 중첩 사운드 방지)
    // -----------------------------------------
    public void PlayLimitedSFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        float last;
        limitedSfxLastTime.TryGetValue(clip, out last);

        if (Time.time - last < limitedCooldown)
            return;

        limitedSfxLastTime[clip] = Time.time;

        sfxSource.PlayOneShot(clip, volume);
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
