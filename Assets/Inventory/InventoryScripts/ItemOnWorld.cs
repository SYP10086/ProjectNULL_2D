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
        }
    }

    private void AddNewItem()//添加物品的函数
    {
        if(!PlayerInventory.itemList.Contains(thisItem))//如果玩家背包里没有该物品
        {
            //PlayerInventory.itemList.Add(thisItem);//玩家背包里添加该物品
            //InventoryManager.CreateNewItem(thisItem);
            for(int i = 0; i < PlayerInventory.itemList.Count; i++)
            {
                if (PlayerInventory.itemList[i] == null)
                {
                    PlayerInventory.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else//如果已经有了该物品
        {
            thisItem.itemHeld += 1;//则物品持有量加1
        }
        InventoryManager.RefreshItem();//只要添加物品，就执行此函数。每次拾取物体就刷新背包
    }
}
