using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : AllNeed
{
    private void Start()
    {
        Load.LoadSetting();
    }
    public void StartNewGame()
    {
        PlayerMain.StartPlayer();
        SceneManager.LoadScene("Sence1");
        
    }

}
