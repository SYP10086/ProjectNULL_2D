using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Save0 : MonoBehaviour
{
    Button button;
    public static bool saveWorking = false;
    public static string saveName;
    static protected TMP_Text[] buttonText=new TMP_Text[9];
    public static int Number;//the number of sellscted
    int i;//Each button local number
    private void Start()
    {
        button = GetComponent<Button>();
        i = int.Parse(button.name.Substring(4, 1));
        buttonText[i-1] = button.GetComponentInChildren<TMP_Text>();
    }
    public void PressButton()
    {
        if (ControlMenu.SaveOrLoad&&!saveWorking)
        {
            saveWorking = true;
            InputField.showInput = true;
            InputField.inputFieldPrefab.SetActive(true);
            Number = i;
            SetInput();
        }
    }
    void SetInput()
    {
        if (buttonText[Number - 1].text != "¿Õ")
            InputField.inputField.text = PlayerPrefs.GetString($"saveName{Number}", "¿Õ");
        else
            InputField.inputField.text = "ÇëÊäÈë´æµµÃû×Ö(ÃüÃûÎª¡°¿Õ¡±»áÉ¾³ý´æµµ)";
    }
    protected void SetSaveName()
    {
        if (!(saveName == "¿Õ"))
            buttonText[Number - 1].text = saveName + " " + $"{System.DateTime.Now}";
        else
            buttonText[Number - 1].text = "¿Õ";
    }
}
