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
        volume.fillAmount = value / 10f;

        float normalized = value / 10f;
        float dB = Mathf.Log10(Mathf.Clamp(normalized, 0.0001f, 1f)) * 20f;

        switch (volumeType)
        {
            case VolumeType.Master:
                masterMixer.SetFloat("MasterParameter", dB);
                break;
            case VolumeType.BGM:
                masterMixer.SetFloat("BGMParameter", dB);
                break;
            case VolumeType.SFX:
                masterMixer.SetFloat("SFXParameter", dB);
                break;
        }
    }



}
