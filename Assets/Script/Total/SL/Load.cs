using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static  class Load
{
    public static void LoadSetting()
    {
        ScreenManager.ScreenSize = PlayerPrefs.GetInt("ScreenSize", 0);
        ScreenManager.FullScreen = (PlayerPrefs.GetInt("FullScreen", 1)==1);
        VoiceOfHit.voiceOfHit = PlayerPrefs.GetFloat("VoiceOfHit", 0.5f);
        VoiceOfBGM.voiceOfBGM = PlayerPrefs.GetFloat("VoiceOfBGM", 0.5f);
    }
    public static void LoadPlayer(int SaveNumber)
    {
                InputField.Place = PlayerPrefs.GetString($"Place{SaveNumber}", "NoThisPlace");
                PlayerMain.Healthy = PlayerPrefs.GetFloat($"Healthy{SaveNumber}", 100f);
                PlayerMain.Stamina = PlayerPrefs.GetFloat($"Stamina{SaveNumber}", 100f);
                PlayerMain.y =PlayerPrefs.GetFloat($"transform.position.y{SaveNumber}", 0);
                PlayerMain.x = PlayerPrefs.GetFloat($"transform.position.x{SaveNumber}", 0);
    }
}
