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
public class UISoundCountroller : MonoBehaviour
{
	public Slider soundSlider;
	public Image volume;

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
				float Mvolume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
				masterMixer.SetFloat("MasterParameter", Mvolume);
				break;
			case VolumeType.BGM:
				float Bvolume = Mathf.Log10(Mathf.Clamp(value,0.001f,1f)) * 20f;
				masterMixer.SetFloat("BGMParameter",Bvolume);
				break;
			case VolumeType.SFX:
				float Svolume = Mathf.Log10(Mathf.Clamp(value,0.001f,1f)) * 20f;
				masterMixer.SetFloat("SFXParameter",Svolume);
				break;
			default:
				break;
		}
		
	}


}
