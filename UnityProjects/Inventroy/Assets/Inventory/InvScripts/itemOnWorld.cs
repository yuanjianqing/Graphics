using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class itemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerInventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }


    public void AddNewItem()
    {
        //Debug.Log(playerInventory.FindItemInInventoryIndex(thisItem));
        playerInventory.AddItemToSlotList(thisItem);

        /*if(!playerInventory.slotList.Contains(thisItem))
        {
            playerInventory.slotList.Add(thisItem);
            //playerInventory.slotList[thisItem.ID];
            thisItem.slotHeld += 1;
            InventoryManager.CreateNewItem(thisItem);
            
        }
        else
        {
            thisItem.slotHeld += 1;
            InventoryManager.RefreshItem(thisItem);
        }*/
        InventoryManager.RefreshItem();
    }
}
