using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{

    [Header("角色的碰撞体, 刚体")]
    protected Rigidbody2D rb;
    protected BoxCollider2D coll;

    [Header("运动参数")]
    public float speed = 3f;    //速度
    public float jumpForce = 10f; // 跳跃高度
    public float gravity = -0.5f;   // 重力
    public float Direction // 移动的方向
    {
        set
        {
            direction = value;
        }
    }


    [Header("状态")]
    public bool isGrounded;
    public bool isJumping;
    public bool isFiring;
    public bool isGunHeld;
    
    private bool isJumpPressed;
    

    [Header("层遮罩")]
    public LayerMask ground = 1 << 8;




    [Header("方向")]
    public float  direction = 0;

    //
    
    
    private float footDistance = -0.5f;
    private float rayLength = 0.02f;
    //

    public void BaseAwake()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        ground = 1 << 8;
        footDistance = coll.offset.y - coll.bounds.size.y/2;
    }



    public void PhysicCheck(Transform trans)
    {
        //角色方向转向

        Vector2 colliderOffset = new Vector2((coll.offset.x + coll.size.x/2) * trans.localScale.x, coll.offset.y - coll.size.y / 2);

        //判断是否落地
        RaycastHit2D hit = Raycast(colliderOffset, Vector2.down, rayLength, ground, trans);

        isGrounded = hit ? true : false;
        if (isGrounded)
        {
            isJumping = false;
        }
        else
        {
            isJumping = true;
        }
    }

    
    public void BaseMovement()
    {
        //利用direction来为物体施加速度
        if (!isFiring)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //跳跃
        if (isJumpPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumpPressed = false;
        }
        
        //角色方向转向
        FlipDirection();
    }

    public void BaseUpdate()
    {

        if (isFiring)
        {
            Fire();
        }
    }


    static public RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer, Transform trans)
    {
        Vector2 pos = trans.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDirection * length, color);

        return hit;
    }

    public void SetDirection(float value)
    {
        direction = value;
    }

    public void SetJumpPressFlag(bool isTrue)
    {
        if (!isJumpPressed && isGrounded)
        {
            isJumpPressed = isTrue;
        }
    }


    void FlipDirection()
    {
        if (direction > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (direction < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Fire()
    {
        BulletPool.instance.GetFromPool();
        isFiring = true;
    }

    public void SetFire(bool flag)
    {
        if(!isFiring && flag && isGunHeld)
        {
            isFiring = true;
        }
    }


    //播放完子弹射击的动画后，通过event调用此函数将射击设为false
    public void SetFirefalse()
    {
        isFiring = false;
    }

    public void SetGunHeld()
    {
        isGunHeld = !isGunHeld;
    }
}
