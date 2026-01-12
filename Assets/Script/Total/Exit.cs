using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public static GameObject settingUI;
    public static GameObject SLmenu;

    private void Start()
    {
        settingUI = GameObject.Find("SettingMenu");
        SLmenu= GameObject.Find("SLMenu");
    }
    public void SafeExitGame()
    {
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
        SLmenu.SetActive(false);
        SceneManager.LoadScene("Start");
    }
    public void BackStart()
    {
        SLmenu.SetActive(false);
        SceneManager.LoadScene("Start");
    }
}
