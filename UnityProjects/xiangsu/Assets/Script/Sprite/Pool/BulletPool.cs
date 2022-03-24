using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    public GameObject bulletPrefab;

    public int shadowCount = 6;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public float lastShoot;
    public float shootGap = 0.5f;

    private void Awake()
    {
        instance = this;

        lastShoot = Time.time - shootGap;//��ʼ����ʼ�����ʱ��


        //��ʼ�������
        FillPool();
    }

    public void FillPool()
    {
        for(int i = 0; i < shadowCount; i++)
        {
            var newBullet = Instantiate(bulletPrefab);
            newBullet.transform.SetParent(transform);



            //ȡ�����ã����ض����
            ReturnPool(newBullet);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObjects.Enqueue(gameObject);
    }

    public GameObject GetFromPool()
    {
        if(Time.time - lastShoot >= shootGap)
        {
            lastShoot = Time.time;
            if (availableObjects.Count == 0)  //����Ϊ�գ�Ԥ���岻�����ڸ���һЩ����
            {
                FillPool();
            }

            var outBullet = availableObjects.Dequeue();

            outBullet.SetActive(true);

            return outBullet;
        }
        return null;
    }
}
