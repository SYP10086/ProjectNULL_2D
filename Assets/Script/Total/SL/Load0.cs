using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load0 : MonoBehaviour
{
    Button button;
    TMP_Text buttonText;
    Scene scene;
    private void Start()
    {
        button = GetComponent<Button>();
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
            }
        }
    }
    string Getname(Scene scene)
    {
        scene = SceneManager.GetActiveScene();
        return scene.name;
    }
}
