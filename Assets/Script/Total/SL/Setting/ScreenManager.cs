using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static int ScreenSize;
    public static bool FullScreen=true;
    public TMP_Dropdown dropdown;
    public Toggle toggle;
    private void Start()
    {
        toggle = GetComponent<Toggle>();
        dropdown = GetComponent<TMP_Dropdown>();
        if(toggle!=null)
            toggle.isOn = FullScreen;
        if (dropdown != null)
            dropdown.value = ScreenSize;
    }
    // Update is called once per frame
    public void ChangedScreenSize()
    {
        ScreenSize=dropdown.value;
        SettingScreen();
    }
    public void ChangedFullScreen()
    {
        FullScreen = toggle.isOn;
        SettingScreen();
    }
    void SettingScreen()
    {
        switch (ScreenSize)
        {
            case 0:
                Screen.SetResolution(1920, 1080, FullScreen);
                break;
            case 1:
                Screen.SetResolution(1280, 720, FullScreen);
                break;
            case 2:
                Screen.SetResolution(2560, 1440, FullScreen);
                break;
            case 3:
                Screen.SetResolution(640, 480, FullScreen);
                break;
        }
    }

}
