using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamage : MonoBehaviour
{

    Rigidbody2D rb;
    SpinningAndBoucing SB;
    BoxCollider2D coll;

    public float moveDistance;

    private SpriteRenderer render;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SB = GetComponent<SpinningAndBoucing>();
        coll = GetComponent<BoxCollider2D>();

        render = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damaging(Vector3 rayDirection)
    {
        //不让物体旋转和弹跳
        SB.enabled = false;

        //给失重的物体添加重力,并加大Mass（重力
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 10f;

        //让碰撞体取消triger状态，不然不会停留在地板上
        coll.isTrigger = false;

        rb.velocity = rayDirection * moveDistance;

        render.color = Color.gray;

        tag = "toLift";
        //把已经被破坏的物体的layer设置为地面 （第层），player可以在上面跳跃
        gameObject.layer = 8;

        //scene0管理中的 item数量减1

        Scene0.totalNum += 1;
    }
}
