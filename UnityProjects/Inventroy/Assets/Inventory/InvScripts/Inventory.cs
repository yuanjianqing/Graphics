using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CreateAssetMenu(fileName = "New SlotForInsert", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public int inventoryCapacity;
    public List<SlotForInsert> slotList = new List<SlotForInsert>();
    public List<Item> itemList = new List<Item>();
    public Item example;
    


 
    //���������壬����Ʒʱ�Ѳ�ѯslotList���Ѽ񵽵���Ʒ�����Ը�ֵ��������


    public int FindItemInInventoryIndex(int ID) //���س��е���Ʒ,��ͬ����Ʒ�г����������ٵ�Indexֵ
    {
        //���û���ҵ��Ļ�����-1
        int target = -1;
        int minNum = itemList[ID].itemSlotCapacity;
        for (int i = 0; i < inventoryCapacity; i++)
        {
            if(slotList[i] != null)
                if (ID == slotList[i].ID && minNum >= slotList[i].slotHeld)
                {
                    target = i;
                }
        }
        return target;
    }

    public int FindItemInInventoryIndex(Item item) //���س��е���ƷIndexֵ
    {
        //���û���ҵ��Ļ�����-1
        return item.ID;
    }

    public int FindEmptySlotIndex()
    {
        //���û���ҵ��Ļ�����-1
        int target = -1;
        for (int i = 0; i < inventoryCapacity; i++)
        {
            if (slotList[i].empty)  //imageΪ��ʱ����ʾ����һ������Ŀ
            {
                target = i;
                break;
            }
        }
        return target;
    }

    //��source�е���������target��
    public void FillEmptySlot(Item item)//��item�е���������յ�slot�У������empty��ʶ��Ϊfalse������+1
    {
        /* public string itemName;
         public Sprite slotImage;
         public int slotHeld;
         public string itemInfo;
         public bool equiped;
         public int ID;
         public int itemCapacity;
         public bool empty = true;
        */
        int emptyIndex = FindEmptySlotIndex();

        if (emptyIndex != -1)
        {

            slotList[emptyIndex].ID = item.ID;
            slotList[emptyIndex].empty = false;
            AddHeldNum(emptyIndex);
        }
        else
        {
            //��������Ui�еġ��Ѵ�������������ش�����ʾ
            Debug.Log("��������");
        }
    }

   

    public void AddHeldNum(int index)
    {
        //slot�е����� + 1
        slotList[index].slotHeld += 1;

        //item�������� + 1
        itemList[slotList[index].ID].itemTotalHeld += 1;
    }

    public void AddItemToSlotList(Item item)
    {
        int index = FindItemInInventoryIndex(item.ID); // IndexΪ��ʱ���������ڱ����ҵ���Index����ֹ����Ѱ��

        if (index == -1) // -1Ϊû�ҵ��ı�־
        {
            FillEmptySlot(item);
        }
        else
        {
            if (slotList[index].slotHeld + 1 > item.itemSlotCapacity)
            {
                FillEmptySlot(item);
            }
            else
            {
                AddHeldNum(index);
            }
        }
    }

    public void AddItemToItemList(Item item)
    {
        int index = FindItemInInventoryIndex(item);
        if(index == -1)
        {
            FillEmptySlot(item);
        }
    }

    public void Replace(int index, SlotForInsert slot)
    {
        slotList.RemoveAt(index);
        slotList.Insert(index, slot);
    }


   


    public void InitializeSlotList()
    {
        /*if (slotList.Count < inventoryCapacity)
        {
            for (int i = 0; i < inventoryCapacity; i++)
            {
                slotList.Add(slotItem);
            }
        }*/
        //;

///////////////////Ϊ���Է����ã�����ɾ��////////////////////////////////
        slotList.Clear();
        if (slotList.Count < inventoryCapacity)
        {
            for (int i = 0; i < inventoryCapacity; i++)
            {
                string slotName = string.Concat("slot ", i, ".asset");
                slotList.Add((SlotForInsert)AssetDatabase.LoadAssetAtPath(string.Concat("Assets/Inventory/New Inventory/Slot/", slotName), typeof(SlotForInsert)));
                
 ///////////Ϊ���Է����ã�����ɾ��////////////////////////////////
                slotList[i].empty = true;
                slotList[i].ID = example.ID;
                slotList[i].slotHeld = 0;
            }
        }
        for(int i = 0; i < itemList.Count; i++)
        {
            itemList[i].itemTotalHeld = 0;
        }
        
        
    }









}

