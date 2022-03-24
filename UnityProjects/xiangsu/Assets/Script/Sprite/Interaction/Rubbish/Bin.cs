using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("toLift"))
        {
            anim.SetBool("isDistroying", true);
            Scene0.nowScreenNum -= 1;
            Destroy(collision.gameObject);
        }
        
    }

    public void SetDistroyFalse()
    {
        anim.SetBool("isDistroying", false);
    }
}
