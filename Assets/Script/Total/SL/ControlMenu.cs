using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ControlMenu : AllNeed
{
    static public bool SaveOrLoad;
    ControlMenu()
    {
        Event.Death += new MyDel(OpenSLmenuInGameLoad);
    }
    ~ControlMenu()
    {
        Event.Death -= new MyDel(OpenSLmenuInGameLoad);
    }
    public void CloseSLmenuInGame()
    {
        Save0.saveWorking = false;
        InputField.showInput = false;
        Event.SpeedRe();
        HideSLmenu();

    }
    public void OpenSLmenuInGameLoad()
    {
        Event.CleanSpeed();
        SaveOrLoad = false;
        ShowSLmenu();
    }
    public void OpenSLmenuInGameSave()
    {
        Event.CleanSpeed();
        SaveOrLoad = true;
        ShowSLmenu();
    }
    public void CloseSLmenu()
    {
        ShowFirstUI();
        HideSLmenu();
        
    }
    public void OpenSLmenu()
    {
        SaveOrLoad = false;
        ShowSLmenu();
        HideFirstUI();
    }
}
