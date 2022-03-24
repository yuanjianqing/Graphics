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


    [Header("ʱ����Ʋ���")]
    public float activeTime;//��ʾʱ��
    public float activeStart;//��ʼ��ʾ��ʱ���

    [Header("�ӵ�����")]
    public float bulletSpeed = 10f;
    public float bulletLength = 0.3f;
    private bool isBombing;


    //�ӵ���ը�����Ĺ�ϣֵ
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

        //��ֹ�ڶ����ӵ������ڵ�һ���ӵ���ը���ڶ����ӵ����ǵ�bug
        isBombing = false;

    }

    void Update()
    {
        if(Time.time >= activeStart + activeTime)
        {
            //���ض����
            BulletPool.instance.ReturnPool(this.gameObject);
        }


        if (!isBombing)
        {
            //�ж��ӵ��Ƿ�ײǽ,��������
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -transform.right * transform.localScale.x, bulletLength);

            Color color = hit ? Color.red : Color.green;

            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), -transform.right * transform.localScale.x * bulletLength, color);


            if (hit != false)
            { //���ײǽ�����Ŷ�����Ȼ��ͨ��event���ض����
                if (hit.collider.CompareTag("Wall"))
                {
                    //��ը
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
        //�ӵ��˶�
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
