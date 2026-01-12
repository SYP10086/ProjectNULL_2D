using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceOfBGM : MonoBehaviour
{
    public static float voiceOfBGM;
    Slider sliderBGM;
    // Start is called before the first frame update
    void Start()
    {
        sliderBGM = GetComponent<Slider>();
        sliderBGM .value = voiceOfBGM;
    }
    void Update()
    {
        voiceOfBGM = sliderBGM.value;
    }
}
