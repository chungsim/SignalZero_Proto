using UnityEngine;


[CreateAssetMenu(fileName ="AudioSO", menuName = "AudioData/WeaponAudioSO")]
public class AudioData : ScriptableObject
{
    public AudioClip fireSFX;
    public float fireVolume = 1f;
    public AudioClip impactSFX;
    public float impactVolume = 1f;
}
