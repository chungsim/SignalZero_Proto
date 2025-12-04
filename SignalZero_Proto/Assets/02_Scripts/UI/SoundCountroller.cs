using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundCountroller : MonoBehaviour
{
     public Slider soundSlider;
     public Image volume;
    private float sliderValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       sliderValue = soundSlider.value;
       volume.fillAmount = sliderValue * 0.1f;
    }


   
}
