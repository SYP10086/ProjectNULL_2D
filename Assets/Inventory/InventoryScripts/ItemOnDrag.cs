using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform OriginalParent;

    public Inventory myBag;
    public int currentItemID;//当前物品ID

    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalParent = transform.parent;//获取初位置
        currentItemID = OriginalParent.GetComponent<Slot>().slotID;//物品ID
        transform.SetParent(transform.parent.parent);//父级调高，使其显示在上层
       transform.position = eventData.position;//鼠标位置
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;//鼠标位置
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image")//如果鼠标在其他格子上，则执行代码，交换
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);//改变父级
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;//改变自己位置

                var temp = myBag.itemList[currentItemID];
                myBag.itemList[currentItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;


                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = OriginalParent.position;//调换位置
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(OriginalParent.parent);//调换父级
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
            {
                //在空白格子上执行该代码
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[currentItemID];
                if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID != currentItemID)
                {
                    myBag.itemList[currentItemID] = null;
                }

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }
        //其他位置回到原位置
        transform.SetParent(OriginalParent);
        transform.position = OriginalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
