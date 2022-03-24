using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RubbishDic", menuName = "Inventory/New Inventory/New RubbishDic")]
public class ItemDic : ScriptableObject
{


    public Dictionary<ItemType, Rubbish> RubbishDictionary = new Dictionary<ItemType, Rubbish>();
    


    public Rubbish[] RubbishSprites;

    public void OnEnable()
    {
        //Debug.Log("nihao");
    }


}
