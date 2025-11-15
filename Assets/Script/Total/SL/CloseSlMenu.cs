using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseSlMenu : AllNeed
{
    [SerializeField] bool SenceChange = false;

    CloseSlMenu()
    {
        Event.LocalChange+= new MyDel(HideSLmenu);
    }
    ~CloseSlMenu()
    {
        Event.LocalChange -= new MyDel(HideSLmenu);
    }
    void Update()
    {

        if (!SenceChange)
        {
            if(Event.SpeedRe!=null)
                Event.SpeedRe();
            SenceChange = true;
            HideSLmenu(); 
        }

    }
}
