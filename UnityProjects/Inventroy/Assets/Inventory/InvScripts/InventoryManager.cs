using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;
    public Inventory myBag;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Text itemInformation;
    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;

        //��ʼ��inventory�б�
        myBag.InitializeSlotList();
    }

    private void OnEnable()
    {
        RefreshItem();
    }
    public static void CreateNewItem(SlotForInsert slot)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform,false);
        newItem.slotItem = slot;
        if(slot.ID != -1 ) //���slotΪ��ʱ��Ҳ����ID = -1�����Ͳ�Ҫ��newItem��ͼ���ֵ��
            newItem.slotImage.sprite = instance.myBag.itemList[slot.ID].slotImage;
        newItem.slotNum.text = slot.slotHeld.ToString();
        newItem.empty = slot.empty;
        if (newItem.empty)
            newItem.gameObject.SetActive(false);
        
        //newItem.ID = item.ID;
    }

    /*public static void RefreshItem1(SlotForInsert item)
    {
        for(int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            FindItem(item).slotNum.text = item.slotHeld.ToString();
        }
    }*/

    /*public static void LoadInItem()
    {
        for(int i = 0; i < instance.myBag.slotList.Count; i++)
        {
            CreateNewItem(instance.myBag.slotList[i]);
        }
    }*/

    /*public static Slot FindItem(SlotForInsert item)
    {
        Slot target = null;
        for(int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if(instance.slotGrid.transform.GetChild(i).gameObject.GetComponent<Slot>().ID == item.ID)
            {
                target = instance.slotGrid.transform.GetChild(i).gameObject.GetComponent<Slot>();
                break;
            }
        }
        return target;
    }*/

    public static void RefreshItem()
    {
        for(int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < instance.myBag.inventoryCapacity; i++)
        {
            CreateNewItem(instance.myBag.slotList[i]);
        }
    }


}
