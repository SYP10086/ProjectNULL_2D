using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
     public void SafeExitGame()
    {
        PlayerPrefs.Save();
        StopAllCoroutines();
        Application.Quit();
    }
    public void ExitGame()
    {
        StopAllCoroutines();
        Application.Quit();
    }
    public void SafeBackStart()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("Start");
    }
    public void BackStart()
    {
        SceneManager.LoadScene("Start");
    }
}
