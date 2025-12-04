using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAudioSO", menuName = "AudioData/PlayerAudioSO")]
public class PlayerAudioData : ScriptableObject
{
    public AudioClip boosterStartSFX;
    public float boosterStartVolume = 1f;

    public AudioClip boosterLoopSFX;
    public float boosterLoopVolume = 1f;

    public AudioClip boosterEndSFX;
    public float boosterEndVolume = 1f;

    // 필요하면 점프, 데미지, 사망음 등도 여기 추가 가능
}

