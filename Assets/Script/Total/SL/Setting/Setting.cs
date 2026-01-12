using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : AllNeed
{

    public void OpenSettingInGame()
    {
        if (settingUI.activeSelf)
        {
            Save.SaveSetting();
            HideSetingUI();
        }
        else
        {
            Load.LoadSetting();
            ShowSetingUI();
        }
    }

    public void OpenSetting()
    {
        Load.LoadSetting();
        ShowSetingUI();
        if (settingUI != null)
        HideFirstUI();
    }
}
