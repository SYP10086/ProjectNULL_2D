using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : AllNeed
{
    private void Awake()
    {
        Time.timeScale =0;
    }
    public void StartNewGame()
    {
        Time.timeScale = 1;
        PlayerMain.haveGravity = false;
        Event.Start?.Invoke();
        SceneManager.LoadScene("FirstScene");
    }

}
