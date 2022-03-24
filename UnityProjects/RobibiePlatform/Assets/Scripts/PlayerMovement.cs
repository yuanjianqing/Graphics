using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("�ƶ�����")]

    public float speed = 8f;
    public float crouchSpeedDivisor = 3f;

    [Header("��Ծ����")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float hangingJumpForce = 15f;

    [Header("״̬")]

    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isHanging;

    [Header("�������")]
    public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;
    float playerHeight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;

    public LayerMask groundLayer;


    public float xVelocity;

    float jumpTime;

    //����������
    bool jumpPressed;
    bool jumpHeld;
    bool crouchHeld;
    bool crouchPress;

    //��ײ��ĳߴ�
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
        playerHeight = coll.size.y;
    }

    void Update()
    {
        if (!jumpPressed && Input.GetButtonDown("Jump") && (isOnGround || isHanging))
        {
            jumpPressed = true;
        }
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
        if(!crouchPress && Input.GetButtonDown("Crouch") && isHanging)
        {
            crouchPress = true;
        }
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        JumpOrDownMovement();
    }

    void PhysicsCheck()
    {
        //���ҽ�����
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(+footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if (leftCheck || rightCheck)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;

        }

        //����ͷ������
        RaycastHit2D leftHeadCheck = Raycast(new Vector2(-footOffset, coll.size.y), Vector2.up, headClearance, groundLayer);
        RaycastHit2D rightHeadCheck = Raycast(new Vector2(+footOffset, coll.size.y), Vector2.up, headClearance, groundLayer);

        if(leftHeadCheck || rightHeadCheck)
        {
            isHeadBlocked = true;
        }
        else
        {
            isHeadBlocked = false;
        }

        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight),grabDir, grabDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);

        if(!isOnGround && rb.velocity.y < 0f && ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance - 0.05f) * direction ;
            pos.y -= ledgeCheck.distance;

            transform.position = pos;
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }

    void GroundMovement()
    {
        if (isHanging)
            return;

        if (crouchHeld && !isCrouch && isOnGround)
        {
            Crouch();
        }
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
        {
            StandUp();
        }
        else if (!isOnGround && isCrouch && !isHeadBlocked)
            StandUp();
        
        xVelocity = Input.GetAxis("Horizontal");
        
        if(isCrouch)
        {
             xVelocity /= crouchSpeedDivisor;
        }

        
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        FlipDirection();
    }

    void JumpOrDownMovement()
    {
        if(isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);


                isHanging = false;
                jumpPressed = false;
            }

            if(crouchPress)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;

                isHanging = false;
                crouchPress = false;
            }
        }
        if(jumpPressed && isOnGround && !isJump)
        {
            if(isCrouch && !isHeadBlocked)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            //isOnGround = false;
            isJump = true;

            jumpTime = Time.time + jumpHoldDuration;

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse );
            jumpPressed = false;
            AudioManager.PlayJumpAudio();
        }
        else if(isJump)
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

    void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    void StandUp()
    {
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
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
