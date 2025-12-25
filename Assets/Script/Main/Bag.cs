using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public GameObject MyBag;//获取背包组件
    bool IsOpen = false;//是否打开背包


    void Update()
    {
       OpenMyBag();
    }
    void OpenMyBag()
    {
        if(Input.GetKeyDown(KeyCode.B))//按下B键打开背包
        {
            IsOpen = !IsOpen;
            MyBag.SetActive(IsOpen);
        }
        InventoryManager.RefreshItem();
    }
}
