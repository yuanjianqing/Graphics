using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAndBoucing : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 rayDirection;
    LayerMask WallLayer;
    


    //��ת���ٶ�
    float spinningSpeed;

    [Header("�ٶ�")]
    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        WallLayer = 1 << 10 | 1 << 9;  //Wall���ֲ��Player���ֲ�

        //�������һ������45��
        int x = 2 * Random.Range(0, 2) - 1;
        int y = 2 * Random.Range(0, 2) - 1;
        rayDirection = new Vector2(x, y);

        rb.velocity = rayDirection * speed;

        spinningSpeed = Random.Range(-80f, 80f);

    }

    void Update()
    {


        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 0.4f, WallLayer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(transform.position, rayDirection * 0.3f, color);


        
        transform.Rotate(0, 0,  spinningSpeed * Time.deltaTime);


        if (hit != false)
        {
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Rubbish"))
            {
                Bounce(hit);
            }
        }

    }


    //����
    void Bounce(RaycastHit2D hit)
    {
        //�����󷴵�
        rayDirection = Vector2.Reflect(rayDirection, hit.normal);
        rb.velocity = rayDirection * speed;


        //������ı���ת�ٶ�
        spinningSpeed = Random.Range(-80f, 80f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Bounce(Physics2D.Raycast(transform.position, rayDirection));
        }
        if(collision.CompareTag("Rubbish"))
        {
            Bounce(Physics2D.Raycast(transform.position, rayDirection));
        }
    }
}
