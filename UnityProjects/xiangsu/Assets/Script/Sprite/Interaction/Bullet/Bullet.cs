using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float gunToBodyVerticalDistance = 0.1f;
    private float gunToBodyHorizontalDistance = 0.4f;
    private Transform playerTransfrom;
    Animator anim;
    

    private Rigidbody2D rb;


    [Header("时间控制参数")]
    public float activeTime;//显示时间
    public float activeStart;//开始显示的时间点

    [Header("子弹参数")]
    public float bulletSpeed = 10f;
    public float bulletLength = 0.3f;
    private bool isBombing;


    //子弹爆炸东画的哈希值
    int BombID;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        BombID = Animator.StringToHash("isBombing");
    }
    private void OnEnable()
    {
        playerTransfrom = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = playerTransfrom.position + new Vector3(-gunToBodyHorizontalDistance * playerTransfrom.localScale.x, gunToBodyVerticalDistance, 0);
        transform.localScale = playerTransfrom.localScale;
        transform.rotation = playerTransfrom.rotation;

        activeStart = Time.time;

        //防止第二颗子弹发生在第一颗子弹爆炸，第二颗子弹卡壳的bug
        isBombing = false;

    }

    void Update()
    {
        if(Time.time >= activeStart + activeTime)
        {
            //返回对象池
            BulletPool.instance.ReturnPool(this.gameObject);
        }


        if (!isBombing)
        {
            //判断子弹是否撞墙,利用射线
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -transform.right * transform.localScale.x, bulletLength);

            Color color = hit ? Color.red : Color.green;

            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), -transform.right * transform.localScale.x * bulletLength, color);


            if (hit != false)
            { //如果撞墙，播放动画，然后通过event返回对象池
                if (hit.collider.CompareTag("Wall"))
                {
                    //爆炸
                    anim.SetBool(BombID, true);
                    isBombing = true;
                }
                if (hit.collider.CompareTag("Rubbish"))
                {
                    ItemDamage damage;
                    damage = hit.collider.gameObject.GetComponent<ItemDamage>();
                    damage.Damaging(-transform.right * transform.localScale.x);
                    anim.SetBool(BombID, true);
                    isBombing = true;
                }

            }
        }

    
    }

    private void FixedUpdate()
    {
        //子弹运动
        if (!isBombing)
        {
            rb.velocity = transform.right * bulletSpeed * (-transform.localScale.x);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    public void ReturnPool()
    {
        BulletPool.instance.ReturnPool(this.gameObject);
    }
}
