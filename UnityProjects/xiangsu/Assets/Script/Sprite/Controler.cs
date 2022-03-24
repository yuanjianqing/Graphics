using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{

    [Header("��ɫ����ײ��, ����")]
    protected Rigidbody2D rb;
    protected BoxCollider2D coll;

    [Header("�˶�����")]
    public float speed = 3f;    //�ٶ�
    public float jumpForce = 10f; // ��Ծ�߶�
    public float gravity = -0.5f;   // ����
    public float Direction // �ƶ��ķ���
    {
        set
        {
            direction = value;
        }
    }


    [Header("״̬")]
    public bool isGrounded;
    public bool isJumping;
    public bool isFiring;
    public bool isGunHeld;
    
    private bool isJumpPressed;
    

    [Header("������")]
    public LayerMask ground = 1 << 8;




    [Header("����")]
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
        //��ɫ����ת��

        Vector2 colliderOffset = new Vector2((coll.offset.x + coll.size.x/2) * trans.localScale.x, coll.offset.y - coll.size.y / 2);

        //�ж��Ƿ����
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
        //����direction��Ϊ����ʩ���ٶ�
        if (!isFiring)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //��Ծ
        if (isJumpPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumpPressed = false;
        }
        
        //��ɫ����ת��
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


    //�������ӵ�����Ķ�����ͨ��event���ô˺����������Ϊfalse
    public void SetFirefalse()
    {
        isFiring = false;
    }

    public void SetGunHeld()
    {
        isGunHeld = !isGunHeld;
    }
}
