using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDir : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, player.transform.rotation.eulerAngles.y, 0f);
    }
}
