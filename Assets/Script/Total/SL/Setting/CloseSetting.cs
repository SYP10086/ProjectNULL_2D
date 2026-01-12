using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseSetting : AllNeed
{
    bool A = false;
    void Update()
    {
        if(!A)
        {
            A = true;
            CloseSettingUIInGame();
        }
    }
    public void CloseSettingUIInGame()
    {
        Save.SaveSetting();
        HideSetingUI();
    }
    public void CloseSettingUI()
    {
        Save.SaveSetting();
        HideSetingUI();
        ShowFirstUI();
    }
}
