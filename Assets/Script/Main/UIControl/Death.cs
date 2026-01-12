using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    static GameObject Text;
    // Start is called before the first frame update
    Death()
    {
        Event.LocalChange += new MyDel(HideDeath);
    }
    ~Death()
    {
        Event.LocalChange -= new MyDel(HideDeath);
    }
    void Start()
    {
        Text = GameObject.Find("DeathT");
        HideDeath();
    }
    static public void ShowDeath()
    {
        Text.SetActive(true);
    }
    static public void HideDeath()
    {
        Text.SetActive(false);
    }
    public void PressIt()
    {
        if (!PlayerMain.death) return;
        PlayerMain.nowHealthy=1;
        PlayerMain.death=false;
        SceneManager.LoadScene("Start");
    }
}
