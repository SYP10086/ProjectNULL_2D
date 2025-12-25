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

    public UnityEngine.UI.Button useButton;

    public List<GameObject> slots = new List<GameObject>();

    
    // 记录当前选中的物品和槽位ID
    public Item selectedItem;
    public int selectedSlotID;

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

            // 初始化物品列表为18个空槽
            while (MyBag.itemList.Count < 18)
            {
                MyBag.itemList.Add(null);
            }                                                                                                                                                                                                              
        }
    }

    void Start()
    {
        // 绑定USE按钮点击事件，初始隐藏按钮
        if (useButton != null)
        {
            useButton.onClick.AddListener(OnUseButtonClick);
            useButton.gameObject.SetActive(false);
        }
    }


    private void OnEnable()//一开始执行的函数
    {
        RefreshItem();//更新背包
        if (itemInfromation != null)
        {
            Instance.itemInfromation.text = "";
        }
        // 打开背包时隐藏USE按钮
        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
        }
    }

    //物品描述传入的函数
    public static void UpdateItemInfo(string itemDescription)
    {
        if (Instance.itemInfromation != null)
        {
            Instance.itemInfromation.text = itemDescription;
        }
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


    // 公共USE按钮点击逻辑
    void OnUseButtonClick()
    {
        if (selectedItem == null || selectedSlotID < 0 || selectedSlotID >= MyBag.itemList.Count)
        {
            return;
        }

        // 根据道具类型执行对应效果
        switch (selectedItem.itemType)
        {
            case Item.ItemType.HealthPill:
                // 回血：不超过生命值上限
                PlayerMain.nowHealthy = Mathf.Min(PlayerMain.nowHealthy + selectedItem.effectValue, PlayerMain.healthy);
                break;
            case Item.ItemType.HealthMax:
                // 增加生命上限，同步当前生命值
                PlayerMain.healthy += selectedItem.effectValue;
                PlayerMain.nowHealthy = PlayerMain.healthy;
                break;
            case Item.ItemType.Attack:
                // 增加体力上限，同步当前体力
                PlayerMain.stamina += selectedItem.effectValue;
                PlayerMain.nowStamina = PlayerMain.stamina;
                break;
            case Item.ItemType.SpeedUp:
                // 增加移动速度
                PlayerMain.speedthis += selectedItem.effectValue;
                PlayerMain.speed = PlayerMain.speedthis;
                break;
        }

        // 减少道具数量
        selectedItem.itemHeld--;

        // 数量为0时，从背包中移除该道具（置为null）
        if (selectedItem.itemHeld <= 0)
        {
            MyBag.itemList[selectedSlotID] = null;
        }

        // 刷新背包显示，隐藏USE按钮
        RefreshItem();
        useButton.gameObject.SetActive(false);
    }

    public static void RefreshItem()
    { // 清除现有格子
        for (int i = 0; i < Instance.slotGrid.transform.childCount; i++)
        {
            Destroy(Instance.slotGrid.transform.GetChild(i).gameObject);
        }
        Instance.slots.Clear();

        // 固定生成18个格子
        int fixedSlotCount = 18;
        for (int i = 0; i < fixedSlotCount; i++)
        {
            GameObject newSlot = Instantiate(Instance.emptySlot, Instance.slotGrid.transform);
            newSlot.transform.localScale = Vector3.one;
            Instance.slots.Add(newSlot);

            Slot slotScript = newSlot.GetComponent<Slot>();
            slotScript.slotID = i;

            // 给格子赋值（直接用原始Item，无类型冲突）
            Item itemToSet = (i < Instance.MyBag.itemList.Count) ? Instance.MyBag.itemList[i] : null;
            slotScript.SetupSlot(itemToSet);
        }
    }
}
