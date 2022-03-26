using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    [Header("角色的碰撞体, 刚体")]
    protected Rigidbody2D rb;
    protected BoxCollider2D coll;
    public float groundDistance = 0.2f;
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;

    [Header("运动参数")]
    public float speed = 3f;    //速度
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

    [Header("层遮罩")]
    public LayerMask groundLayer;

    [Header("方向")]
    public float direction = 0;

    private float footDistance = -0.5f;
    //



    public void PhysicCheck(Transform trans)
    {
        //碰撞体的一半
        float halfCX = colliderStandSize.x / 2;
        float halfCY = colliderStandSize.y / 2;
        float direction = transform.localScale.x;
        //脚射线
        RaycastHit2D footCheck = Raycast((colliderStandOffset - new Vector2(halfCX, halfCY)) * new Vector2(direction, 1), Vector2.down, groundDistance, groundLayer, trans);

        isGrounded = footCheck ? true : false;
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
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        //角色方向转向
        FlipDirection();
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

    void FlipDirection()
    {
        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }



    public void BaseAwake()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        footDistance = coll.offset.y - coll.bounds.size.y / 2;
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
    }

    public void BaseUpdate()
    {
        BaseMovement();
    }
}
