using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SlotForInsert", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite slotImage;
    public int itemTotalHeld;
    [TextArea]
    public string itemInfo;
    public bool equiped;
    public int itemSlotCapacity;
    public int ID;
}
