using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

//该脚本用于存储物品信息
[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/New Item")]//这行代码的作用大概是自己可以创建一个物体，该物体有下面代码的信息，也就是自定义物体
public class Item : ScriptableObject
{
    public string itemName;//名字
    public Sprite itemIamge;//物品的素材图片
    public int itemHeld;//物品的持有数量
    [TextArea]//让物品描述可以无限长，而不是仅局限于一行
    public string itemInfomation;//物品描述
    public bool equip;//是否装备

    // 添加道具类型
    public enum ItemType { HealthPill, HealthMax, Attack, SpeedUp }
    public ItemType itemType;

    // 道具效果值
    public float effectValue;
}
