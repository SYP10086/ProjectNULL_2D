using System.Collections;
using System.Collections.Generic;
using TMPro;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load0 : MonoBehaviour
{
    GameObject[] monster = new GameObject[15];
    Button button;
    TMP_Text buttonText;
    Scene scene;
    GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TMP_Text>();
        int Number = int.Parse(button.name.Substring(4, 1));
        if (Getname(scene)=="Start"&&PlayerPrefs.GetString($"Place{Number}", null) != "FirstScene")
            buttonText.text = "«ÎΩ¯»Î”Œœ∑∂¡µµ";
        else if (PlayerPrefs.GetString($"saveName{Number}", "ø’") != "ø’")
        {
            buttonText.text = PlayerPrefs.GetString($"saveName{Number}", "ø’") + PlayerPrefs.GetString($"time{Number}", "");
        }
        else
            buttonText.text = "ø’";
        for (int i = 0; i < 15; i++)
        {
            monster[i] = GameObject.Find($"AllMonster/Monster{i}");
        }
    }
    public void PressButtonInMain()
    {
        if(!ControlMenu.SaveOrLoad)
        {
            if (buttonText.text != "ø’"&& buttonText.text!="«ÎΩ¯»Î”Œœ∑∂¡µµ")
            {
                DontDestroyOnLoad(this);
                int Number = int.Parse(button.name.Substring(4, 1));
                PlayerMain.haveGravity= false;
                Load.LoadPlayer(Number);
                Load.LoadItem(Number);
                Time.timeScale = 1;
                Debug.Log(InputField.Place);
                if (InputField.Place != "NoThisPlace" && InputField.Place != Getname(scene))
                {
                    if (Getname(scene) != "Start")
                    { 
                        player.GetComponent<PlayerSceneData>().targetSpawnPos = new Vector3(PlayerMain.x, PlayerMain.y, 0);
                        SceneManager.LoadScene(InputField.Place);
                    }
                    else
                    {
                        SceneManager.LoadScene("FirstScene");
                    }
                }
                
                Death.HideDeath();
                if(monster!=null)
                foreach (GameObject a in monster)
                {
                    if (a == null) continue;
                    if (!a.activeSelf)
                    {
                        a.SetActive(true);
                    }
                }
                Event.LocalChange();
                PlayerMain.initLocation = false;
                Destroy(this);
            }
        }
    }
    string Getname(Scene scene)
    {
        scene = SceneManager.GetActiveScene();
        return scene.name;
    }
}
