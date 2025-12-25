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
    public string slotInfo;//物品描述

    

    void Start()
    {
        // 初始隐藏物品显示
        if (slotItem == null)
        {
            ItemInSlot.SetActive(false);
        }
    }
    public void ItemOnClicked()
    {
        // 更新物品信息显示
        InventoryManager.UpdateItemInfo(slotInfo);

        // 记录当前选中的物品和槽位ID到背包管理器
        InventoryManager.Instance.selectedItem = slotItem;
        InventoryManager.Instance.selectedSlotID = slotID;

        // 只有选中有效物品时，才显示公共USE按钮
        InventoryManager.Instance.useButton.gameObject.SetActive(slotItem != null);
    }

    public void SetupSlot(Item item)
    {
        slotItem = item; // 给当前槽位的物品赋值
        if (item == null)
        {
            ItemInSlot.SetActive(false);
            slotInfo = "";
            return;
        }

        ItemInSlot.SetActive(true);
        slotImage.sprite = item.itemIamge;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.itemInfomation;
      
    }
}
