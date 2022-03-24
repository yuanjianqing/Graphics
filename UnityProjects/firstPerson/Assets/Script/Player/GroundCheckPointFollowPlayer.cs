using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckPointFollowPlayer : MonoBehaviour
{
    Transform trans;
    private void Awake()
    {
        trans = GetComponent<Transform>();
    }
    void Update() 
    { 
        trans.rotation = Quaternion.Euler(0f, 0f, 0f); 
    }
}
