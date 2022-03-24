using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Rubbish", menuName = "Interaction/New Rubbish")]
public class Rubbish : ScriptableObject
{
    public ItemType type;
    new public string name;
    public Sprite image;
    [TextArea]
    public string itemInfo;
}
