using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseSlMenu : AllNeed
{
    [SerializeField] bool SenceChange = false;

    void Update()
    {

        if (!SenceChange)
        {
            Event.SpeedRe();
            SenceChange = true;
            HideSLmenu(); 
        }

    }
}
