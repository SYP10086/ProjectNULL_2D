using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputField : Save0
{
    public static string Place;
    public static TMP_InputField inputField; 
    public static bool showInput;
    public static GameObject inputFieldPrefab;
    Scene scene;
    // Start is called before the first frame update
    private void Awake()
    {
        inputFieldPrefab = GameObject.Find("InputField (TMP)");
        inputField = GetComponent<TMP_InputField>();
        showInput = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!showInput)
        {
            inputFieldPrefab.SetActive(false);
        }  
    }
    public void CancelTheCreate()
    {
        PlayerMain.speed = 10;
        showInput =false;
        saveWorking = false;
    }
    
    public void CheckInput()
    {
        if (!string.IsNullOrEmpty(inputFieldPrefab.GetComponent<TMP_InputField>().text ?? null))
        {
           saveName = inputFieldPrefab.GetComponent<TMP_InputField>().text;
            
            SetSaveName();
            scene = SceneManager.GetActiveScene();
            Place = scene.name;
            Save.SavePlayer(Number);
            Save.SaveItem(Number);
            //Save0.buttonText.text = PlayerPrefs.GetString($"saveName{Save0.Number}", "Empty") + PlayerPrefs.GetString($"time{Save0.Number}", "");
            saveWorking = false;
            showInput = false;
        }
    }
}
