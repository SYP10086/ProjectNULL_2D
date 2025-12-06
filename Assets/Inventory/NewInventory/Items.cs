using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Items : MonoBehaviour
{
    public Inventory MyBag;
    List<Item> myItems=new List<Item>();
    ItemOnWorld itemOnWorld;
    Item item;
    public int itemHeld;
    public string itemName;
    public bool equip;
    private void Start()
    {
        myItems =MyBag.itemList;
    }
    void ItemStart()
    {
        myItems = MyBag.itemList;
        foreach (Item item in myItems)
        {
            itemName = item.itemName;
            item.itemHeld = 0;
            item.equip = false;
        }
    }
    void Save(int Number)
    {
        myItems = MyBag.itemList;
        foreach (Item item in myItems)
        {
            if(item==null) continue;
            itemName = item.itemName;
            PlayerPrefs.SetInt($"{itemName}Held{Number}", item.itemHeld);
            PlayerPrefs.SetInt($"{itemName}Equip{Number}", item.equip ? 1 : 0);
        }
        
    }
    void Load(int Number)
    {
        myItems = MyBag.itemList;
        foreach (Item item in myItems)
        {
            if (item == null) continue;
            itemName = item.itemName;
            item.itemHeld = PlayerPrefs.GetInt($"{itemName}Held{Number}", 0);
            item.equip = (PlayerPrefs.GetInt($"{itemName}Equip{Number}", 0) == 0) ? true : false;
        }
    }
    //void ItemUse(string name)
    //{
    //    if (name != itemName) return;
    //    item.itemHeld--;
    //}
    Items()
    {
        Debug.Log("Items");
        Event.Start += new MyDel(ItemStart);
        Event.Save += new MyInt(Save);
        Event.Load += new MyInt(Load);
    }
    ~Items()
    {
        Debug.Log("~Items");
        Event.Start -= new MyDel(ItemStart);
        Event.Save -= new MyInt(Save);
        Event.Load -= new MyInt(Load);
    }
}
