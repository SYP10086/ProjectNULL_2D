using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceOfHit : MonoBehaviour
{
    public static float voiceOfHit;
    
    Slider sliderHit;
    
    private void Start()
    {
        sliderHit = GetComponent<Slider>();
        sliderHit.value = voiceOfHit;
    }
    void Update()
    {
       voiceOfHit =sliderHit.value  ;
    }
}
