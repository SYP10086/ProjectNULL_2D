using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackStart : MonoBehaviour
{
public void BackStart1()
    {
        SceneManager.LoadScene("Start");
    }
    public void BackStart2()
    {
        Application.Quit();
    }
}
