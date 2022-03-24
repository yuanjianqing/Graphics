using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rubbish", menuName = "Interaction/New RubbishList")]
public class RubbishList : ScriptableObject
{
    public Rubbish[] rubbishList;
}
