using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerHealth
{


    [Header("��ɫ����")]
    public static int health = 100;


    public static void Kill(int value)
    {
        health -= value;
    }
}
