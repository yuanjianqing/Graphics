using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New SlotForInsert", menuName = "Inventory/New slot")]
public class SlotForInsert : ScriptableObject
{
    public int ID = -1;
    public int slotHeld;
    public bool empty = true;

    public SlotForInsert(Item item, int initialSlotHeld)
    {
        ID = item.ID;
        slotHeld = initialSlotHeld;
        empty = false;
    }
}
