using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//这个代码用于世界上的物品，当玩家靠近与物体碰撞时，会将物品加入背包
public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;//Item型变量，用于代指即将拾取的物品
    public Inventory PlayerInventory;//背包，玩家背包

    private void OnTriggerEnter2D(Collider2D other)//检测碰撞
    {
        if(other.gameObject.CompareTag("Player"))//如果标签为Player的碰撞，则执行下述代码
        {
            AddNewItem();//添加物品（函数）
            Destroy(gameObject);//并破坏，使其在地图上消失
            // 强制刷新背包显示
            InventoryManager.RefreshItem();
        }
    }

    // 将道具添加到玩家背包
    // 将道具添加到玩家背包（修复：不实例化，直接修改原始Item）
    private void AddNewItem()
    {
        bool added = false;

        // 第一步：尝试叠加到已有相同道具
        for (int i = 0; i < PlayerInventory.itemList.Count; i++)
        {
            Item existingItem = PlayerInventory.itemList[i];
            // 只匹配原始Item（避免实例化导致的类型问题）
            if (existingItem != null && existingItem == thisItem)
            {
                existingItem.itemHeld++;
                added = true;
                break;
            }
        }

        // 第二步：没有相同道具，添加到空槽位（直接添加原始Item）
        if (!added)
        {
            for (int i = 0; i < PlayerInventory.itemList.Count; i++)
            {
                if (PlayerInventory.itemList[i] == null)
                {
                    // 取消Instantiate，直接赋值原始Item
                    thisItem.itemHeld = 1;
                    PlayerInventory.itemList[i] = thisItem;
                    added = true;
                    break;
                }
            }
        }
    }
}
