using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//这个脚本用来具体显示物品素材。也就是获得物品时，在背包里显示
public class Slot : MonoBehaviour
{
    public int slotID;//空格ID即物品ID

    public Item slotItem;//获得物品
    public Image slotImage;//获得物品素材
    public TMP_Text slotNum;//物品数量

    public GameObject ItemInSlot;
    public string slotInfo;
    //物品描述
    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo);
    }

    public void SetupSlot(Item item)
    {
        if(item == null)
        {
            ItemInSlot.SetActive(false);
            return;
        }

        slotImage.sprite = item.itemIamge;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.itemInfomation;
    }
}
