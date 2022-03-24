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
        //����������ת�͵���
        SB.enabled = false;

        //��ʧ�ص������������,���Ӵ�Mass������
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 10f;

        //����ײ��ȡ��triger״̬����Ȼ����ͣ���ڵذ���
        coll.isTrigger = false;

        rb.velocity = rayDirection * moveDistance;

        render.color = Color.gray;

        tag = "toLift";
        //���Ѿ����ƻ��������layer����Ϊ���� ���ڲ㣩��player������������Ծ
        gameObject.layer = 8;

        //scene0�����е� item������1

        Scene0.totalNum += 1;
    }
}
