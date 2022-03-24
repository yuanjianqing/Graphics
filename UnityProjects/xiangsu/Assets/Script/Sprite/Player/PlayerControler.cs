using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : Controler
{

    SpriteRenderer render;
    //角色互动的长度
    float handDistance = 0.7f;

    //角色举起物品的位置
    Vector3 liftPosition;

    //获得的物品的transform  rigibody2D Boxcollider2D
    Transform itemTransform;
    Rigidbody2D itemRb;
    Collider2D itemColl;

    bool pickPressed;


    [Header("状态")]
    public bool isLifting;
    public bool isHurting;
    public bool isDying;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();

        BaseAwake();
    }
    


    void Update()
    {
        // 设置方向
        if (!isHurting)
        {
            SetDirection(Input.GetAxisRaw("Horizontal"));


            SetJumpPressFlag(Input.GetButtonDown("Jump"));


            PhysicCheck(transform);

            if (Input.GetButtonDown("Weapon"))
            {
                SetGunHeld();
            }

            SetFire(Input.GetButtonDown("Fire"));

            BaseUpdate();

            //射线检测是否有物体，如果有就获取transform，将物体固定到角色头顶        //被击落的ribbish设置为layer ground，可以踩着

            //按下拾取按键后再发射射线
            pickPressed = Input.GetButtonDown("Pick");

            if(pickPressed && isGrounded && !isGunHeld && !isLifting)
            {
                RaycastHit2D hit = Raycast(Vector2.zero, new Vector2(-transform.localScale.x, 0f), handDistance, ground, transform);

                if (hit != false && !isLifting && hit.collider.CompareTag("toLift"))
                {
                    //获得举起的物品的collider（后面举起的时候关闭collider防止卡在矮墙）获取rigibody2D是为了把物品丢出去
                    itemColl = hit.collider;
                    itemTransform = hit.transform;

                    itemRb = hit.rigidbody;

                    itemColl.enabled = false;

                    isLifting = true;
                }
            }else if(pickPressed && isLifting)
            {
                Debug.Log("Pick");
                //再次按下Pick按键后  延迟0.01s恢复collider（延迟是为了防止挤压player的碰撞体）   丢出去
                StartCoroutine( ItemCollEnable( itemColl));

                itemRb.velocity = new Vector2(-transform.localScale.x * 5f, 3f);

                //lift状态重新设为false
                isLifting = false;

                //清空指针
                itemTransform = null;
                itemColl = null;
                itemRb = null;

            }

            
            if (isLifting)
            {
                liftPosition = transform.position + new Vector3(0, handDistance, 0);
                itemTransform.position = liftPosition;
                itemTransform.localScale = transform.localScale;
            }
            



        }

    }
    private void FixedUpdate()
    {
        if (!isHurting)
        {
            BaseMovement();
        }

    }

    //碰撞体检测是否被打中



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rubbish") && !isHurting && PlayerHealth.health > 0)
        {

            PlayerHealth.Kill(1);

            //受伤后改变状态，把isLifting设置为false，把itemTransform设置为null
            isLifting = false;
            itemTransform = null;


            if (PlayerHealth.health > 1)
            {
                Debug.Log("PlayerHealth: " + PlayerHealth.health);

                isHurting = true;

                rb.velocity = new Vector2(2 * transform.localScale.x, 0);
                render.color = new Color(0.94f, 0.6f, 0.7f, 1f);
                Invoke("TurnWhite", 0.5f);
            }
            else
            {
                Debug.Log("PlayerHealth: " + PlayerHealth.health);

                isDying = true;

                render.color = Color.gray;
                rb.gravityScale = 0.02f;

                rb.velocity = new Vector2(0, 0.5f);

                coll.offset += new Vector2(0, 0.15f);
                coll.size = new Vector2(coll.size.y, coll.size.x);
                enabled = false;
            }

        }
    }



    //方法
    void TurnWhite()
    {
        render.color = Color.white;
    }
    //协程

    IEnumerator ItemCollEnable(Collider2D itemColl)
    {
        yield return new WaitForSeconds(0.01f);

        itemColl.enabled = true;
    }



}
