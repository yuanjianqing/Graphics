using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("移动参数")]

    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float hangingJumpForce = 15f;

    [Header("状态")]
    public bool isOnGround;
    public bool isJump;
    public bool isHanging;

    [Header("环境检测")]
    public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;
    float playerHeight;
    public float eyeDis = 1f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;

    public LayerMask groundLayer;


    public float xVelocity;

    float jumpTime;

    //按键的设置
    bool jumpPressed;
    bool jumpHeld;

    //碰撞体的尺寸
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
    }

    void Update()
    {
        if (!jumpPressed && Input.GetButtonDown("Jump") && (isOnGround || isHanging))
        {
            jumpPressed = true;
        }
        jumpHeld = Input.GetButton("Jump");

    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        JumpOrDownMovement();
    }

    void PhysicsCheck()
    {
        //碰撞体的一半
        float halfCX = colliderStandSize.x / 2;
        float halfCY = colliderStandSize.y / 2;
        float direction = transform.localScale.x;
        //左右脚射线
        RaycastHit2D footCheck = Raycast((colliderStandOffset - new Vector2(halfCX, halfCY)) * new Vector2(direction, 1), Vector2.down, groundDistance, groundLayer);
        if (footCheck)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;

        }



        
        Vector2 grabDir = new Vector2(direction, 0f);

        //RaycastHit2D blockedCheck = Raycast((colliderStandOffset + new Vector2(halfCX, halfCY)) * new Vector2(direction, 1), grabDir, grabDistance, groundLayer);
        //RaycastHit2D wallCheck = Raycast((colliderStandOffset + new Vector2(halfCX, halfCY - eyeDis)) * new Vector2(direction, 1), grabDir, grabDistance, groundLayer);
        //RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);

        /*if (!isOnGround && rb.velocity.y < 0f && ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance - 0.05f) * direction;
            pos.y -= ledgeCheck.distance;

            transform.position = pos;
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }*/
    }

    void GroundMovement()
    {
        if (isHanging)
            return;


        xVelocity = Input.GetAxis("Horizontal");



        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        FlipDirection();
    }

    void JumpOrDownMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);


                isHanging = false;
                jumpPressed = false;
            }


        }
        if (jumpPressed && isOnGround && !isJump)
        {


            rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            //isOnGround = false;
            isJump = true;

            jumpTime = Time.time + jumpHoldDuration;

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpPressed = false;
            /////////////////AudioManager.PlayJumpAudio();
        }
        else if (isJump)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if (jumpTime < Time.time)
            {
                isJump = false;
            }
        }
    }

    void FlipDirection()
    {
        if (xVelocity < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (xVelocity > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }





    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDirection * length, color);

        return hit;
    }
}
