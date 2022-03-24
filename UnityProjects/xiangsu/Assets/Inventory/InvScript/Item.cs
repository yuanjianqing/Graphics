using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{

    public ItemType type;

    public string itemName;
    public Sprite Image;
    [TextArea]
    public string itemInfo;
}
