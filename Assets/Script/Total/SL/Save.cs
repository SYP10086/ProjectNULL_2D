using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save
{
    // Start is called before the first frame update
    
    public static void SaveSetting()
    {
        PlayerPrefs.SetInt("ScreenSize", ScreenManager.ScreenSize);
        PlayerPrefs.SetInt("FullScreen", ScreenManager.FullScreen ?1:0);
        PlayerPrefs.SetFloat("VoiceOfHit", VoiceOfHit.voiceOfHit);
        PlayerPrefs.SetFloat("VoiceOfBGM", VoiceOfBGM.voiceOfBGM);

        PlayerPrefs.Save();
    }
    public static void SavePlayer(int SaveNumber)
    {
        PlayerPrefs.SetString($"Place{SaveNumber}", InputField.Place);
<<<<<<< Updated upstream
        PlayerPrefs.SetFloat($"Healthy{SaveNumber}", PlayerMain.healthy);
        PlayerPrefs.SetFloat($"Stamina{SaveNumber}", PlayerMain.stamina);
=======
        PlayerPrefs.SetFloat($"Healthy{SaveNumber}", PlayerMain.Healthy);
        PlayerPrefs.SetFloat($"Stamina{SaveNumber}", PlayerMain.Stamina);
>>>>>>> Stashed changes
        PlayerPrefs.SetFloat($"transform.position.x{SaveNumber}", PlayerMain.transform.position.x);
        PlayerPrefs.SetFloat($"transform.position.y{SaveNumber}", PlayerMain.transform.position.y);
        PlayerPrefs.SetString($"time{SaveNumber}", " "+System.DateTime.Now);
        PlayerPrefs.SetString($"saveName{SaveNumber}", Save0.saveName);
        PlayerPrefs.Save();
    }
    public static void SaveItem(int SaveNumber)
    {
        if(Event.Save!=null)
        Event.Save(SaveNumber);
    }
}
