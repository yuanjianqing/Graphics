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

        lastShoot = Time.time - shootGap;//初始化开始射击的时间


        //初始化对象池
        FillPool();
    }

    public void FillPool()
    {
        for(int i = 0; i < shadowCount; i++)
        {
            var newBullet = Instantiate(bulletPrefab);
            newBullet.transform.SetParent(transform);



            //取消启用，返回对象池
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
            if (availableObjects.Count == 0)  //队列为空，预制体不够，在复制一些出来
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
