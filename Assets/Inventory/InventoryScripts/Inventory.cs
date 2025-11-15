using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//所有人的背包都用这个代码，无论是NPC还是玩家
[CreateAssetMenu(fileName = "New Inventory",menuName = "Inventory/New Inventory")]//同Item里自定义物体，这里自定义“背包”
public class Inventory : ScriptableObject
{
   public List<Item> itemList = new List<Item>();//Item的列表，使得创建的每一个物体都在这个List里
    //具体表现出来，就是，每获得一个物品（该物品肯定是在Item里定义过的），List计数加1，然后把物品加入List
}
