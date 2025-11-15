using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load0 : MonoBehaviour
{
    Button button;
    TMP_Text buttonText;
    Scene scene;
    GameObject[] monster;
    private void Start()
    {
        button = GetComponent<Button>();
        monster = GameObject.FindGameObjectsWithTag("Monster");
        buttonText = button.GetComponentInChildren<TMP_Text>();
        int Number = int.Parse(button.name.Substring(4, 1));
        if (PlayerPrefs.GetString($"saveName{Number}", "Пе") != "Пе")
            buttonText.text = PlayerPrefs.GetString($"saveName{Number}", "Пе") + PlayerPrefs.GetString($"time{Number}", "");
        else
            buttonText.text = "Пе";
    }
    public void PressButtonInMain()
    {
        if(!ControlMenu.SaveOrLoad)
        {
            if (buttonText.text != "Пе")
            {
                int Number = int.Parse(button.name.Substring(4, 1));
                Load.LoadPlayer(Number);
                if(InputField.Place != "NoThisPlace"&& InputField.Place!= Getname(scene))
                SceneManager.LoadScene(InputField.Place);
                PlayerMain.initLocation = false;
                foreach (GameObject a in monster)
                {
                    Debug.Log("Setactive"+a);
                    a.SetActive(true);
                    a.GetComponent<Monster>().Init();
                }
                Event.LocalChange();
                Event.SpeedRe();
            }
        }
    }
    string Getname(Scene scene)
    {
        scene = SceneManager.GetActiveScene();
        return scene.name;
    }
}
