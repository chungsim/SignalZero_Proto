using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum VolumeType
{
	Master,
	BGM,
	SFX
}
public class SoundCountroller : MonoBehaviour
{
	public Slider soundSlider;
	public Image volume;
	private float sliderValue;

	public AudioMixer masterMixer;
	public VolumeType volumeType;
	// Start is called before the first frame update
	void Start()
	{
		soundSlider.onValueChanged.AddListener(OnSliderValueChanged);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnSliderValueChanged(float value)
	{
		volume.fillAmount = value * 0.1f;
		switch (volumeType)
		{
			case VolumeType.Master:
				float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
				masterMixer.SetFloat("MasterParameter", volume);
				break;
			case VolumeType.BGM:
				//GameManager.Instance.audioManager.bgmSource.volume = value;
				break;
			case VolumeType.SFX:
				//GameManager.Instance.audioManager.sfxSource.volume = value;
				break;
			default:
				break;
		}
		
	}


}
