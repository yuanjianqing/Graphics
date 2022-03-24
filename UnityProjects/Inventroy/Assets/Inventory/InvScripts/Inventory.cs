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
    


 
    //创建空物体，捡到物品时把查询slotList，把捡到的物品的属性赋值给空物体


    public int FindItemInInventoryIndex(int ID) //返回持有的物品,且同类物品中持有数量最少的Index值
    {
        //如果没有找到的话返回-1
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

    public int FindItemInInventoryIndex(Item item) //返回持有的物品Index值
    {
        //如果没有找到的话返回-1
        return item.ID;
    }

    public int FindEmptySlotIndex()
    {
        //如果没有找到的话返回-1
        int target = -1;
        for (int i = 0; i < inventoryCapacity; i++)
        {
            if (slotList[i].empty)  //image为空时，表示这是一个空项目
            {
                target = i;
                break;
            }
        }
        return target;
    }

    //把source中的内容填入target中
    public void FillEmptySlot(Item item)//把item中的物体填入空的slot中，填完后empty标识改为false，数量+1
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
            //后续放入Ui中的“已存满背包”的相关错误提示
            Debug.Log("背包已满");
        }
    }

   

    public void AddHeldNum(int index)
    {
        //slot中的数量 + 1
        slotList[index].slotHeld += 1;

        //item中总数量 + 1
        itemList[slotList[index].ID].itemTotalHeld += 1;
    }

    public void AddItemToSlotList(Item item)
    {
        int index = FindItemInInventoryIndex(item.ID); // Index为临时变量，用于保存找到的Index，防止反复寻找

        if (index == -1) // -1为没找到的标志
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

///////////////////为测试方便用，后期删除////////////////////////////////
        slotList.Clear();
        if (slotList.Count < inventoryCapacity)
        {
            for (int i = 0; i < inventoryCapacity; i++)
            {
                string slotName = string.Concat("slot ", i, ".asset");
                slotList.Add((SlotForInsert)AssetDatabase.LoadAssetAtPath(string.Concat("Assets/Inventory/New Inventory/Slot/", slotName), typeof(SlotForInsert)));
                
 ///////////为测试方便用，后期删除////////////////////////////////
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

