using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Bag : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    public GameObject MyBag;//获取背包组件
    bool IsOpen = true;//是否打开背包
    bool init = false;

    void Update()
    {
        if (!init) keybd_event(0x42, 0, 0, 0);
        OpenMyBag();
        if (!init)
        {
            init = true;
            keybd_event(0x42, 0, 2, 0);
        }
    }
    void OpenMyBag()
    {
        if(Input.GetKeyDown(KeyCode.B))//按下B键打开背包
        {
            IsOpen = !IsOpen;
            MyBag.SetActive(IsOpen);
        }
    }
}
