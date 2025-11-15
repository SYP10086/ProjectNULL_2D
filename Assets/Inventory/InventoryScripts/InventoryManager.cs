using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

//该脚本即统筹各个脚本，最终实现作用
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }//静态类instance，静态实例，全局唯一

    public Inventory MyBag;//调用我的背包组件,知道是那个背包
    public GameObject slotGrid;//调用物品格组件
    //public Slot slotPrefab;
    public GameObject emptySlot;
    public TMP_Text itemInfromation;//物品描述

    public List<GameObject> slots = new List<GameObject>();

    //下面这段函数用于创建单例，管理游戏的库存系统

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // 销毁整个游戏对象
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景不销毁
        }
    }

    private void OnEnable()//一开始执行的函数
    {
        RefreshItem();//更新背包
        Instance.itemInfromation.text = "";//物品描述无
    }

    //物品描述传入的函数
    public static void UpdateItemInfo(string itemDescription)
    {
        Instance.itemInfromation.text = itemDescription;
    }

    //创建新物品的方法
    //可以获得ItemList里的所有Item信息，并把信息传给slot
    /*public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(Instance.slotPrefab, Instance.slotGrid.transform.position, Quaternion.identity);
        //指定位置和旋转：在position和rotation处克隆对象，position是三维坐标值，rotation是旋转值即角度（上述代码表示角度不变）
        //Instantiate(original, position, rotation);
        newItem.gameObject.transform.SetParent(Instance.slotGrid.transform);
        //Slot从外面可见其实是脚本，所以要先点出gameObject，才代表生成的新物体的组件。把这个组件建在格子下
        newItem.slotItem = item;//物品传入Slot
        newItem.slotImage.sprite = item.itemIamge;//物品素材传入
        newItem.slotNum.text = item.itemHeld.ToString();//物品持有数量传入
    }*/

    //刷新，本质是摧毁然后重新生成。主要解决持有数量的显示问题
    public static void RefreshItem()
    {
        //破坏
        for(int i = 0;i < Instance.slotGrid.transform.childCount;i++)
        {
            if(Instance.slotGrid.transform.childCount == 0)//格为空，就跳出循环
                break;
            Destroy(Instance.slotGrid.transform.GetChild(i).gameObject);//格不为空，就破坏所有格
            Instance.slots.Clear();
        }
        //再生成
        for(int i = 0;i < Instance.MyBag.itemList.Count;i++)
        {
            //CreateNewItem(Instance.MyBag.itemList[i]);
            Instance.slots.Add(Instantiate(Instance.emptySlot));
            Instance.slots[i].transform.SetParent(Instance.slotGrid.transform);
            Instance.slots[i].GetComponent<Slot>().slotID = i;
            Instance.slots[i].GetComponent<Slot>().SetupSlot(Instance.MyBag.itemList[i]);
        }
    }
}
