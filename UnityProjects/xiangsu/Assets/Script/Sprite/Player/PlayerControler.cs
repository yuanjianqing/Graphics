using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : Controler
{

    SpriteRenderer render;
    //��ɫ�����ĳ���
    float handDistance = 0.7f;

    //��ɫ������Ʒ��λ��
    Vector3 liftPosition;

    //��õ���Ʒ��transform  rigibody2D Boxcollider2D
    Transform itemTransform;
    Rigidbody2D itemRb;
    Collider2D itemColl;

    bool pickPressed;


    [Header("״̬")]
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
        // ���÷���
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

            //���߼���Ƿ������壬����оͻ�ȡtransform��������̶�����ɫͷ��        //�������ribbish����Ϊlayer ground�����Բ���

            //����ʰȡ�������ٷ�������
            pickPressed = Input.GetButtonDown("Pick");

            if(pickPressed && isGrounded && !isGunHeld && !isLifting)
            {
                RaycastHit2D hit = Raycast(Vector2.zero, new Vector2(-transform.localScale.x, 0f), handDistance, ground, transform);

                if (hit != false && !isLifting && hit.collider.CompareTag("toLift"))
                {
                    //��þ������Ʒ��collider����������ʱ��ر�collider��ֹ���ڰ�ǽ����ȡrigibody2D��Ϊ�˰���Ʒ����ȥ
                    itemColl = hit.collider;
                    itemTransform = hit.transform;

                    itemRb = hit.rigidbody;

                    itemColl.enabled = false;

                    isLifting = true;
                }
            }else if(pickPressed && isLifting)
            {
                Debug.Log("Pick");
                //�ٴΰ���Pick������  �ӳ�0.01s�ָ�collider���ӳ���Ϊ�˷�ֹ��ѹplayer����ײ�壩   ����ȥ
                StartCoroutine( ItemCollEnable( itemColl));

                itemRb.velocity = new Vector2(-transform.localScale.x * 5f, 3f);

                //lift״̬������Ϊfalse
                isLifting = false;

                //���ָ��
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

    //��ײ�����Ƿ񱻴���



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rubbish") && !isHurting && PlayerHealth.health > 0)
        {

            PlayerHealth.Kill(1);

            //���˺�ı�״̬����isLifting����Ϊfalse����itemTransform����Ϊnull
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



    //����
    void TurnWhite()
    {
        render.color = Color.white;
    }
    //Э��

    IEnumerator ItemCollEnable(Collider2D itemColl)
    {
        yield return new WaitForSeconds(0.01f);

        itemColl.enabled = true;
    }



}
