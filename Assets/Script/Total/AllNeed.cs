using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AllNeed : MonoBehaviour
{
    public static GameObject startUI;
    public static GameObject settingUI;
    public static GameObject SLmenu;


    void Start()
    {
        settingUI = GameObject.Find("SettingMenu");
        startUI = GameObject.Find("StartUI");
        SLmenu= GameObject.Find("SLMenu");
        Load.LoadSetting();

    }
    protected void HideFirstUI()
    {
            if (startUI != null)
                startUI.SetActive(false);
    }
    protected void ShowFirstUI()
    {
            if (startUI != null)
                startUI.SetActive(true);
    }
    protected void HideSetingUI()
    {

        if (settingUI != null)
            settingUI.SetActive(false);
    }
    protected void ShowSetingUI()
    {
        if (settingUI != null)
            settingUI.SetActive(true);
    }
    protected void ShowSLmenu()
    {
        if (SLmenu != null)
            SLmenu.SetActive(true);
    }
    protected void HideSLmenu()
    {
        if (SLmenu != null)
            SLmenu.SetActive(false);
    }
}
